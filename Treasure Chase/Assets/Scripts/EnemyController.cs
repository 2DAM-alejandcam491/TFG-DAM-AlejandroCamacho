using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private Animator animator;
    private bool isPlayerDetected = false;
    private bool mirandoDerecha = true; 

    /// <summary>
    ///  Seguir por aqui
    /// </summary>
    public float attackRange = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerDetected && player != null)
        {
            float currentY = transform.position.y;

            transform.position = Vector2.MoveTowards(
            new Vector2(transform.position.x, currentY),
            new Vector2(player.position.x, currentY),
            speed * Time.deltaTime);
        }
        GestionarOrientacion(player.position.x);
    }

    public void PlayerDetected(Transform playerTransform)
    {
        player = playerTransform;
        isPlayerDetected = true;
        animator.SetBool("PlayerDetected", isPlayerDetected);
    }

    public void PlayerLost()
    {
        isPlayerDetected = false;
        animator.SetBool("PlayerDetected", isPlayerDetected);
        player = null;
    }

    void GestionarOrientacion(float playerPositionX)
    {
        if (playerPositionX < transform.position.x && mirandoDerecha)
        {
            mirandoDerecha = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else if (playerPositionX > transform.position.x && !mirandoDerecha)
        {
            mirandoDerecha = true;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}
