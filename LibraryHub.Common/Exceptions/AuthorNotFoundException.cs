namespace LibraryHub.Common.Exceptions;

/// <summary>
/// Representa un error cuando un autor no existe en el sistema.
/// </summary>
public class AuthorNotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la excepción.
    /// </summary>
    public AuthorNotFoundException()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la excepción con un mensaje personalizado.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public AuthorNotFoundException(string message) : base(message)
    {
    }
}
