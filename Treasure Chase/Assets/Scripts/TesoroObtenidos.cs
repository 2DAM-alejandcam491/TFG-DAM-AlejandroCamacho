using SQLite4Unity3d;

public class TesorosObtenidos
{
    public int IdUsuario { get; set; }  // Foreing Key
    
    public int IdTesoro { get; set; }  // Foreing Key
    
    public string FechaObtenido { get; set; }

    [PrimaryKey]
    public string Id { get { return IdUsuario + "_" + IdTesoro; } } // Combinamos los dos ids como clave primaria
}
