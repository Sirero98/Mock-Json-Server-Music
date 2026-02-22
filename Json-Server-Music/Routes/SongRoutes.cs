using Json_Server_Music.Models;
using Json_Server_Music.Services;
using Microsoft.AspNetCore.Mvc;

namespace Json_Server_Music.Routes;

public static class SongRoutes
{
    public static void MapSongRoutes(this WebApplication app, List<Song> songs, JsonStore store)
    {
        app.MapGet("/songs", ([FromQuery] string? genre, [FromQuery] string? artist, [FromQuery] int? year, [FromQuery] string? sortBy, [FromQuery] string? order, [FromQuery] int? page, [FromQuery] int? limit) =>
        {
            var currentPage = Math.Max(page ?? 1, 1);
            var currentLimit = Math.Clamp(limit ?? 10, 1, 100);

            var filtered = songs
                .Where(x => genre == null || x.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .Where(x => artist == null || x.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
                .Where(x => year == null || x.Year == year)
                .ToList();

            var descending = order?.ToLower() == "desc";

            filtered = sortBy?.ToLower() switch
            {
                "title" => descending ? filtered.OrderByDescending(x => x.Title).ToList() : filtered.OrderBy(x => x.Title).ToList(),
                "artist" => descending ? filtered.OrderByDescending(x => x.Artist).ToList() : filtered.OrderBy(x => x.Artist).ToList(),
                "album" => descending ? filtered.OrderByDescending(x => x.Album).ToList() : filtered.OrderBy(x => x.Album).ToList(),
                "genre" => descending ? filtered.OrderByDescending(x => x.Genre).ToList() : filtered.OrderBy(x => x.Genre).ToList(),
                "year" => descending ? filtered.OrderByDescending(x => x.Year).ToList() : filtered.OrderBy(x => x.Year).ToList(),
                "duration" => descending ? filtered.OrderByDescending(x => x.DurationInSeconds).ToList() : filtered.OrderBy(x => x.DurationInSeconds).ToList(),
                _ => filtered
            };

            var total = filtered.Count;
            var totalPages = (int)Math.Ceiling((double)total / currentLimit);

            var result = filtered
                .Skip((currentPage - 1) * currentLimit)
                .Take(currentLimit)
                .ToList();

            return Results.Ok(new
            {
                Data = result,
                Page = currentPage,
                Limit = currentLimit,
                Total = total,
                TotalPages = totalPages
            });
        });

        app.MapGet("/songs/{id:int}", (int id) =>
        {
            var song = songs.Find(x => x.Id == id);
            if (song == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(song);
        });

        app.MapPost("/songs", (Song song) =>
        {
            var error = song.Validate();
            if (error != null) return error;

            song.Id = songs.Any() ? songs.Max(s => s.Id) + 1 : 1;
            songs.Add(song);
            store.Save(songs);

            return Results.Created($"/songs/{song.Id}", song);
        });

        app.MapPut("/songs/{id:int}", (int id, Song song) =>
        {
            var error = song.Validate();
            if (error != null) return error;

            var existingSong = songs.Find(s => s.Id == id);
            if (existingSong == null)
            {
                return Results.NotFound();
            }

            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;
            existingSong.Album = song.Album;
            existingSong.Genre = song.Genre;
            existingSong.DurationInSeconds = song.DurationInSeconds;
            existingSong.Year = song.Year;

            store.Save(songs);
            return Results.Ok(existingSong);
        });

        app.MapPatch("/songs/{id:int}", (int id, Song song) =>
        {
            var existingSong = songs.Find(s => s.Id == id);
            if (existingSong == null)
            {
                return Results.NotFound();
            }

            if (!string.IsNullOrEmpty(song.Title))
                existingSong.Title = song.Title;
            if (!string.IsNullOrEmpty(song.Artist))
                existingSong.Artist = song.Artist;
            if (!string.IsNullOrEmpty(song.Album))
                existingSong.Album = song.Album;
            if (!string.IsNullOrEmpty(song.Genre))
                existingSong.Genre = song.Genre;
            if (song.DurationInSeconds > 0)
                existingSong.DurationInSeconds = song.DurationInSeconds;
            if (song.Year > 0)
                existingSong.Year = song.Year;

            var error = existingSong.Validate();
            if (error != null) return error;

            store.Save(songs);
            return Results.Ok(existingSong);
        });

        app.MapDelete("/songs/{id:int}", (int id) =>
        {
            var song = songs.Find(x => x.Id == id);
            if (song == null)
            {
                return Results.NotFound();
            }
            songs.Remove(song);
            store.Save(songs);
            return Results.Ok();
        });
    }
}
