using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public int vidaMaxima = 10; // Cantidad de HITS
    private int vidaActual;

    public GameObject barraVidaUI;
    public Slider sliderVida;

    public Transform jugador;
    public float distanciaActivacion = 100f;

    public float knockbackForce = 5f; // Fuerza del retroceso
    private Rigidbody2D rb;
    private bool mirandoDerecha = true;
    private bool isPlayerDetected = false, hasAttacked = false;
    private Animator animator;
    public GameObject attackHitbox;
    private bool puedeAtacar = true;
    public AudioClip audioClipAttack;
    public float attackRange = 20f;
    public float speed = 2f;
    void Start()
    {
        animator = GetComponent<Animator>();
        vidaActual = vidaMaxima;
        barraVidaUI.SetActive(false); // Se oculta al inicio
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vidaActual;
        InvokeRepeating("EnableAttack", 0f, 3f);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isPlayerDetected && jugador != null)
        {
            float currentY = transform.position.y;

            transform.position = Vector2.MoveTowards(
            new Vector2(transform.position.x, currentY),
            new Vector2(jugador.position.x, currentY),
            speed * Time.deltaTime);

            float distanceToPlayer = Mathf.Abs(jugador.position.x - transform.position.x);

            if (distanceToPlayer <= attackRange && !hasAttacked)
            {
                Debug.Log("Atacar");
                ProcesarAtaque();
            }
        }

        float distancia = Vector2.Distance(jugador.position, transform.position);
        if (distancia < distanciaActivacion)
        {
            barraVidaUI.SetActive(true);
        }
        else
        {
            barraVidaUI.SetActive(false);
        }

        GestionarOrientacion();
    }


    void EnableAttack() {
        hasAttacked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            RecibirDanio(1);
        }
    }

    void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);
        sliderVida.value = vidaActual;

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("El jefe ha muerto.");
        Destroy(gameObject); // o animación de muerte + luego Destroy
        barraVidaUI.SetActive(false);
    }

    void GestionarOrientacion()
    {
        if (jugador == null) return;

        if (jugador.position.x < transform.position.x && mirandoDerecha)
        {
            mirandoDerecha = false;
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (jugador.position.x > transform.position.x && !mirandoDerecha)
        {
            mirandoDerecha = true;
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    public void PlayerDetected(Transform playerTransform)
    {
        jugador = playerTransform;
        isPlayerDetected = true;
    }

    public void PlayerLost()
    {
        isPlayerDetected = false;
        jugador = null;
    }

    public void ActivarAtaque()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }
    }

    public void DesactivarAtaque()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    void ProcesarAtaque()
    {
        hasAttacked = true;
        AudioManager.Instance.ReproducirSonido(audioClipAttack, 1);
        animator.SetTrigger(Random.Range(0, 2) == 0 ? "Attack1" : "Attack2");
    }
}
