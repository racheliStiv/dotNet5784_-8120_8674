namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId =  DataSource.Config.NextDependencyId;
        Dependency d = new(newId, item.DependentTask, item.DependensOnTask) ;
        DataSource.Dependencies.Add(d);
        return newId;   
    }

    public void Delete(int id)
    {
        DataSource.Dependencies.Remove(DataSource.Dependencies.Find(x => x.Id == id) ?? throw new Exception($"Dependency with ID={id} not exists"));
    }

    public Dependency? Read(int id)
    {
        Dependency? d = DataSource.Dependencies.Find(x => x.Id == id);
        return d;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Dependency with ID={item.Id} not exists");
        Delete(item.Id);
        Create(item);
    }
}
