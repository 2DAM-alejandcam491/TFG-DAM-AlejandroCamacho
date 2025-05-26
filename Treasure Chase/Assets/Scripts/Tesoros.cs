using SQLite4Unity3d;

public class Tesoros
{
    [PrimaryKey, AutoIncrement] 
    public int id_tesoro { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
}
