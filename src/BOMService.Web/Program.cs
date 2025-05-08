using BOMService.Web.Extensions;
using BOMService.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationLayers();
builder.Services.ConfigureSwagger();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
