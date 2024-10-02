using System.Security.Claims;
using System.Text;
using Events.Application.Authorization.Handlers;
using Events.Application.Authorization.Requirements;
using Events.Application.Configuration.Settings;
using Events.Domain.Constants;
using Events.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Events.WebAPI.Configuration;

public static class WebLayerConfiguration
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        services.AddScoped<ExceptionHandlingMiddleware>();

        services.ConfigureAuthentication(appSettings);

        services.ConfigureAuthorization(appSettings);

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        services.ConfigureSwagger();

        services.AddCors(options =>
        {
            options.AddPolicy(
                CorsPolicies.AllowSpecificOrigins,
                policy =>
                    policy
                        .WithOrigins(appSettings.CorsSettings.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
            );
        });

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        var jwtSettings = appSettings.JwtSettings;
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudiences = jwtSettings.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role,
                };
            });

        return services;
    }

    private static IServiceCollection ConfigureAuthorization(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        var authorizationSettings = appSettings.AuthorizationSettings;

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                AuthPolicies.AdministratorPolicy,
                policy => policy.RequireRole(authorizationSettings.RoleTitles.Administrator)
            )
            .AddPolicy(
                AuthPolicies.ParticipantPolicy,
                policy => policy.RequireRole(authorizationSettings.RoleTitles.Participant)
            )
            .AddPolicy(
                AuthPolicies.AccountOwnerOrAdministratorPolicy,
                policy => policy.AddRequirements(new AccountOwnerOrAdministratorRequirement())
            )
            .AddPolicy(
                AuthPolicies.NotificationOwnerOrAdministratorPolicy,
                policy => policy.AddRequirements(new NotificationOwnerOrAdministratorRequirement())
            )
            .AddPolicy(
                AuthPolicies.ParticipantOwnerOrAdministratorPolicy,
                policy =>
                    policy.AddRequirements(new ParticipantAccountOwnerOrAdministratorRequirement())
            );

        services.AddSingleton<
            IAuthorizationHandler,
            AccountOwnerOrAdministratorAuthorizationHandler
        >();
        services.AddSingleton<
            IAuthorizationHandler,
            NotificationOwnerOrAdministratorAuthorizationHandler
        >();
        services.AddSingleton<
            IAuthorizationHandler,
            ParticipantAccountOwnerOrAdministratorAuthorizationHandler
        >();

        return services;
    }

    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Events Web API",
                    Version = "v1.0",
                    Description = "An ASP.NET Core Web API for event and participant management.",
                }
            );

            opt.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                }
            );

            opt.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }
}
