using ClubesDeportivos.Datos;

var builder = WebApplication.CreateBuilder(args);
var conexion = builder.Configuration.GetConnectionString("Conexion")!;
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new ClubDatos(conexion));
builder.Services.AddSingleton(new MiembroDatos(conexion));
builder.Services.AddSingleton(new RegistroDatos(conexion));

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute("default", "{controller=Clubes}/{action=Index}/{id?}");
app.Run();
