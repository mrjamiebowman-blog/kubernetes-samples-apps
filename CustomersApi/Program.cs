using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// serilog
var seriLoggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
        .WriteTo.Console()
    ;

//if (builder.Environment.IsEnvironment("Local"))
//{
//    seriLoggerConfiguration.WriteTo.Seq("http://localhost:5340");
//}

// create logger
Log.Logger = seriLoggerConfiguration.CreateBootstrapLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog(Log.Logger);

Log.Information("Staring Consumer Service...");
Log.Information("Environment: {environment}", builder.Environment.EnvironmentName);

/****************************************/
/*              configuration           */
/****************************************/

// user secrets
if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Local"))
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}

// env vars
builder.Configuration.AddEnvironmentVariables();

// azure app config
builder.ConfigureAzureApplicationConfiguration();

/****************************************/
/*              auth                    */
/****************************************/

builder.Services.AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://demo.duendesoftware.com";

        // confidential client using code flow + PKCE
        options.ClientId = "api.customers";
        options.ClientSecret = "C86ABBFF-629B-4FBB-9E01-6EBE65E579F8";
        options.ResponseType = "code";

        // query response type is compatible with strict SameSite mode
        options.ResponseMode = "query";

        // get claims without mappings
        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;

        // save tokens into authentication session
        // to enable automatic token management
        options.SaveTokens = true;

        // request scopes
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api");

        // and refresh token
        options.Scope.Add("offline_access");
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


/****************************************/
/*              swagger                 */
/****************************************/

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


/****************************************/
/*              configure               */
/****************************************/

builder.Services.AddAuthorization(options => {
    options.AddPolicy("api.customers", policy => policy.RequireClaim("api.customers"));
});

// problem details
builder.Services.AddProblemDetails();

// otel
builder.AddServiceDefaults();

/****************************************/
/*              build                   */
/****************************************/

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

} else
{
    app.UseHttpsRedirection();
}

/****************************************/
/*              swagger                 */
/****************************************/

app.ConfigureSwaggerApp(builder.Configuration, builder.Environment);

/****************************************/
/*              health checks           */
/****************************************/

// status check
//app.MapHealthChecks("/status", HealthCheckBase.GetHealthCheckOptions());

/****************************************/
/*              api                     */
/****************************************/

app.MapGet("/customers", () =>
{
    return "Customers";
})
.WithName("customers")
.RequireAuthorization()
.WithOpenApi();


app.MapGet("/customers/admin", () =>
{
    return "Customers (Admin)";
})
.WithName("customers-admin")
.RequireAuthorization()
.WithOpenApi();


app.MapGet("/up", () =>
{
    return "UP";
})
.WithName("UP")
.AllowAnonymous()
.WithOpenApi();

// fail: out of memory
app.MapGet("/fail/out-of-memory", () =>
{
    return "OOM";
})
.WithName("out-of-memory")
.AllowAnonymous()
.WithOpenApi();

try
{
    app.Run();
} catch (Exception ex)
{
    throw ex;
}

namespace MrJB.Kubernetes.Customers.Api
{
    public partial class Program { }
}
