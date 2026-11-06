using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Nodes;

namespace Tabeeb.Filters;

public class Language
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class SwaggerHeaderLanguageFilter : IOperationFilter
{
    private readonly List<Language> _languages = new()
    {
        new Language { Name = "Arabic",  Code = "ar"    },
        new Language { Name = "English", Code = "en-US" },
    };

  
    

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Enum = _languages.Select(l => (IOpenApiAny)new OpenApiString(l.Code)).ToList(),
            Default = (IOpenApiAny)new OpenApiString(_languages[0].Code),
        };

        var parameter = new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Required = false,
            Description = "The language to use for the response",
            Schema = schema,
        };

        operation.Parameters ??= new List<OpenApiParameter>();

        var existing = operation.Parameters.FirstOrDefault(p => p.Name == "Accept-Language");
        if (existing is not null)
            operation.Parameters.Remove(existing);

        operation.Parameters.Add(parameter);
    }
}




