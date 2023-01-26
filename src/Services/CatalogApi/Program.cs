using CatalogApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CatalogApi.Data;
using CatalogApi.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = LogExtensions.ConfigureLoger()
        .CreateBootstrapLogger();

builder.Host.UseLogging();
builder.LogStartUp();

builder.Services.AddDbContext<ProductApiContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false;

            options.SaveToken = true;
            var baseUri = builder.Configuration.GetValue<string>("BaseUri");
            var issuerUri = builder.Configuration.GetValue<string>("IssuerUri");
            var token = builder.Configuration.GetValue<string>("Token");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuerUri,
                ValidAudience = baseUri,
                ValidateAudience = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token)),  // optionsForToken.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
        });

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });

});

var app = builder.Build();

try
{

    using (IServiceScope scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<ProductApiContext>().Database.Migrate();
    }
    
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        
        string pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_PATHBASE");
        if (!string.IsNullOrEmpty(pathBase) && pathBase != "/")
        {
            app.UsePathBase(new PathString(pathBase));
            Log.Logger.Debug($"Catalog API subfolder: {pathBase}");
            
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                {
                    if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;
                    var basePath = pathBase;
                    var serverUrl = $"{httpRequest.Scheme}://{httpRequest.Headers["X-Forwarded-Host"]}{basePath}";
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                });
                
            });
            app.UseSwaggerUI();
        }
        
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            Log.Logger.Debug($"Catalog API subfolder: Empty");
        }
    }

    app.MapControllers();
    app.MapGet("/", () => "Catalog API Hello World!");
    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}


