using System.ComponentModel.DataAnnotations;

namespace SynchApp.Models;

public class Collection
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<RequestApi> Requests { get; set; } = new List<RequestApi>();
    
    public DateTime? UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;

    // Flag to indicate if this product is synced with SQL Server
    [Required]
    public bool IsSynced { get; set; } = false;

    // Timestamp for tracking last sync
    public DateTime? LastSyncedAt { get; set; }
}