namespace SoftlutionicAPI.Configuration
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } =
            "softlutionic_nosql";

        public string SolicitudesCollection { get; set; } =
            "solicitudes_contacto";

        public string NotificacionesCollection { get; set; } =
            "notificaciones";
    }
}