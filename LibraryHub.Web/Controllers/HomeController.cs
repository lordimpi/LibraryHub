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
    /// Redirige al módulo principal del dashboard SPA.
    /// </summary>
    /// <returns>Redirección a la ruta de autores.</returns>
    public IActionResult Index()
    {
        return RedirectPermanent("/LibraryHub/authors");
    }

    /// <summary>
    /// Hospeda la interfaz SPA-like de LibraryHub.
    /// </summary>
    /// <returns>Vista principal del dashboard.</returns>
    [HttpGet]
    [Route("LibraryHub/{*path}")]
    [Route("LibaryHub/{*path}")]
    public IActionResult LibraryHub()
    {
        if (Request.Path.StartsWithSegments("/LibaryHub", StringComparison.OrdinalIgnoreCase))
        {
            var correctedPath = Request.Path.Value?
                .Replace("/LibaryHub", "/LibraryHub", StringComparison.OrdinalIgnoreCase)
                ?? "/LibraryHub/authors";

            return RedirectPermanent($"{correctedPath}{Request.QueryString}");
        }

        ViewData["Title"] = "LibraryHub Dashboard";
        return View("Index");
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
