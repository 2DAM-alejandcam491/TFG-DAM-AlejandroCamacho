using SQLite4Unity3d;

[Table("TesorosObtenidos")]
public class TesorosObtenidos
{
    [PrimaryKey, AutoIncrement]
    public int id_tesoro_obtenido { get; set; }
    [NotNull]
    public int id_usuario { get; set; }
    [NotNull]
    public int id_tesoro { get; set; }
    public string fecha_obtenido { get; set; }
}
