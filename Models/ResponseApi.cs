﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SynchApp.Models;

public class ResponseApi
{
    public int Id { get; set; }
    public int StatusCode { get; set; }
    public string HeadersJson { get; set; } = string.Empty; // Store as JSON
    public string Body { get; set; } = string.Empty;
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
    
    [NotMapped] // Not mapped in DB
    public Dictionary<string, string> Headers
    {
        get => HeadersJson == null ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(HeadersJson);
        set => HeadersJson = JsonSerializer.Serialize(value);
    }
}