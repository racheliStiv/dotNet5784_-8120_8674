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
        throw new Exception("This object cannot be deleted");
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
            throw new Exception($"Engineer with ID={item.Id} already exists");
        Delete(item.Id);
        Create(item);
    }
}
