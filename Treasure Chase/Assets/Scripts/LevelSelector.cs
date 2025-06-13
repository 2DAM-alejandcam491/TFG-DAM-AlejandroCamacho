using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Transform[] levelPoints; // Puntos de los niveles
    public string[] sceneNames; // Nombre de los niveles
    public Transform character; // El PNG del personaje que se moverá

    private int currentIndex = 0;
    private Tween moveTween;

    // Array de los caminos que unen los levelPoints
    public GameObject[] paths;

    // Colores de levelPath
    private Color colorSuperado = Color.green;
    private Color colorNoSuperado = Color.red;

    private int nivelesCompletados = 0;

    void Awake()
    {
        int IdUsuario = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;
        Debug.Log($"ID USUARIO: {IdUsuario}");
        // Obtener el nivel superado del jugador actual desde la DB
        var progreso = DBManager.Instance.Connection.Table<Progreso>()
            .Where(p => p.id_usuario == IdUsuario)
            .FirstOrDefault();
         nivelesCompletados = progreso.niveles_completados;
        Debug.Log($"NIVEL SUPERADO: {nivelesCompletados}");

        // Posicionar al personaje en el nivel que le toca jugar (el siguiente)
        currentIndex = Mathf.Clamp(progreso.niveles_completados, 0, levelPoints.Length - 1);

        MoveToCurrentPoint(true);

        // Mostrar caminos activados
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].SetActive(i < nivelesCompletados);
        }

        // Colorear puntos
        for (int i = 0; i < levelPoints.Length; i++)
        {
            var renderer = levelPoints[i].GetComponent<UnityEngine.UI.Image>();
            if (renderer != null)
                renderer.color = i < nivelesCompletados ? colorSuperado : colorNoSuperado;
        }
    }

    void Start()
    {
        // Coloca el personaje en el primer punto al iniciar
        MoveToCurrentPoint(false);
    }

    void Update()
    {
        if (moveTween != null && moveTween.IsActive() && moveTween.IsPlaying()) // No se juega hjasta que no este quieto el personaje
            return;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentIndex < nivelesCompletados && currentIndex < levelPoints.Length - 1)
            {
                currentIndex++;
                MoveToCurrentPoint(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                MoveToCurrentPoint(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EnterLevel();
        }
    }

    private void MoveToCurrentPoint(bool instant)
    {
        Vector3 targetPos = levelPoints[currentIndex].position + new Vector3(15f, 105f, 0); // 0.5f = altura deseada

        moveTween = character.DOMove(targetPos, instant ? 0f : 0.4f)
                                 .SetEase(Ease.OutQuad)
                                 .OnComplete(() => Debug.Log($"Llegó al nivel {currentIndex}"));
    }

    private void EnterLevel()
    {
        if (sceneNames.Length > currentIndex && !string.IsNullOrEmpty(sceneNames[currentIndex]))
        {
            Debug.Log($"Cargando nivel: {sceneNames[currentIndex]}");
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
        else
        {
            Debug.LogWarning("No hay una escena asociada a este punto.");
        }
    }
}