namespace DigitalBanking.BAL.Exceptions;

public sealed class CustomerOperationException(string message, Exception innerException)
    : Exception(message, innerException);