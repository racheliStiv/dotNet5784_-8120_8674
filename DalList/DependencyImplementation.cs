namespace Dal;
using DalApi;
using DO;

internal class DependencyImplementation : IDependency
{

    //CRUD of Dependency
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency d = new(newId, item.DependentTask, item.DependensOnTask);
        DataSource.Dependencies.Add(d);
        return newId;
    }

    public void Delete(int id)
    {
        //remove the dependency from dependencies collection, throw ex in case this ID doesn't exsist
        DataSource.Dependencies.Remove(DataSource.Dependencies.FirstOrDefault(x => x.Id == id) ?? throw new DalDoesNotExistException($"Dependency with ID={id} not exists"));
    }

    public Dependency? Read(int id)
    {
        //return the dependency of received id, return null in case ID doesn't exsist
        return DataSource.Dependencies.FirstOrDefault(x => x.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter) //stage 2
    {
        //return the first dependency that meet the condition
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter)
    {
        //return all dependency collection in case there is no filter
        if (filter == null)
            return DataSource.Dependencies.Select(x => x);

        //return the dependencies that meet the condition 
        return DataSource.Dependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        //delete in case item exsist
        Delete(item.Id);

        //create with original id
        DataSource.Dependencies.Add(item);
    }

    public void Reset()
    {
        DataSource.Dependencies.Clear();
    }

}
