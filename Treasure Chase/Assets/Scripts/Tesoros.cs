using SQLite4Unity3d;

public class Tesoros
{
    [PrimaryKey, AutoIncrement] // ID -> clave primaria e autoincrementable
    public int Id { get; set; }
    
    public string Nombre { get; set; }
    
    public string Descripcion { get; set; }
    
    public string FechaRegistro { get; set; }
}
