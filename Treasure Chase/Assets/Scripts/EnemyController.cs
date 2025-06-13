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

    public float attackRange = 20f;
    public Collider2D attackCollider;
    private bool hasAttacked = false;
    public AudioClip audioClipGetHit;



    void Start()
    {
        animator = GetComponent<Animator>();
        attackCollider.enabled = false;
        InvokeRepeating("EnableAttack", 0f, 2f);
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

            float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);

            if (distanceToPlayer <= attackRange && !hasAttacked)
            {
                hasAttacked = true;
                animator.SetTrigger("Attack");
            }
        }
        GestionarOrientacion(player.position.x);
    }

    void EnableAttack()
    {
        hasAttacked = false;
    }

    public void PlayerDetected(Transform playerTransform)
    {
        player = playerTransform;
        isPlayerDetected = true;
        hasAttacked = false;
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

    public void ActivarColliderDeAtaque()
    {
        Debug.Log("Activar Coll");
        attackCollider.enabled = true;
    }

    public void DesactivarColliderDeAtaque()
    {
        Debug.Log("DesActivar Coll");
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSword"))
        {
            AudioManager.Instance.ReproducirSonido(audioClipGetHit, 1);
            Debug.Log("¡El enemigo ha sido golpeado por el jugador!");
            Destroy(gameObject);
        }
    }
}
