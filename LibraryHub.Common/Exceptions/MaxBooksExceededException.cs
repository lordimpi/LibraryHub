namespace LibraryHub.Common.Exceptions;

/// <summary>
/// Representa un error cuando se excede el máximo de libros permitidos.
/// </summary>
public class MaxBooksExceededException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la excepción.
    /// </summary>
    public MaxBooksExceededException()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la excepción con un mensaje personalizado.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public MaxBooksExceededException(string message) : base(message)
    {
    }
}
