using Azure.Identity;
using MessageReceiverApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var keyVaultUrl = builder.Configuration["KeyVaultUrl"] ?? string.Empty;
var creds = new DefaultAzureCredential(new DefaultAzureCredentialOptions 
{
    ManagedIdentityClientId = "7494deba-68fe-48fe-a074-aef13a3446be"
});
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), creds);

var queueServiceConnectionString = builder.Configuration["StorageConnectionString"] ?? string.Empty;
builder.Services.AddSingleton(new QueueListenerService(queueServiceConnectionString, "webapptest"));

builder.Services.AddHostedService(sp => sp.GetRequiredService<QueueListenerService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
