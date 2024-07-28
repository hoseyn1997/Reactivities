namespace Domain;

public class PrivateComment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public AppUser Author { get; set; }
    public AppUser Target { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
