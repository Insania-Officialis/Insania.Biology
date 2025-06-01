using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Insania.Biology.Middleware;

/// <summary>
/// Класс фильтров операции для сваггера
/// </summary>
public class AuthenticationRequirementsOperationFilter : IOperationFilter
{
    /// <summary>
    /// Метод применение токена
    /// </summary>
    /// <param cref="OpenApiOperation" name="operation">Операция</param>
    /// <param cref="OperationFilterContext" name="context">Контекст запроса</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Security ??= [];

        OpenApiSecurityScheme scheme = new()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        operation.Security.Add(new OpenApiSecurityRequirement { [scheme] = [] });
    }
}