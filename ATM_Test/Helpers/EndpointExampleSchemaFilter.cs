using ATM_Test.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ATM_Test.Helpers;

/// <summary>
/// Endpoint example schema filter for Swagger documentation.
/// </summary>
public class EndpointExampleSchemaFilter : ISchemaFilter
{
    private const int OpenApiValue = 16000;

    /// <summary>
    /// Applys the schema filter to the OpenAPI schema.
    /// </summary>
    /// <param name="schema">Schema</param>
    /// <param name="context">Schema filter context</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // deposit endpoint
        if (context.Type == typeof(Dictionary<string, uint>))
        {
            schema.Example = new OpenApiObject()
            {
                [Denomation.TenThousand.Unit.ToString()] = new OpenApiString("2"),
                [Denomation.FiveThousand.Unit.ToString()] = new OpenApiString("3"),
            };
        }
        // withdraw endpoint
        else if (context.Type == typeof(ulong))
        {
            schema.Example = new OpenApiLong(OpenApiValue) { };
        }
    }
}
