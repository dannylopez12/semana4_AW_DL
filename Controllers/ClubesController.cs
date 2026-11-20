using ClubesDeportivos.Datos;
using ClubesDeportivos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Controllers;

public class ClubesController : Controller
{
    private readonly ClubDatos datos;

    public ClubesController(ClubDatos datos)
    {
        this.datos = datos;
    }

    public IActionResult Index(int? editar)
    {
        var club = new Club();
        try
        {
            if (editar != null)
            {
                club = datos.Buscar(editar.Value) ?? new Club();
                if (club.ClubId == 0)
                {
                    ViewBag.Error = "El club no existe.";
                }
            }
            ViewBag.Clubes = datos.Listar();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View(club);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Guardar(Club club)
    {
        if (club.FechaFundacion > DateTime.Today)
        {
            ModelState.AddModelError(nameof(Club.FechaFundacion), "La fecha de fundación no puede ser futura.");
        }
        try
        {
            if (ModelState.IsValid)
            {
                if (club.ClubId == 0)
                {
                    datos.Insertar(club);
                    TempData["Mensaje"] = "Club registrado correctamente.";
                }
                else
                {
                    datos.Actualizar(club);
                    TempData["Mensaje"] = "Club actualizado correctamente.";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Clubes = datos.Listar();
        }
        catch (SqlException)
        {
            ViewBag.SinConexion = true;
        }
        return View("Index", club);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        try
        {
            datos.Eliminar(id);
            TempData["Mensaje"] = "Club eliminado correctamente.";
        }
        catch (SqlException)
        {
            TempData["Error"] = "No se pudo eliminar el club.";
        }
        return RedirectToAction("Index");
    }
}
