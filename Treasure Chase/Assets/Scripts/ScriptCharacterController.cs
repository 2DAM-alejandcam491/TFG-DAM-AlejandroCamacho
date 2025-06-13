using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacterController : MonoBehaviour
{
    public static ScriptCharacterController Instance { get; private set; }
    public float velocidad;
    public float fuerzaSalto;
    public int saltosMaximos;
    public int PuntosTotales { get { return puntosTotales; } }
    private int puntosTotales;
    public AudioClip audioClipLoseHeart;
    public AudioClip audioClipJump;
    public AudioClip audioClipSword;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    public LayerMask capaSuelo;
    private bool mirandoDerecha = true;
    public int saltosRestantes;
    private Animator animator;
    public GameObject attackHitbox;
    private bool puedeAtacar = true;
    public bool canGetHit = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMaximos;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
        ProcesarAtaque();
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }

    void ProcesarAtaque()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
        {
            Debug.Log("procesarAttack()");
            AudioManager.Instance.ReproducirSonido(audioClipSword, 1);
            animator.SetTrigger(Random.Range(0, 2) == 0 ? "Attack1" : "Attack2");
            puedeAtacar = false;
        }
    }

    void ProcesarSalto()
    {

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && saltosRestantes > 0)
        {
            AudioManager.Instance.ReproducirSonido(audioClipJump, 1);
            animator.SetBool("isJumping", true);
            saltosRestantes--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }

        if (EstaEnSuelo() && rigidBody.velocity.y <= 0)
        {
            saltosRestantes = saltosMaximos;
            animator.SetBool("isJumping", false);
        }
    }

    void ProcesarMovimiento()
    {
        // Logica Movimiento
        float inputMovimiento = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

        if (inputMovimiento != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        gestionarOrientacion(inputMovimiento);
    }

    void gestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public void CoroutineDieAnimation()
    {
        Debug.Log("CoroutineDieAnimation()");
        StartCoroutine(DieAnimation());
    }

    public IEnumerator DieAnimation()
    {
        Debug.Log("Die()");
        yield return new WaitForSeconds(2f); // 2 segundos de espera
    }

    public void CoroutieLoseHeartAnimation()
    {
        Debug.Log("CoroutineLoseHeartAnimation()");
        StartCoroutine(LoseHeartAnimation());
    }

    public IEnumerator LoseHeartAnimation()
    {
        Debug.Log("LoseHeartAnimation()");
        AudioManager.Instance.ReproducirSonido(audioClipLoseHeart, 1);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float duration = 2f;
        float elapsedTime = 0f;
        float blinkInterval = 0.2f;
        bool faded = false;

        while (elapsedTime < duration)
        {
            Color color = sr.color;
            color.a = faded ? 1f : 0.2f; // alterna entre opaco y transparente
            sr.color = color;

            faded = !faded;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        // Asegura que termine totalmente visible
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;
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
        puedeAtacar = true;
    }
}
