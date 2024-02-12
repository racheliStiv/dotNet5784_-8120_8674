using System;
namespace BO;
[Serializable]

//exeption to not exist obj access
public class BODoesNotExistException : Exception
{
    public BODoesNotExistException(string? message) : base(message) { }
}

[Serializable]
//exeption to creating existing item
public class BOAlreadyExistsException : Exception
{
    public BOAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
//exeption to invalid details
public class BOInvalidDetailsException : Exception
{
    public BOInvalidDetailsException(string? message) : base(message) { }
}

[Serializable]

//exeption to deleting of shared obj
public class BODeletionImpossibleException : Exception
{
    public BODeletionImpossibleException(string? message) : base(message) { }
}

//exeption to empty collection or uninitialization object
[Serializable]
public class BOCanNotBeNullException : Exception
{
    public BOCanNotBeNullException(string? message) : base(message) { }
}
//exeption for workin with XML 
public class BOXMLFileLoadCreateException : Exception
{
    public BOXMLFileLoadCreateException(string? message) : base(message) { }
}