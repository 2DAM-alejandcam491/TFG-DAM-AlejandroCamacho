using SQLite4Unity3d;

[Table("Usuarios")]
public class Usuarios
{
    [PrimaryKey, AutoIncrement] // ID -> clave primaria e autoincrementable
    public int id_usuario { get; set; }
    public string nombre_usuario { get; set; }
    public string contrasenia { get; set; }
    public string correo { get; set; }
    public string fecha_registro { get; set; }
    public bool es_admin { get; set; }
}