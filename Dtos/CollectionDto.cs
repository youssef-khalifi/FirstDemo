using SynchApp.Models;

namespace SynchApp.Dtos;

public class CollectionDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<RequestApi> Requests { get; set; } = new List<RequestApi>();
}