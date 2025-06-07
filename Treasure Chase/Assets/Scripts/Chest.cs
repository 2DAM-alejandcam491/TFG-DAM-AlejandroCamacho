using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool abierto = false;

    public int id_cofre_tesoro;

    private SQLiteConnection db;

    public CanvasGroup fadePanel;  // Asigna desde el Inspector

    public GameObject imagenTesoro;
    private float subidaY = 1.5f; // Distancia hacia arriba
    private float duracionSubida = 1f; // Tiempo de animación

    void Awake()
    {
        var dbPath = Path.Combine(Application.streamingAssetsPath, "Prueba.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        fadePanel.alpha = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!abierto && other.CompareTag("Player"))
        {
            abierto = true;
            animator.SetTrigger("Abrir");
            StartCoroutine(DesaparecerDespues());
        }
    }

    private IEnumerator DesaparecerDespues()
    {
        yield return new WaitForSeconds(1f);

        if (imagenTesoro != null)
        {
            imagenTesoro.SetActive(true);

            var sr = imagenTesoro.GetComponent<SpriteRenderer>();

            imagenTesoro.transform.DOMoveY(
                imagenTesoro.transform.position.y + subidaY,
                duracionSubida
            ).SetEase(Ease.OutQuad);

            // Esperar a que se "asome"
            yield return new WaitForSeconds(0.6f);
            sr.maskInteraction = SpriteMaskInteraction.None;
        }
        // Espera la duración de la animación (ajusta a tu duración real)
        yield return new WaitForSeconds(duracionSubida);
        AddTeasureToDB();
        Destroy(gameObject); // o gameObject.SetActive(false);
    }

    private void AddTeasureToDB()
    {
        int id_player = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;

        var lista = db.Table<TesorosObtenidos>()
              .Where(t => t.id_usuario == id_player && t.id_tesoro == id_cofre_tesoro)
              .ToList();

        if (lista.Count != 0)
        {
            Debug.Log("El tesoro ya fue obtenido por este usuario.");
        }
        else
        {
            // Insertar nuevo tesoro
            var nuevoTesoro = new TesorosObtenidos
            {
                id_usuario = id_player,
                id_tesoro = id_cofre_tesoro,
                fecha_obtenido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            db.Insert(nuevoTesoro);
            Debug.Log("Tesoro insertado correctamente.");
        }

        // Actualizar progreso del Player
        UpdateProgress(id_player);

        // Cambio de escena: TestLevelScene
        FadeAndLoadScene("MenuSelectLevel");

    }

    void UpdateProgress(int id_player)
    {
        var progreso = db.Table<Progreso>()
            .Where(p => p.id_usuario == id_player)
            .FirstOrDefault();

        if (progreso != null)
        {
            // Solo actualizar si el nuevo nivel es mayor al que tenía
            if (id_cofre_tesoro >= progreso.niveles_completados)
            {
                progreso.niveles_completados = id_cofre_tesoro;
                db.Update(progreso);
                Debug.Log($"Progreso actualizado: {progreso.niveles_completados}");
            }
            else
            {
                Debug.Log("Este cofre no desbloquea un nuevo nivel.");
            }
        }
        else
        {
            Debug.Log("No exsiste el progreso en la DB.");
        }
    }

    private void FadeAndLoadScene(string sceneName)
    {
        Debug.Log("Iniciando fade...");

        fadePanel.DOFade(1f, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("Fade terminado, cargando escena...");
                SceneManager.LoadScene(sceneName);
            });
    }



}
