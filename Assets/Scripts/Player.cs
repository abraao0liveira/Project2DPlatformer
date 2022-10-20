using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using TMPro; // Importando biblioteca para os textos.
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    public int life;
    public int apple;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    private Vector2 direction;
    public SpriteRenderer sprite;
    public GameObject gameOver;

    [Header("UI")]
    public TextMeshProUGUI appleText;
    public TextMeshProUGUI lifeText;

    private bool isGrounded; // Quando o personagem esta no chao.
    private bool recovery;
    void Start()
    {
        lifeText.text = life.ToString(); // Armazena a vida no inicio.
        Time.timeScale = 1;

        DontDestroyOnLoad(gameObject); // Mantem os atributos ganhados.
    }
    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Jump(); // Chamando jump.
        PlayerAnim();
    }
    void FixedUpdate() // Usado para a física.
    {
        Movement(); // Chamando movement.
    }

    #region Movement
    void Movement()
    {
        rig.velocity = new Vector2(direction.x * speed, rig.velocity.y); // Limita o personagem ao eixo x.
    }
    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded == true) // somente quando aperta space do espectro da Unity.
        {
            anim.SetInteger("transition", 2);
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // Mecanica do pulo.
            isGrounded = false;
        }
    }
    #endregion

    void Death()
    {
        if(life <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Pausa o Game.
    }

    #region Anim
    void PlayerAnim()
    {
        if(direction.x > 0) // Direita.
        {
            if(isGrounded == true)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = Vector2.zero;
        }
        if(direction.x < 0) // Esquerda.
        {
            if (isGrounded == true)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector2(0, 180); // Virar o personagem.
        }
        if(direction.x == 0) // Parado.
        {
            if (isGrounded == true)
            {
                anim.SetInteger("transition", 0);
            }
        }
    }
    #endregion

    public void Hit() // Metodo.
    {
        if(recovery == false) // Leva dano somente quando não está piscando.
        {
            StartCoroutine(Flick()); // Referenciando o Coroutine
        }
    }
    IEnumerator Flick()
    {
        recovery = true;
        life -= 1; // Dano.
        Death();
        lifeText.text = life.ToString(); // Nova vida.
        sprite.color = new Color(1, 1, 1, 1); // Com cor.
        yield return new WaitForSeconds(0.2f); // espaço de tempo.
        sprite.color = new Color(1, 1, 1, 0); // Sem cor.
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 1);
        recovery = false;
    }
    public void IncreaseScore()
    {
        apple++; // Acresentando mais 1.
        appleText.text = apple.ToString(); // Pegando um int e transformando em string, mostrando no text.
    }
    private void OnCollisionEnter2D(Collision2D collision) // Detecta quando o player encosta em um objeto.
    {
        if(collision.gameObject.layer == 6) // Verifica quando encostar no chão.
        {
            isGrounded = true;
        }
    }
}
