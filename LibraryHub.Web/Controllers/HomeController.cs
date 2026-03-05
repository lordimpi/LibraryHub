using LibraryHub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryHub.Web.Controllers;

/// <summary>
/// Controlador principal de páginas públicas de la aplicación MVC.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Muestra la vista principal.
    /// </summary>
    /// <returns>Vista de inicio.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Muestra la vista de privacidad.
    /// </summary>
    /// <returns>Vista de privacidad.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Muestra la vista de error con el identificador de la solicitud actual.
    /// </summary>
    /// <returns>Vista de error.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
