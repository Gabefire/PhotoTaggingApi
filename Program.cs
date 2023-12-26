using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.Configure<PhotoTaggingDatabaseSettings>(builder.Configuration.GetSection("PhotoTaggingDatabase"));
builder.Services.AddSingleton<TagsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<HighScoreService>();
builder.Services.AddControllers();

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();



app.Run();
