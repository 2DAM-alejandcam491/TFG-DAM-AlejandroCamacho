using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public HUD hud;
    public int PuntosTotales { get{return puntosTotales;}}
    private int puntosTotales;
    private int vidas = 3;
    private Animator animator;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this; 
        } else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentPLayerDB.Instance.CurrentPlayer != null) puntosTotales = DBManager.Instance.ObtenerPuntuacion();
    }

    public void SumarPuntos(int puntosASumar){
        puntosTotales += puntosASumar;
        hud.ActualizarPuntos(puntosTotales);
    }

    // public void perderVida(){
    //     vidas--;
    //     if (vidas == 0){
    //         //Animacion Muerte y Espera
    //         CharacterController.Instance.coroutineAnimacionMuerte();
    //         //Reiniciar Level
    //         SceneManager.LoadScene(0);
    //     }
    //     hud.desactivarVida(vidas);
    //     CharacterController.Instance.coroutineAnimacionRecibirDanio();
    // }

    // public void ganarVida(){
    //     if (vidas == 3){
    //         return;
    //     }
    //     hud.activarVida(vidas++);
    // }

}
