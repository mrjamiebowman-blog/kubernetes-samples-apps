using Asp.Versioning;
using AutoMapper;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using MrJB.Kubernetes.Customers.Api;
using MrJB.Kubernetes.Customers.Api.Swagger;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class Builder
{
    public static WebApplicationBuilder ConfigureAzureApplicationConfiguration(this WebApplicationBuilder builder, bool required = false)
    {
        // settings
        var connStr = builder.Configuration.GetValue<string>("AZ_APPCONFIG_CONNECTION_STRING");
        var labelFilter = builder.Configuration.GetValue<string>("AZ_APPCONFIG_LABEL_FILTER") ?? "";

        // client id & secret
        var tenantId = builder.Configuration.GetValue<string>("AZ_TENANT_ID");
        var clientId = builder.Configuration.GetValue<string>("AAD_CLIENT_ID");
        var secret = builder.Configuration.GetValue<string>("AAD_CLIENT_SECRET");

        // validate
        List<string> missingConfig = new List<string>();

        if (String.IsNullOrWhiteSpace(tenantId))
        {
            missingConfig.Add("Tenant ID");
        }
        if (String.IsNullOrWhiteSpace(clientId))
        {
            missingConfig.Add("Client ID");
        }
        if (String.IsNullOrWhiteSpace(secret))
        {
            missingConfig.Add("Secret");
        }

        if (missingConfig.Count > 0)
        {
            string csvString = string.Join(", ", missingConfig.ToArray());

            if (required == true)
            {
                Log.Error($"Azure App Configuration is missing configuration values. Settings: {csvString}");
                throw new Exception($"Azure App Configuration is missing configuration values. Settings: {csvString}");
            } else
            {
                Log.Warning($"Azure App Configuration is missing configuration values. Settings: {csvString}");
            }
        }

        // azure app configuration
        if (!String.IsNullOrWhiteSpace(connStr))
        {
            builder.Configuration.AddAzureAppConfiguration((options =>
            {
                // label(s)
                options.Select(KeyFilter.Any);
                options.Select(KeyFilter.Any, labelFilter);
                options.Connect(connStr)
                    .ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(new ClientSecretCredential(tenantId, clientId, secret));
                    });
            }));
        }

        return builder;
    }

    public static IMapperConfigurationExpression ApplyMappingProfilesConsumer(this IMapperConfigurationExpression cfg)
    {
        //cfg.AddProfile(new SampleModelSaveRequest.MappingProfile());
        return cfg;
    }

    public static IServiceCollection ConfigureMapping(this IServiceCollection services, IConfiguration configuration)
    {
        // configure automapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.ApplyMappingProfilesConsumer();
            //cfg.ApplyMappingProfilesInfrastructure();
        });

        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
    public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // open telemetry
        var resource = ResourceBuilder
            .CreateDefault()
            .AddService(OTel.ActivitySourceName)
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector();

        services.AddOpenTelemetry()
            // tracing
            .WithTracing(builder =>
            {
                if (environment.IsDevelopment())
                {
                    builder.SetSampler(new AlwaysOnSampler());
                }
                builder.SetResourceBuilder(resource)
                       .AddSource(OTel.ActivitySource.Name)
                       //.AddEnterpriseRabbitMqInstrumentation() // when the package gets updated... we will have metrics from this package
                       //.AddCommonInstrumentations()
                       //.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
                //.AddAspNetCoreInstrumentationWithBaggage()
            })

            // metrics
            .WithMetrics(builder => builder
                .SetResourceBuilder(resource)
                .AddMeter(OTel.Meters.Consumer.Name)
                //.AddEnterpriseRabbitMqInstrumentation() // when the package gets updated... we will have metrics from this package
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddPrometheusExporter()
            //.AddPrometheusHttpListener(options => options.UriPrefixes = new string[] { "http://localhost:9001/" })
            );

        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // swagger gen
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        services.AddApiVersioning(o => {
            o.DefaultApiVersion = new ApiVersion(2, 0);
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ReportApiVersions = true;
            o.ApiVersionReader = ApiVersionReader.Combine(new MediaTypeApiVersionReader("x-api-version"), new HeaderApiVersionReader("x-api-version"));
        })
            .AddMvc()
            .AddApiExplorer(options => {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        // this will use the ConfigureSwaggerOptions.cs file.
        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication ConfigureSwaggerApp(this WebApplication app, IConfiguration configuration, IHostEnvironment environment)
    {
        // vars - in the future.. we may need to prefix our routes for complicated kubernetes route matching patterns... (this depends on which service mesh / ingress controller we use... def with nginx ingress (used to demo in minikube))
        string apiName = ""; // i.e., "customers/"

        app.UseSwagger(c => {
            c.RouteTemplate = apiName + "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c => {
            // docs
            c.RoutePrefix = apiName + "swagger";
            c.SwaggerEndpoint("v1/swagger.json", "Customers: V1");
            c.SwaggerEndpoint("v2/swagger.json", "Customers: V2");

            // identityserver
            c.OAuthClientId("clientname");
            c.OAuthClientSecret("secret");
            c.OAuthRealm("your-realm");
            c.DisplayRequestDuration();
            // custom styling
            //c.InjectStylesheet("/swagger-ui/custom.css");
        });

        return app;
    }
}
