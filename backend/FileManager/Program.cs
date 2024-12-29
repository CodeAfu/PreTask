using FileManager.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileManager API", Version = "v1" });

    // Enable support for file upload in Swagger
    c.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSingleton<MongoDbService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

