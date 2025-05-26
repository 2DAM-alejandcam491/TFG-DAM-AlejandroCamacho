using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    //public Sprite spriteVida;
    //public Sprite spriteNoVida;
    public TextMeshProUGUI puntos;
    public GameObject[] vidas;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        BuscarScoreNum();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (puntos != null)
        {
            puntos.text = GameManager.Instance.PuntosTotales.ToString();
        }
    }

    public void ActualizarPuntos(int puntosTotales){
        if (puntos != null)
        {
            puntos.text = puntosTotales.ToString();
        }
        else
        {
            Debug.LogWarning("No se puede actualizar puntos: 'puntos' es null");
        }
    }
    
    public void desactivarVida(int indice){
        //Image spriteController = vidas[indice].GetComponent<Image>();
        //spriteController.sprite = spriteNoVida;
    }

    public void activarVida(int indice){
        //Image spriteController = vidas[indice].GetComponent<Image>();
        //spriteController.sprite = spriteVida;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuscarScoreNum();
    }

    // Busca el componente TMP Text en ScoreNum dentro del Canvas
    private void BuscarScoreNum()
    {
        // Buscar el GameObject ScoreNum dentro del Canvas (que es el objeto donde está este script)
        Transform scoreNumTransform = transform.Find("ScoreNum");
        if (scoreNumTransform != null)
        {
            puntos = scoreNumTransform.GetComponent<TextMeshProUGUI>();
            if (puntos == null)
            {
                Debug.LogWarning("No se encontró TextMeshProUGUI en ScoreNum");
            }
            else
            {
                puntos.text = DBManager.Instance.ObtenerPuntuacion().ToString();
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ScoreNum como hijo de Canvas");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
