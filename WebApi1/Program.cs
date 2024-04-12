using Minio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MinioClient service
builder.Services.AddSingleton<MinioClient>((sp) =>
{
    MinioClient minioConfig = (MinioClient)new MinioClient()
                .WithEndpoint("192.168.10.10:9000")
                .WithCredentials("mW1WNdWDwJylFr6c6GvO", "yfDjjsBNRS8hKONYqR4zsN9Wn0ar7JP9MMsnn0uF")
                .Build();
    return minioConfig;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
