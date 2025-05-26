using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBViewer : MonoBehaviour
{
    public TMP_Text displayText;

    private SQLiteConnection _connection;

    // Buttons
    public Button usuariosButton, progresoButton, tesorosButton, tesorosObtenidosButton;
    public Button cerrarSesionbutton;

    // Start is called before the first frame update
    void Start()
    {
        _connection = DBManager.Instance.Connection;

        usuariosButton.onClick.AddListener(VerUsuarios);
        progresoButton.onClick.AddListener(VerProgreso);
        tesorosButton.onClick.AddListener(VerTesoros);
        tesorosObtenidosButton.onClick.AddListener(VerTesorosObtenidos);

        cerrarSesionbutton.onClick.AddListener(LogOut);

        VerUsuarios();
    }

    public void VerUsuarios()
    {
        var lista = _connection.Table<Usuarios>().ToList();
        string resultado = "=== Usuarios ===\n";
        foreach (var u in lista)
        {
            resultado += $"{u.id_usuario}: {u.nombre_usuario} ({u.correo})\n";
        }
        displayText.text = resultado;
    }

    public void VerProgreso()
    {
        var lista = _connection.Table<Progreso>().ToList();
        string resultado = "=== Progreso ===\n";
        foreach (var p in lista)
        {
            resultado += $"Usuario {p.id_usuario}: {p.niveles_completados} niveles, {p.puntuacion} puntos\n";
        }
        displayText.text = resultado;
    }

    public void VerTesoros()
    {
        var lista = _connection.Table<Tesoros>().ToList();
        string resultado = "=== Tesoros ===\n";
        foreach (var t in lista)
        {
            resultado += $"{t.id_tesoro}: {t.nombre} - {t.descripcion}\n";
        }
        displayText.text = resultado;
    }

    public void VerTesorosObtenidos()
    {
        var lista = _connection.Table<TesorosObtenidos>().ToList();
        string resultado = "=== Tesoros Obtenidos ===\n";
        foreach (var to in lista)
        {
            resultado += $"Usuario {to.id_usuario} obtuvo tesoro {to.id_tesoro} en {to.fecha_obtenido}\n";
        }
        displayText.text = resultado;
    }

    public void LogOut()
    {
        CurrentPLayerDB.Instance.CurrentPlayer = null;
        SceneManager.LoadScene("LoginScene");
    }
}
