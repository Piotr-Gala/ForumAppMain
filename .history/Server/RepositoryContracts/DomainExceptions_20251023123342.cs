namespace RepositoryContracts;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string message) : base(message) { }
}

public class DuplicateEntityException : DomainException
{
    public DuplicateEntityException(string message) : base(message) { }
}

public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
}
