namespace LibraryHub.Common.Exceptions;

/// <summary>
/// Representa un error cuando un libro no existe en el sistema.
/// </summary>
public class BookNotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la excepcion.
    /// </summary>
    public BookNotFoundException()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la excepcion con un mensaje personalizado.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public BookNotFoundException(string message) : base(message)
    {
    }
}
