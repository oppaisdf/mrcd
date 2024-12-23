namespace api.Common;

public class AlreadyExistsException(
    string message
) : Exception(message)
{
}

public class DoesNotExistsException(
    string message
) : Exception(message)
{ }

public class BadRequestException(
    string message
) : Exception(message)
{ }