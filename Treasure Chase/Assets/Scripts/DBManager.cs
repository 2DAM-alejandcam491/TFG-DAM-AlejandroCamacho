using System.IO;
using SQLite4Unity3d;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public SQLiteConnection Connection;

    public static DBManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            var dbPath = Path.Combine(Application.streamingAssetsPath, "DBTreasureChase.db");
            Connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SQLiteConnection GetSQLiteConnection()
    {
        var dbPath = Path.Combine(Application.streamingAssetsPath, "DBTreasureChase.db");
        return (Connection == null) ? new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create) : Connection; 
    }

    public int ObtenerPuntuacion()
    {
        int IdUsuario = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;
        var progreso = Connection.Table<Progreso>().Where(p => p.id_usuario == IdUsuario).FirstOrDefault();
        return progreso != null ? progreso.puntuacion : -1;
    }

    public void IncrementScoreOnDB(int valor)
    {
        if (GameManager.Instance == null || CurrentPLayerDB.Instance.CurrentPlayer == null)
        {
            Debug.LogError("GameManager o CurrentPlayer es NULL");
            return;
        }

        int IdUsuario = CurrentPLayerDB.Instance.CurrentPlayer.id_usuario;
        var progreso = Connection.Table<Progreso>().Where(p => p.id_usuario == IdUsuario).FirstOrDefault();
        

        if (progreso != null)
        {
            progreso.puntuacion += valor;
            Connection.Update(progreso);
            Debug.Log("Puntuación actualizada: " + progreso.puntuacion);
        }
        else
        {
            Debug.LogWarning("No se encontró progreso para el usuario con ID: " + IdUsuario);
        }
    }
}
