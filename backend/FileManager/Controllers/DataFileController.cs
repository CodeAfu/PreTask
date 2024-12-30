using FileManager.Data;
using FileManager.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FileManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataFileController : ControllerBase
{
    private readonly IMongoCollection<DataFile> _dataFiles;

    public DataFileController(MongoDbService mongoDbService)
    {
        _dataFiles = mongoDbService.getDataFilesCollection();
    }

    [HttpGet]
    public async Task<IEnumerable<DataFile>> Get()
    {
        return await _dataFiles.Find(FilterDefinition<DataFile>.Empty).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DataFile?>> GetById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID is required");
        }

        var filter = Builders<DataFile>.Filter.Eq(x => x.Id, id);
        var dataFile = await _dataFiles.Find(filter).FirstOrDefaultAsync();
        return dataFile is not null ? Ok(dataFile) : NotFound();
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<DataFile>>> SearchFiles([FromQuery] string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            return BadRequest("Filename query is required");
        }

        var filter = Builders<DataFile>.Filter.Regex(x => x.FileName, new BsonRegularExpression(filename, "i"));
        var dataFiles = await _dataFiles.Find(filter).ToListAsync();
        
        return Ok(dataFiles);
    }

    
    [HttpPost]
    public async Task<ActionResult<DataFile>> Post(DataFile dataFile)
    {
        await _dataFiles.InsertOneAsync(dataFile);
        return CreatedAtAction(nameof(Get), new { id = dataFile.Id }, dataFile);
    }

    [HttpPost("upload")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<DataFile>> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileHash = CalculateFileHash(memoryStream.ToArray());

        var dataFile = new DataFile
        {
            Data = memoryStream.ToArray(),
            FileName = file.FileName,
            Extension = Path.GetExtension(file.FileName),
            FileSize = file.Length,
            UploadDate = DateTime.UtcNow,
            FileHash = fileHash,
        };

        await _dataFiles.InsertOneAsync(dataFile);
        return CreatedAtAction(nameof(GetById), new { id = dataFile.Id }, dataFile);
    }

    [HttpPost("upload-multiple")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
    {
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var fileHash = CalculateFileHash(memoryStream.ToArray());

                var dataFile = new DataFile
                {
                    Data = memoryStream.ToArray(),
                    FileName = file.FileName,
                    Extension = Path.GetExtension(file.FileName),
                    FileSize = file.Length,
                    UploadDate = DateTime.UtcNow,
                    FileHash = fileHash,
                };

                await _dataFiles.InsertOneAsync(dataFile);
            }
        }

        return Ok("Files uploaded successfully.");
    }

    [HttpGet("download/{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DownloadFile(string id)
    {
        var filter = Builders<DataFile>.Filter.Eq(x => x.Id, id);
        var dataFile = await _dataFiles.Find(filter).FirstOrDefaultAsync();

        if (dataFile == null)
        {
            return NotFound("File not found");
        }

        var fileBytes = (byte[])dataFile.Data;

        return File(fileBytes, "application/octet-stream", dataFile.FileName);
    }

    [HttpPut]
    public async Task<IActionResult> Put(DataFile dataFile)
    {
        var filter = Builders<DataFile>.Filter.Eq(x => x.Id, dataFile.Id);
        await _dataFiles.ReplaceOneAsync(filter, dataFile);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateFileNameRequest request)
    {
        try
        {
            var filter = Builders<DataFile>.Filter.Eq(x => x.Id, id);
            var update = Builders<DataFile>.Update.Set(x => x.FileName, request.FileName);

            var result = await _dataFiles.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                return NotFound($"File with ID {id} not found.");
            }

            return Ok(new { message = "Filename updated successfully" });
        
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var filter = Builders<DataFile>.Filter.Eq(x => x.Id, id);
        await _dataFiles.DeleteOneAsync(filter);
        return Ok();
    }

    private string CalculateFileHash(byte[] fileData)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(fileData);
        return Convert.ToBase64String(hashBytes);
    }
}