using Mapster;
using MapsterMapper;
using QuestPDF.Drawing;
using System.Reflection;
using Tabeeb.Application;
using Tabeeb.Application.Mapping;
using Tabeeb.Configuration;
using Tabeeb.Infrastructure.DbConfiguration;
using Tabeeb.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.ConfigDb(builder.Configuration);
builder.Services.ConfigSwagger();

builder.Host.AddSerilogConfiguration();
builder.Services.ConfigIdentity();
builder.Services.AddLocalizationConfig();
builder.Services.ConfigJWT(builder.Configuration);
builder.Services.ConfigCors();
//builder.Services.ConfigMapping();
builder.Services.ConfigMediatR();
builder.Services.ConfigDependencyInjection();
builder.Services.AddRateLimitConfig();

var fontPath = Path.Combine(builder.Environment.WebRootPath, "Fonts", "Cairo-Regular.ttf");
if (File.Exists(fontPath))
{
    using var stream = File.OpenRead(fontPath);
    FontManager.RegisterFont(stream);
}
ClinicMapping.Run();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedDataAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "حدث خطأ أثناء إضافة البيانات الأولية لقاعدة البيانات.");
    }
}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}
app.UseCors("AllowAll");

app.ConfigureLocalization();
app.UseSerilogConfiguration();


app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tabeeb API v1");

    // (إختياري) لجعل صفحة Swagger هي الصفحة الرئيسية عند تشغيل المشروع بدلاً من /swagger
    options.RoutePrefix = string.Empty;
});
app.UseMiddleware<GlobalErrorMiddleware>();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
