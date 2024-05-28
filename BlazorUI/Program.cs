// ReSharper disable ConvertTypeCheckPatternToNullCheck

using BlazorUI.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]));
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseMiddleware<FirstMiddleware>();
// app.UseMiddleware<SecondMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

/**
 * différents points pour scanner fichier :
 *     extension
 *     macro docx
 *     antivirus => voir avec Bao ? Sinon
 *                                  a) DIY
 *                                  b) fake one
 * middleware d'exception : try catch général -> pas http donc complexe
 * filtre de validation FluentValidation
 *      (avec et sans implémentation Blazor)
 */