namespace Dal;
using DalApi;
using DO;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer newE)
    {
        if (!DataSource.Engineers.Exists(x => x.Id == newE.Id))
            throw new Exception($"Engineer with ID={newE.Id} already exists");
        DataSource.Engineers.Add(newE);
        return newE.Id;
    }

    public void Delete(int id)
    {
        if (!DataSource.Engineers.Exists(x => x.Id == id) || DataSource.Tasks.Exists(x => x.EngineerId == id))
            throw new Exception("This object cannot be deleted");
        DataSource.Engineers.Remove(DataSource.Engineers.Find(x => x.Id == id)!);
    }

    public Engineer? Read(int id)
    {
        Engineer? e = DataSource.Engineers.Find(x => x.Id == id);
        return e;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Engineer with ID={item.Id} not exists");
        Delete(item.Id);
        Create(item);
    }
}
