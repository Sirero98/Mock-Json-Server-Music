# Mock Json Server Music

API REST para gestionar una coleccion de canciones. Almacena los datos en un archivo JSON, sin necesidad de base de datos.

## Tecnologias

- ASP.NET Core 8.0
- Swagger / OpenAPI

## Endpoints

| Metodo | Ruta | Descripcion |
|--------|------|-------------|
| GET | `/songs` | Listar canciones (con filtros, ordenacion y paginacion) |
| GET | `/songs/{id}` | Obtener una cancion por ID |
| POST | `/songs` | Crear una cancion |
| PUT | `/songs/{id}` | Actualizar una cancion (completa) |
| PATCH | `/songs/{id}` | Actualizar una cancion (parcial) |
| DELETE | `/songs/{id}` | Eliminar una cancion |

### Parametros de consulta (GET /songs)

| Parametro | Descripcion |
|-----------|-------------|
| `genre` | Filtrar por genero |
| `artist` | Filtrar por artista |
| `year` | Filtrar por año |
| `sortBy` | Ordenar por campo (title, artist, album, genre, year, duration) |
| `order` | Orden ascendente o descendente (asc/desc) |
| `page` | Numero de pagina (por defecto 1) |
| `limit` | Resultados por pagina (por defecto 10, max 100) |

## Como ejecutar

1. Tener instalado [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

2. Ejecutar:
```bash
cd Json-Server-Music
dotnet run
```

3. Acceder a la API:
   - http://localhost:5016
   - Swagger UI: http://localhost:5016/swagger/ui.html
