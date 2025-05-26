using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContoller : MonoBehaviour
{
    private int valor = 5;

    // AUDIO
    public AudioClip audioClip;
    float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            GameManager.Instance.SumarPuntos(valor);
            Destroy(gameObject);
            AudioManager.Instance.ReproducirSonido(audioClip, volume);
            DBManager.Instance.IncrementScoreOnDB(valor);
        }
    }
}
