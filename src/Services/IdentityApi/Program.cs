using IdentityApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = LogExtensions.ConfigureLoger()
        .CreateBootstrapLogger();

builder.Host.UseLogging();
builder.LogStartUp();

/*
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost",
                                              "http://localhost/identity");
                      });
});
*/
var app = builder.Build();

app.MapGet("/", () => "Identity API Hello World!");

//app.UseCors(MyAllowSpecificOrigins);

app.Run();
