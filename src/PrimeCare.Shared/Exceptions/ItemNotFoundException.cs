namespace PrimeCare.Shared.Exceptions;

/// <summary>
/// Exception that is thrown when an item is not found in the data store.
/// </summary>
/// <param name="message">The message that describes the error.</param>
public class ItemNotFoundException(string message) : Exception(message)
{
}
