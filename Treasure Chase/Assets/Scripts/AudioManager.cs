using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource audioSourceSXF;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirSonido(AudioClip audio, float volume)
    {
        if (audioSourceSXF == null)
        {
            Debug.LogWarning("AudioSource no inicializado.");
            return;
        }
        if (audio == null)
        {
            Debug.LogWarning("AudioClip es null.");
            return;
        }

        audioSourceSXF.PlayOneShot(audio, volume);
    }
}
