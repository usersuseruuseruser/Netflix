using API;
using API.Controllers;
using API.Hubs;
using API.MetadataProviders;
using API.Middlewares.ExceptionHandler;
using DataAccess.Extensions;
using Infrastructure;
using Application;
using Infrastructure.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("authAppSettings.json");

builder.Services.AddSignalR();
builder.Services.Configure<FrontendConfig>(builder.Configuration.GetSection("FrontendConfig"));
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddControllers().AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new CustomMetadataProvider ()));
builder.Services.AddContentApiServices();
builder.Services.AddAutoMapper(typeof(ContentProfile));
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseExceptionHandlerMiddleware();

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/hub/notification");
app.MapControllers();

app.Run();

public partial class Program;