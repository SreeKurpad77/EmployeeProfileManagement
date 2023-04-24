using Azure.Identity;
using EmployeeProfileManagement.API;
using EmployeeProfileManagement.Core.Model;
using EmployeeProfileManagement.Core.Repositories;
using EmployeeProfileManagement.Core.Repositories.Interfaces;
using EmployeeProfileManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//build configuration
IConfiguration config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
//configure services
builder.Services.AddScoped<IEmployeeProfileRepository, EmployeeProfileRepository>();
builder.Services.AddScoped<IRepository<EmployeeProfile>, AzureSqldbRepository<EmployeeProfile>>();

var isKeyVaultSet = Convert.ToBoolean(config.GetSection("useKeyVault").Value);

if (isKeyVaultSet)
{
    //get sql connection string from keyvault
    var keyvaultEndpoint = new Uri(String.Concat("https://", config.GetSection("AzureKeyVaultName").Value, ".vault.azure.net"));
    builder.Configuration.AddAzureKeyVault(keyvaultEndpoint, new DefaultAzureCredential());

    builder.Services.AddDbContext<AzureSqldbContext>(options =>
    {
        var connectionString = builder.Configuration["empprofile-dev-connstring"];
        options.UseSqlServer(connectionString);
    }
    );
}
else
    builder.Services.AddDbContext<AzureSqldbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        options.UseSqlServer(connectionString);
    }
    );

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
    options.SchemaNameGenerator = new CustomSchemaNameGenerator();
    options.TypeNameGenerator = new CustomTypeNameGenerator();
});

var app = builder.Build();


app.MapSwagger();
app.UseOpenApi();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/profiles", async (IEmployeeProfileRepository repo) => await repo.GetAll());

app.MapGet("/profiles/{id}", async (int id, IEmployeeProfileRepository repo) =>
{
var result = await repo.GetById(id);
if (result.IsSuccess && result.Result != null)
{
    //var content = new ByteArrayContent(result.Result.PhotoUrl);
    //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
    //var empProfile = new EmployeeProfile{ Id = result.Result.Id, Name = result.Result.Name, DateofBirth = result.Result.DateofBirth, HireDate = result.Result.HireDate, PhotoUrl = result.Result.PhotoUrl };
    return Results.Ok(result);
    }
    else if (result.IsSuccess && result.Result == null)
        return Results.NotFound();
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
}
);
app.MapPost("/profiles", async (EmployeeProfile profile, IEmployeeProfileRepository repo) =>
    {
        var result = await repo.Add(profile);
        if (result.IsSuccess && result.Result != null)
        {
            //var content = new ByteArrayContent(result.Result.PhotoUrl);
            //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
//            return Results.Created($"/profiles/{result.Result.Id}", new { result.Result.Id, result.Result.Name, result.Result.DateofBirth, result.Result.Designation, result.Result.HireDate, Image = content });
            return Results.Created($"/profiles/{result.Result.Id}", result);

        }
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
);

app.MapPut("/profiles", async (EmployeeProfile profile, IEmployeeProfileRepository repo) =>
{
    var result = await repo.Update(profile);
    if (result.IsSuccess && result.Result != null)
    {
        //var content = new ByteArrayContent(result.Result.PhotoUrl);
        //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        //return Results.Ok(new { result.Result.Id, result.Result.Name, result.Result.DateofBirth, result.Result.Designation, result.Result.HireDate, Image = content });
        return Results.Ok(result);
    }
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
}
);
app.MapDelete("/profiles/{id}", async (int id, IEmployeeProfileRepository repo) =>
{
    var result = await repo.Delete(id);
    if (result.IsSuccess)
        return Results.Ok();
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
}
);
app.Run();
