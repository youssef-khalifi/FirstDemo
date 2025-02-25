
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SynchApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public double Price { get; set; }
        

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        // Flag to indicate if this product is synced with SQL Server
        [Required]
        public bool IsSynced { get; set; } = false;

        // Timestamp for tracking last sync
        public DateTime? LastSyncedAt { get; set; }
        [Required]
        public Guid SyncId { get; set; } = Guid.NewGuid();
    }
    
}