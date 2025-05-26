using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;


public class Login : MonoBehaviour
{
    public TMP_InputField  usernameInputLogin;  // Referencia al InputField de Usuario Login
    public TMP_InputField passwordInputLogin;  // Referencia al InputField de Contraseña Login
    public TMP_InputField  usernameInputRegister;  // Referencia al InputField de Usuario Register
    public TMP_InputField passwordInputRegister;  // Referencia al InputField de Contraseña Register
    public TMP_Text InfoPanelLabelTxt;  // Referencia label de InfoPanel
    public Button loginButton;  // Referencia al botón de login
    public Button registerButton;  // Referencia al botón de registro para abrir el panel
    public Button tryRegistrerButton;  // Referencia al botón de registro para abrir el panel
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    public GameObject loginErrorDialogShadow; 
    public GameObject InfoPanelDialog; 
    public GameObject registerDialog; 

    void Start()
    {
        // Asignar la acción al botón
        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        tryRegistrerButton.onClick.AddListener(OnTryRegisterButtonClick);

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void OnLoginButtonClick() // Al clickar
    {
        string username = usernameInputLogin.text;  // Obtener el texto del campo de usuario
        string password = passwordInputLogin.text;  // Obtener el texto del campo de contraseña

        if (TryLogin(username, password))
        {
            Debug.Log("Inicio de sesión exitoso");
            if (CurrentPLayerDB.Instance.CurrentPlayer.es_admin) SceneManager.LoadScene("AdminScene");
            else
            {
                int id = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;
                var progreso = DBManager.Instance.Connection.Table<Progreso>().Where(p => p.id_usuario == id).FirstOrDefault();
                if (progreso.niveles_completados == 0)
                {
                    SceneManager.LoadScene("BaseLevelScene");
                }
                else
                {
                    SceneManager.LoadScene("MenuSelectLevel");
                }
            }
        }
        else
        {
            Debug.Log("Credenciales incorrectas");
            ShowInfo("Nombre de usuario o contraseña incorrectos."); // Mostrar Panel
        }
    }

    private void OnRegisterButtonClick() // Al clickar
    {
        ShowRegister();
    }

    public bool TryLogin(string username, string password)
    {
        Debug.Log("Dentro de TryLogin()");

        var _connection = DBManager.Instance.GetSQLiteConnection();
        
        Debug.Log(_connection.Query<Usuarios>("Select * from usuarios").ToList());
                Debug.Log("Dentro de TryLogin()2");

        var usuarios = _connection.Table<Usuarios>().ToList();
            Debug.Log("Dentro de TryLogin()3");

        foreach (var u in usuarios)
        {
            Debug.Log($"Usuario DB -> ID: {u.id_usuario}, Usuario: {u.nombre_usuario}, Pass: {u.contrasenia}");
        }

        Usuarios user = _connection.Table<Usuarios>().FirstOrDefault(u => u.nombre_usuario == username && u.contrasenia == password);

        if (user != null)
        {
            Debug.Log("Usuario encontrado con ID: " + user.id_usuario);
            
            if (GameManager.Instance != null)
            {
                CurrentPLayerDB.Instance.CurrentPlayer = user;
                Debug.Log("Usuario asignado a GameManager con ID: " + CurrentPLayerDB.Instance.CurrentPlayer.id_usuario);
            }
            else
            {
                Debug.LogError("GameManager.Instance es NULL en TryLogin()");
            }

            return true;
        }
        else
        {
            Debug.LogWarning("No se encontró usuario con ese nombre y contraseña.");
            return false;
        }
    }

    private void OnTryRegisterButtonClick()
    {
        string username = usernameInputRegister.text;  // Obtener el texto del campo de usuario
        string password = passwordInputRegister.text;  // Obtener el texto del campo de contraseña

        Debug.Log(username);
        Debug.Log(password);

        if (TryRegister(username, password))
        {
            Debug.Log("Registrado Correctamente");
            ShowInfo("Se registró correctamente!\nInicie Sesión");
        }
        else
        {
            Debug.Log("Error al registrarse, pruebe de nuevo");
            ShowInfo("Error al registrarse, usuario existente o credenciales erróneas"); // Mostrar Panel
        }
    }

    private bool TryRegister(string username, string password)
    {
        try
        {
            var _connection = DBManager.Instance.Connection;
            
            var usuarios = _connection.Table<Usuarios>().Where(u => u.nombre_usuario == username).ToList();
            Usuarios user = usuarios.FirstOrDefault();
            if (user == null && !String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                Usuarios newUser = new Usuarios
                {
                    nombre_usuario = username,
                    contrasenia = password,
                    correo = username.ToLower() + "@email.com",
                    fecha_registro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    es_admin = false
                };
                _connection.Insert(newUser);

                // Crear los registros del usuario.
                CrearRegistros(newUser);

                Usuarios insertedUser = _connection.Table<Usuarios>().FirstOrDefault(u => u.nombre_usuario == username);

                if (GameManager.Instance != null)
                {
                    CurrentPLayerDB.Instance.CurrentPlayer = insertedUser;
                }
                else
                {
                    Debug.LogError("GameManager.Instance es NULL en TryRegister()");
                }

                CloseRegister();

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    void CrearRegistros(Usuarios u)
    {
        // Progreso
        var _connection = DBManager.Instance.Connection;

        _connection.Insert(new Progreso
        {
            id_usuario = u.id_usuario,
            niveles_completados = 0,
            puntuacion = 0
        });
    }
    void ShowInfo(string info)
    {
        InfoPanelDialog.SetActive(true);
        InfoPanelDialog.transform.localScale = Vector3.zero;
        InfoPanelDialog.GetComponentInChildren<TMP_Text>().text = info;
        // InfoPanelDialog.

        // Animacion
        InfoPanelDialog.transform
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
    public void CloseInfo()
    {
        // Anuimacion
        InfoPanelDialog.transform
        .DOScale(Vector3.zero, 0.2f)
        .SetEase(Ease.InBack)
        .OnComplete(() => InfoPanelDialog.SetActive(false));
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