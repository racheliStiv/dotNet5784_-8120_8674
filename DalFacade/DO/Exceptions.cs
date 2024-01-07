using System;

namespace DO;
[Serializable]

//exeption to not exist obj access
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
//exeption to creating existing item
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]

//exeption to deleting of shared obj
public class DalDeletionImpossibleException : Exception
{
    public DalDeletionImpossibleException(string? message) : base(message) { }
}

//exeption to empty collection or uninitialization object
[Serializable]
public class DalCanNotBeNullException : Exception
{
    public DalCanNotBeNullException(string? message) : base(message) { }
}
//exeption for workin with XML 
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}