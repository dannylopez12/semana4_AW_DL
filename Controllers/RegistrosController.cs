using ClubesDeportivos.Datos;
using ClubesDeportivos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Controllers;

public class RegistrosController : Controller
{
    private readonly RegistroDatos registros;
    private readonly ClubDatos clubes;
    private readonly MiembroDatos miembros;

    public RegistrosController(RegistroDatos registros, ClubDatos clubes, MiembroDatos miembros)
    {
        this.registros = registros;
        this.clubes = clubes;
        this.miembros = miembros;
    }

    public IActionResult Index()
    {
        try
        {
            CargarListas();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View(new Registro { FechaRegistro = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Guardar(Registro registro)
    {
        if (registro.FechaRegistro > DateTime.Today)
        {
            ModelState.AddModelError(nameof(Registro.FechaRegistro), "La fecha de registro no puede ser futura.");
        }
        try
        {
            if (ModelState.IsValid)
            {
                registros.Insertar(registro);
                TempData["Mensaje"] = "Miembro registrado en el club correctamente.";
                return RedirectToAction("Index");
            }
            CargarListas();
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            ViewBag.Error = "Ese miembro ya está registrado en ese club.";
            CargarListas();
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            ViewBag.Error = "El club o el miembro seleccionado ya no existe.";
            CargarListas();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View("Index", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        try
        {
            registros.Eliminar(id);
            TempData["Mensaje"] = "Registro eliminado correctamente.";
        }
        catch (SqlException)
        {
            TempData["Error"] = "No se pudo eliminar el registro.";
        }
        return RedirectToAction("Index");
    }

    private void CargarListas()
    {
        ViewBag.Registros = registros.Listar();
        ViewBag.Clubes = clubes.Listar();
        ViewBag.Miembros = miembros.Listar();
    }
}
