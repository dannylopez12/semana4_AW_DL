using ClubesDeportivos.Datos;
using ClubesDeportivos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Controllers;

public class MiembrosController : Controller
{
    private readonly MiembroDatos datos;

    public MiembrosController(MiembroDatos datos)
    {
        this.datos = datos;
    }

    public IActionResult Index(int? editar)
    {
        var miembro = new Miembro();
        try
        {
            if (editar != null)
            {
                miembro = datos.Buscar(editar.Value) ?? new Miembro();
                if (miembro.MiembroId == 0)
                {
                    ViewBag.Error = "El miembro no existe.";
                }
            }
            ViewBag.Miembros = datos.Listar();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View(miembro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Guardar(Miembro miembro)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (miembro.MiembroId == 0)
                {
                    datos.Insertar(miembro);
                    TempData["Mensaje"] = "Miembro registrado correctamente.";
                }
                else
                {
                    datos.Actualizar(miembro);
                    TempData["Mensaje"] = "Miembro actualizado correctamente.";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Miembros = datos.Listar();
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            ModelState.AddModelError(nameof(Miembro.Email), "Ya existe un miembro con ese email.");
            ViewBag.Miembros = datos.Listar();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View("Index", miembro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        try
        {
            datos.Eliminar(id);
            TempData["Mensaje"] = "Miembro eliminado correctamente.";
        }
        catch (SqlException)
        {
            TempData["Error"] = "No se pudo eliminar el miembro.";
        }
        return RedirectToAction("Index");
    }
}
