using System.Text.Json;
using Json_Server_Music.Models;

namespace Json_Server_Music.Services;

public class JsonStore
{
    private const string FilePath = "Data/songs.json";

    public List<Song> Load()
    {
        if (!File.Exists(FilePath)) return new List<Song>();
        var json = File.ReadAllText(FilePath);
        if (string.IsNullOrWhiteSpace(json)) return new List<Song>();
        return JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
    }

    public void Save(List<Song> songs)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(songs, options);
        File.WriteAllText(FilePath, json);
    }
}