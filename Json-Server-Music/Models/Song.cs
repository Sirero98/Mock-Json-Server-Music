using System.ComponentModel.DataAnnotations;

namespace Json_Server_Music.Models;

public class Song
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int DurationInSeconds { get; set; }
    [Range(1900, 2026)]
    public int Year { get; set; }

    public IResult? Validate()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(this, context, results, true))
        {
            return Results.BadRequest(results.Select(r => r.ErrorMessage));
        }
        return null;
    }
}
