using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallingTime;

    public BoxCollider2D boxCollider; // Referenciando o boxCollider.
    public TargetJoint2D joint; // Referenciando o TargetJoin.

    public void Falling()
    {
        boxCollider.enabled = false;
        joint.enabled = false;
        Destroy(gameObject, 3);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Invoke("Falling", fallingTime); // Tempo para a queda da plataforma.
        }
    }
}
