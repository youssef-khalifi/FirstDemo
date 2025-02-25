namespace SynchApp.Dtos;

public class CreateRequestDto
{
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string HeadersJson { get; set; } = string.Empty; // Store as JSON
    public string Body { get; set; } = string.Empty;
}