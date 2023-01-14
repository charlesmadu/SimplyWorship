using ElectronNET.API;
using ElectronNET.API.Entities;

var builder = WebApplication.CreateBuilder(args);

//Use Electron

builder.WebHost.UseElectron(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

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

if (HybridSupport.IsElectronActive)
{
    CreateMenu();
    CreateElectronWindow();
}

app.Run();

async void CreateElectronWindow()
{
    var options = new BrowserWindowOptions
    {
        Width = 800,
        Height = 500,
    };

    Display extraScreen = null;
    var hold = await Electron.Screen.GetAllDisplaysAsync();
    foreach(var screen in hold){
        if(screen.Bounds.X != 0 || screen.Bounds.Y!= 0){
            extraScreen = screen;
        }
    }

    var displayOptions = new BrowserWindowOptions
    {
        Show = true,
    };

    var displayWindow = await Electron.WindowManager.CreateWindowAsync(displayOptions, "http://localhost:8001/Home/LivePage");
    var window = await Electron.WindowManager.CreateWindowAsync(options);

    Rectangle bounds = new Rectangle();
    bounds.X = extraScreen.Bounds.X;
    bounds.Y = extraScreen.Bounds.Y;
    
    displayWindow.SetBounds(bounds);
    displayWindow.SetFullScreen(true);

    
    window.OnClosed += () => Electron.App.Quit();
    displayWindow.OnClosed += () => Electron.App.Quit();
}

void CreateMenu()
{
    var fileMenu = new MenuItem[]
    {
        new MenuItem { Label = "Home", Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/") },
        new MenuItem { Label = "Privacy", Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/Home/Privacy") },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.quit }
    };
    var viewMenu = new MenuItem[]
    {
        new MenuItem { Role = MenuRole.reload },
        new MenuItem { Role = MenuRole.forcereload },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.togglefullscreen }
    };
    var menu = new MenuItem[] 
    {
        new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },
        new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }
    };
    Electron.Menu.SetApplicationMenu(menu);
}
