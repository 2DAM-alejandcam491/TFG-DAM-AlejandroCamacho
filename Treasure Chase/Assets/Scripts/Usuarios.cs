using SQLite4Unity3d;

[Table("Usuarios")]
public class Usuarios
{
    [PrimaryKey, AutoIncrement] // ID -> clave primaria e autoincrementable
    public int Id { get; set; }
    public string nombre_usuario  { get; set; }
    public string contrasenia  { get; set; }
}