namespace DigitalBanking.BAL.Exceptions;

public sealed class AccountOperationException(string message, Exception innerException)
    : Exception(message, innerException);