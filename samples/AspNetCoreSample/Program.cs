using Colosoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddColosoftDiagnostics(options =>
{
    options.ShowStackTrace = true;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<DiagnosticsExceptionFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
