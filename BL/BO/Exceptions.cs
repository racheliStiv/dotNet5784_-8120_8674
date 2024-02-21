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
[Serializable]
public class BOXMLFileLoadCreateException : Exception
{
    public BOXMLFileLoadCreateException(string? message) : base(message) { }
}

//exeption for invalid update
[Serializable]
public class BOInvalidUpdateException : Exception
{
    public BOInvalidUpdateException(string? message) : base(message) { }
}
[Serializable]

//exeption for null obj in update function
public class BONullObj : Exception
{
    public BONullObj(string? message) : base(message) { }
}

[Serializable]
public class BOTaskAlreadyOccupied : Exception
{
    public BOTaskAlreadyOccupied(string? message) : base(message) { }
}
[Serializable]
public class BOTaskIsDone : Exception
{
    public BOTaskIsDone(string? message) : base(message) { }
}
[Serializable]
public class BOCannotAddNewOne : Exception
{
    public BOCannotAddNewOne(string? message) : base(message) { }
}