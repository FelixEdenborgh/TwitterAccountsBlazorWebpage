using Autofac.Core;
using TwitterAccountsBlazorWebpage.Components;
using TwitterAccountsBlazorWebpage.Rss_Feed_Import;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<RssFeedService>();
builder.Services.AddHttpClient<TwitterRSSFeedService>();
builder.Services.AddSingleton<RiksArticlesRssFeedService>();



// Lägg till Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Lägg till Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseResponseCompression(); // Lägg till denna rad

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
