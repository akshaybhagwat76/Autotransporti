using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ASP
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "PP Autotrasporti API");
                        c.OperationFilter<AddRequiredHeaderParameter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("PP Autotrasporti API");
                    });
        }
    }
}

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
        if (operation.parameters == null)
            operation.parameters = new List<Parameter>();

        var authorize = apiDescription.GetControllerAndActionAttributes<ApiKeyAuth>().Any();
        var token = apiDescription.GetControllerAndActionAttributes<TokenAuth>().Any();
        if (authorize)
        {
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                description = "API Key (key xxxxxxx)",
                required = true
            });
        }
        if (token)
        {
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                description = "Access token (token xxxxxxx)",
                required = true
            });
        }
    }
}
