using CanchaGol.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// 2. Servir archivos estáticos de la carpeta wwwroot (index.html, css, js)
app.UseDefaultFiles();
app.UseStaticFiles();

// 3. Datos simulados
var empresa = new Empresa
{
    Nombre = "CanchaGol",
    Eslogan = "Reserva, juega y disfruta",
    Descripcion = "Complejo deportivo con canchas de grass sintético, iluminación LED y todos los servicios para que solo te preocupes por jugar.",
    Direccion = "Av. Los Deportistas 123, Lima",
    Telefono = "+51 999 888 777",
    HorarioAtencion = "Todos los días de 8:00 a 23:00",
    Destacados = new List<Destacado>
    {
        new() { Icono = "🌱", Titulo = "Grass sintético", Descripcion = "Superficie de última generación que cuida tus articulaciones." },
        new() { Icono = "💡", Titulo = "Iluminación LED", Descripcion = "Juega de noche con visibilidad total." },
        new() { Icono = "🚗", Titulo = "Estacionamiento", Descripcion = "Espacio seguro para más de 40 vehículos." },
        new() { Icono = "🚿", Titulo = "Vestuarios", Descripcion = "Duchas y casilleros para todo el equipo." }
    }
};

var canchas = new List<Cancha>
{
    new() { Id = 1, Nombre = "Cancha 1", Tipo = "Fútbol 5", Superficie = "Grass sintético", Jugadores = "5 vs 5", Icono = "⚽", PrecioBase = 60m,
            Descripcion = "Ideal para partidos rápidos entre amigos o compañeros de trabajo." },
    new() { Id = 2, Nombre = "Cancha 2", Tipo = "Fútbol 7", Superficie = "Grass sintético", Jugadores = "7 vs 7", Icono = "🥅", PrecioBase = 80m,
            Descripcion = "La favorita para campeonatos y partidos de fin de semana." },
    new() { Id = 3, Nombre = "Cancha 3", Tipo = "Fútbol 11", Superficie = "Grass sintético", Jugadores = "11 vs 11", Icono = "🏟️", PrecioBase = 150m,
            Descripcion = "Cancha de dimensiones reglamentarias para ligas y entrenamientos." }
};

var servicios = new List<Servicio>
{
    new() { Id = 1, Nombre = "Alquiler de cancha", Categoria = "Principal", Icono = "⚽", Precio = 60m, Unidad = "por hora (desde)", Descripcion = "Reserva la cancha que mejor se adapte a tu equipo." },
    new() { Id = 2, Nombre = "Campeonatos", Categoria = "Principal", Icono = "🏆", Precio = 350m, Unidad = "por equipo inscrito", Descripcion = "Torneos relámpago y ligas con árbitros y premiación." },
    new() { Id = 3, Nombre = "Entrenamientos", Categoria = "Principal", Icono = "🏃", Precio = 90m, Unidad = "por sesión", Descripcion = "Sesiones con entrenadores certificados para todas las edades." },
    new() { Id = 4, Nombre = "Eventos", Categoria = "Principal", Icono = "🎉", Precio = 500m, Unidad = "por evento", Descripcion = "Cumpleaños, integraciones y actividades corporativas." },
    new() { Id = 5, Nombre = "Vestuarios", Categoria = "Adicional", Icono = "🚿", Precio = 0m, Unidad = "Incluido con el alquiler", Descripcion = "Duchas, casilleros y baños para jugadores." },
    new() { Id = 6, Nombre = "Iluminación LED", Categoria = "Adicional", Icono = "💡", Precio = 20m, Unidad = "por hora desde las 18:00", Descripcion = "Reflectores LED para jugar de noche." },
    new() { Id = 7, Nombre = "Estacionamiento", Categoria = "Adicional", Icono = "🚗", Precio = 0m, Unidad = "Incluido con el alquiler", Descripcion = "Estacionamiento vigilado dentro del complejo." },
    new() { Id = 8, Nombre = "Arbitraje", Categoria = "Adicional", Icono = "🟨", Precio = 40m, Unidad = "por partido", Descripcion = "Árbitros profesionales para tus partidos y torneos." }
};

// 4. Endpoints de la API (solo lectura)
app.MapGet("/api/empresa", () => Results.Ok(empresa));

app.MapGet("/api/canchas", () => Results.Ok(canchas));

app.MapGet("/api/servicios", () => Results.Ok(servicios));

app.MapGet("/api/promociones", () => Results.Ok(GenerarPromociones()));

// Horarios: se puede filtrar por fecha con ?fecha=2026-10-02
app.MapGet("/api/horarios", (string? fecha) =>
{
    var lista = GenerarHorarios();
    if (!string.IsNullOrWhiteSpace(fecha))
        lista = lista.Where(h => h.Fecha == fecha).ToList();
    return Results.Ok(lista);
});

app.MapGet("/api/horarios/{id:int}", (int id) =>
{
    var horario = GenerarHorarios().FirstOrDefault(h => h.Id == id);
    return horario is null
        ? Results.NotFound(new { mensaje = $"No existe el horario {id}." })
        : Results.Ok(horario);
});

// 5. Puerto asignado dinámicamente por Render o puerto por defecto local
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");

// ---------- Generadores de datos (usan la fecha actual de Perú, UTC-5) ----------
List<Horario> GenerarHorarios()
{
    var ahora = DateTime.UtcNow.AddHours(-5);
    var lista = new List<Horario>();
    var id = 1;

    for (var dia = 0; dia < 7; dia++)
    {
        var fecha = ahora.Date.AddDays(dia);
        var finDeSemana = fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        foreach (var cancha in canchas)
        {
            for (var hora = 8; hora < 23; hora++)
            {
                var precio = cancha.PrecioBase + (hora >= 18 ? 20m : 0m) + (finDeSemana ? 10m : 0m);

                string estado;
                if (dia == 0 && hora <= ahora.Hour)
                {
                    estado = "Finalizado";
                }
                else
                {
                    var n = (id * 37) % 10;
                    estado = n < 6 ? "Disponible" : n < 9 ? "Reservado" : "Mantenimiento";
                }

                lista.Add(new Horario
                {
                    Id = id,
                    CanchaId = cancha.Id,
                    Cancha = cancha.Nombre,
                    TipoCancha = cancha.Tipo,
                    Superficie = cancha.Superficie,
                    Jugadores = cancha.Jugadores,
                    Fecha = fecha.ToString("yyyy-MM-dd"),
                    HoraInicio = $"{hora:00}:00",
                    HoraFin = $"{hora + 1:00}:00",
                    DuracionMinutos = 60,
                    Precio = precio,
                    Estado = estado,
                    Disponible = estado == "Disponible",
                    Iluminacion = hora >= 18
                });
                id++;
            }
        }
    }

    return lista;
}

List<Promocion> GenerarPromociones()
{
    var hoy = DateTime.UtcNow.AddHours(-5).Date;
    return new List<Promocion>
    {
        new() { Id = 1, Titulo = "Hora feliz", DescuentoPorcentaje = 20, Codigo = "HORAFELIZ",
                Descripcion = "20% de descuento en reservas de lunes a viernes antes de las 14:00.",
                ValidoHasta = hoy.AddDays(30).ToString("yyyy-MM-dd") },
        new() { Id = 2, Titulo = "Pack campeonato", DescuentoPorcentaje = 15, Codigo = "CAMPEON15",
                Descripcion = "Inscribe 4 equipos o más y obtén 15% de descuento en cada inscripción.",
                ValidoHasta = hoy.AddDays(45).ToString("yyyy-MM-dd") },
        new() { Id = 3, Titulo = "Cumpleaños futbolero", DescuentoPorcentaje = 10, Codigo = "FESTEJA10",
                Descripcion = "10% de descuento al reservar tu evento con 7 días de anticipación.",
                ValidoHasta = hoy.AddDays(60).ToString("yyyy-MM-dd") }
    };
}