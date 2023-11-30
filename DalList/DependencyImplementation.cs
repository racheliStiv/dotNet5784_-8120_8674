namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{

    //CRUD of Dependency
    public int Create(Dependency item)
    {
        int newId =  DataSource.Config.NextDependencyId;
        Dependency d = new(newId, item.DependentTask, item.DependensOnTask) ;
        DataSource.Dependencies.Add(d);
        return newId;   
    }

    public void Delete(int id)
    {
        //remove the dependency from dependencies list, throw ex in case this ID doesn't exsist
        DataSource.Dependencies.Remove(DataSource.Dependencies.Find(x => x.Id == id) ?? throw new Exception($"Dependency with ID={id} not exists"));
    }

    public Dependency? Read(int id)
    {
        //return the dependency of received id, return null in case ID doesn't exsist
        Dependency? d = DataSource.Dependencies.Find(x => x.Id == id);
        return d;
    }

    public List<Dependency> ReadAll()
    {
        //return all dependency list
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        //delete in case item exsist
        Delete(item.Id);

        //create with original id
        DataSource.Dependencies.Add(item);
    }
}
