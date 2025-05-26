using UnityEngine;

public class CurrentPLayerDB : MonoBehaviour
{
    public static CurrentPLayerDB Instance { get; private set;}
    public Usuarios CurrentPlayer;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else Destroy(gameObject);
    }
}
