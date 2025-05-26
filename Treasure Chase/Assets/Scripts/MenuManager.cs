using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject AlbumPanelDialog;

    public Button AlbumButton;
    public Button CloseAlbumButton;
    public Button CerrarSesionButton;

    // Tesoros
    public List<TesoroUI> tesorosUI = new List<TesoroUI>();
    public Sprite defaultSprite; // NOTFOUND sprite
    public List<Sprite> spritesTesoro; // Lista de sprites en orden

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start()");
        AlbumButton.onClick.AddListener(ShowAlbum);
        CloseAlbumButton.onClick.AddListener(CloseAlbum);
        CerrarSesionButton.onClick.AddListener(LogOut);
        
        CargarTesoros();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseAlbum()
    {
        // Animacion
        AlbumPanelDialog.transform
        .DOScale(Vector3.zero, 0.2f)
        .SetEase(Ease.InBack)
        .OnComplete(() => AlbumPanelDialog.SetActive(false));
    }

    public void ShowAlbum()
    {
        AlbumPanelDialog.SetActive(true);
        AlbumPanelDialog.transform.localScale = Vector3.zero;

        // Animacion
        AlbumPanelDialog.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);
    }

    public void LogOut()
    {
        CurrentPLayerDB.Instance.CurrentPlayer = null;
        SceneManager.LoadScene("LoginScene");
    }
    
    void CargarTesoros()
    {
        int idUsuario = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;
    
        var _connection = DBManager.Instance.Connection;

        // Obtener todos los tesoros del usuario
        var tesorosObtenidos = _connection.Table<TesorosObtenidos>()
            .Where(t => t.id_usuario == idUsuario).ToList();

        var listaTesoros = _connection.Table<Tesoros>().ToList();

        for (int i = 0; i < tesorosUI.Count; i++)
        {
            int indexTesoro = i + 1;

            if (tesorosObtenidos.Select(t => t.id_tesoro).Contains(indexTesoro))
            {
                tesorosUI[i].imagen.sprite = spritesTesoro[i];
                tesorosUI[i].nombre.text = listaTesoros[i].nombre;
            }
            else
            {
                tesorosUI[i].imagen.sprite = defaultSprite;
                tesorosUI[i].nombre.text = "No encontrado";
            }
        }
    }
}
