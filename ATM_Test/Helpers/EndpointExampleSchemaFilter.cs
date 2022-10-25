using ATM_Test.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ATM_Test.Helpers
{
    public class EndpointExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // deposit
            if (context.Type == typeof(Dictionary<string, uint>))
            {
                schema.Example = new OpenApiObject()
                {
                    [Denomation.TenThousand.Unit.ToString()] = new OpenApiString("2"),
                    [Denomation.FiveThousand.Unit.ToString()] = new OpenApiString("3"),
                };
            }
            else if (context.Type == typeof(ulong))
            {
                schema.Example = new OpenApiLong(16000) { };
            }

        }
    }
}
