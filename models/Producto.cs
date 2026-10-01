namespace CanchaGol.Models;

public class Empresa
{
    public string Nombre { get; set; } = string.Empty;
    public string Eslogan { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string HorarioAtencion { get; set; } = string.Empty;
    public List<Destacado> Destacados { get; set; } = new();
}

public class Destacado
{
    public string Icono { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

public class Cancha
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Superficie { get; set; } = string.Empty;
    public string Jugadores { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
}

public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
}

public class Promocion
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int DescuentoPorcentaje { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string ValidoHasta { get; set; } = string.Empty;
}

public class Horario
{
    public int Id { get; set; }
    public int CanchaId { get; set; }
    public string Cancha { get; set; } = string.Empty;
    public string TipoCancha { get; set; } = string.Empty;
    public string Superficie { get; set; } = string.Empty;
    public string Jugadores { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public int DuracionMinutos { get; set; }
    public decimal Precio { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Disponible { get; set; }
    public bool Iluminacion { get; set; }
}