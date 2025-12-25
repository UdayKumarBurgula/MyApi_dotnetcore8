using System.ComponentModel.DataAnnotations;

namespace MyApi.Dtos;

public class TodoUpdateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = "";

    public bool IsDone { get; set; }
}
