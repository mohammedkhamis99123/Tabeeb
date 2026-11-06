using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tabeeb.Filters // قم بتعديل الـ Namespace حسب مشروعك
{
    public class SwaggerFormListFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody?.Content == null)
                return;

            // البحث عن محتوى multipart/form-data
            var formContent = operation.RequestBody.Content
                .FirstOrDefault(c => c.Key.Equals("multipart/form-data", StringComparison.OrdinalIgnoreCase));

            if (formContent.Value != null && formContent.Value.Schema != null)
            {
                var mediaType = formContent.Value;
                var schema = mediaType.Schema;

                if (schema.Properties != null)
                {
                    foreach (var prop in schema.Properties)
                    {
                        // نتحقق مما إذا كانت الخاصية عبارة عن مصفوفة (Array / List)
                        if (prop.Value.Type == "array")
                        {
                            // تعيين الـ Encoding داخل الـ OpenApiMediaType مباشرة وليس الـ Schema
                            mediaType.Encoding[prop.Key] = new OpenApiEncoding
                            {
                                ContentType = "application/json"
                            };
                        }
                    }
                }
            }
        }
    }
}