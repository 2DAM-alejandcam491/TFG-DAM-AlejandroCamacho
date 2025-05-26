using SQLite4Unity3d;

[Table("Progreso")]
public class Progreso
{
    [PrimaryKey, AutoIncrement]
    public int id_progreso { get; set; }
    public int id_usuario { get; set; }
    public int niveles_completados { get; set; }
    public int puntuacion { get; set; }
}
