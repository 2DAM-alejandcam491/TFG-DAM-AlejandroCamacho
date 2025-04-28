using SQLite4Unity3d;

public class Progreso
{
    [PrimaryKey, AutoIncrement] // ID -> clave primaria e autoincrementable
    public int Id { get; set; }
    
    public int IdUsuario { get; set; }  // Foreing Key
    
    public int NivelesCompletados { get; set; }
    
    public int Puntuacion { get; set; }
}
