using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform point;
    void Start()
    {
        FindObjectOfType<Player>().transform.position = point.position; // Passando a posição do point para o player.
    }
    void Update()
    {
        
    }
}
