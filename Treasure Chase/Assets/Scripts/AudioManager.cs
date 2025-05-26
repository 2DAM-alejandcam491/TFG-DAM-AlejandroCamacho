using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))  ]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}
    private AudioSource audioSource;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido(AudioClip audio, float volume){
        audioSource.PlayOneShot(audio, volume);
    }
}
