namespace SoftlutionicAPI.Models
{
    public class Contacto
    {
        
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public string? Telefono { get; set; }
            public string? Mensaje { get; set; }
            public DateTime FechaRegistro { get; set; } = DateTime.Now;
        }
}


