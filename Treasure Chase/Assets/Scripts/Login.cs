using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    private SQLiteConnection _connection;
    public TMP_InputField  usernameInput;  // Referencia al InputField de Usuario
    public TMP_InputField passwordInput;  // Referencia al InputField de Contraseña
    public Button loginButton;  // Referencia al botón de login
    public Button registerButton;  // Referencia al botón de login

    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    public GameObject loginErrorDialogShadow; 
    public GameObject loginErrorDialog; 
    public GameObject registerDialog; 

    void Start()
    {
        Debug.Log("Antes ");
        string dbPath = Path.Combine(Application.streamingAssetsPath, "Prueba.db"); // Al estar en la carpeta 'StreammingsAssets' podemos acceder al fichero directamente por su nombre
        Debug.Log("Despues ");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // Asignar la acción al botón
        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerButton.onClick.AddListener(OnRegisterButtonClick);

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void OnLoginButtonClick() // Al clickar
    {
        string username = usernameInput.text;  // Obtener el texto del campo de usuario
        string password = passwordInput.text;  // Obtener el texto del campo de contraseña

        if (TryLogin(username, password))
        {
            Debug.Log("Inicio de sesión exitoso");
            SceneManager.LoadScene("Scene1");     // Cargar otra escena
        }
        else
        {
            Debug.Log("Credenciales incorrectas");
            ShowLoginError(); // Mostrar Panel
        }
    }

    private void OnRegisterButtonClick() // Al clickar
    {
        ShowRegister();
    }

    public bool TryLogin(string username, string password)
    {
        Debug.Log("Dentro de TryLogin()");
        var user = _connection.Table<Usuarios>().FirstOrDefault(u => u.nombre_usuario  == username && u.contrasenia  == password);
        return user != null;
    }

    void ShowLoginError()
    {
        loginErrorDialog.SetActive(true);
        loginErrorDialog.transform.localScale = Vector3.zero;

        // Animacion
        loginErrorDialog.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);

        loginErrorDialogShadow.SetActive(true);    
    }

    void ShowRegister()
    {
        registerDialog.SetActive(true);
        registerDialog.transform.localScale = Vector3.zero;

        // Animacion
        registerDialog.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);    
    }

    public void CloseLoginError()
    {
        // Anuimacion
        loginErrorDialog.transform
        .DOScale(Vector3.zero, 0.2f)
        .SetEase(Ease.InBack)
        .OnComplete(() => loginErrorDialog.SetActive(false));
        loginErrorDialogShadow.SetActive(false);
    }
    public void CloseRegister()
    {
        // Anuimacion
        registerDialog.transform
        .DOScale(Vector3.zero, 0.2f)
        .SetEase(Ease.InBack)
        .OnComplete(() => registerDialog.SetActive(false));
    }
}