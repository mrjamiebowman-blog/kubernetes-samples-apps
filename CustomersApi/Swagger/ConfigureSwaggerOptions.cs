using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Asp.Versioning.ApiExplorer;

namespace MrJB.Kubernetes.Customers.Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        // auth
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        // oauth
        options.AddSecurityDefinition("OAuth", new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth" },
            Type = SecuritySchemeType.OAuth2,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Flows = new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow
                {
                    TokenUrl = new Uri("https://demo.identityserver.io/connect/token"),
                    Scopes = new Dictionary<string, string> {
                        { "api", "For accessing the API at all" }
                    }
                }
            }
        });

        // apis
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.ResolveConflictingActions(r => {
            return r.First();
        });

        // filter
        options.OperationFilter<AuthorizeCheckOperationFilter>();

        // xml doc - (<GenerateDocumentationFile>true</GenerateDocumentationFile>)
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Version = description.ApiVersion.ToString(),
            Title = $"Customers {description.GroupName.ToUpper()}",
            Description = "Our API Description",
            Contact = new OpenApiContact
            {
                Name = "Architecture",
                Email = "info@mrjamiebowman.com",
                Url = new Uri("https://www.mrjamiebowman.com/")
            },
            //License = new OpenApiLicense
            //{
            //    Name = ""
            //}
        };

        if (description.IsDeprecated)
        {
            info.Description += "This API version has been deprecated.";
        }

        return info;
    }
}
