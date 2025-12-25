namespace MyApi.Dtos;

public class TodoReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
    public DateTime CreatedUtc { get; set; }
}
