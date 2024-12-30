using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FileManager.Models;

public class DataFile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    [Required(ErrorMessage = "A File is required.")]
    [BsonElement("data")]
    public required object Data { get; set; }

    [BsonElement("filename")]
    public string? FileName { get; set; }

    [BsonElement("extension")]
    public string? Extension { get; set; }
    
    [BsonElement("filesize")]
    public long FileSize { get; set; }

    [BsonElement("uploadDate")]
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    [BsonElement("fileHash")]
    public string? FileHash { get; set; }

}
