using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCharacterController : MonoBehaviour
{
    public static ScriptCharacterController Instance { get; private set;}
    public float velocidad;
    public float fuerzaSalto;
    public int saltosMaximos;
    public int PuntosTotales { get{return puntosTotales;}}
    private int puntosTotales;

    //public AudioClip audioClip;
    
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    public LayerMask capaSuelo;
    private bool mirandoDerecha = true; 
    public int saltosRestantes;
    private Animator animator;


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
    }

    bool EstaEnSuelo(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),0f,Vector2.down,0.2f, capaSuelo);
        return raycastHit.collider != null;
    }

    void ProcesarSalto()
    {
        
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && saltosRestantes > 0 ){
            animator.SetBool("isJumping", true);
            saltosRestantes--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            //AudioManager.Instance.ReproducirSonido(audioClip);
        }

        if (EstaEnSuelo() && rigidBody.velocity.y <= 0){
            saltosRestantes = saltosMaximos;
            animator.SetBool("isJumping", false);
        }
    }

    void ProcesarMovimiento()
    {
        // Logica Movimiento
        float inputMovimiento = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);

        if(inputMovimiento != 0f){
            animator.SetBool("isRunning",true);
        }else{
            animator.SetBool("isRunning",false);
        }
        
        gestionarOrientacion(inputMovimiento);
    }

    void gestionarOrientacion(float inputMovimiento){
        if ((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0) ){
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}
