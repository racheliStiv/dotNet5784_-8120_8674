namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{

    //CRUD of Engineer
    public int Create(Engineer newE)
    {
        //check if the received ID doesn't exsist, throe ex in case not
        if (DataSource.Engineers.Exists(x => x.Id == newE.Id))
            throw new Exception($"Engineer with ID={newE.Id} already exists");

        //create a new engineer & add its to engineers list
        DataSource.Engineers.Add(newE);
        return newE.Id;
    }

    public void Delete(int id)
    {
        //check if the ID number received does exist & does not appear in another list, throw ex in case not
        if (!DataSource.Engineers.Exists(x => x.Id == id) || DataSource.Tasks.Exists(x => x.EngineerId == id))
            throw new Exception("This object cannot be deleted");

        //remove the engineer from engineers list
        DataSource.Engineers.Remove(DataSource.Engineers.Find(x => x.Id == id)!);
    }

    public Engineer? Read(int id)
    {
        //return the engineer of received id, return null in case ID doesn't exsist
        Engineer? e = DataSource.Engineers.Find(x => x.Id == id);
        return e;
    }

    public List<Engineer> ReadAll()
    {
        //return all engineers list
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        //check if item exsist
        if (Read(item.Id) == null)
            throw new Exception($"Engineer with ID={item.Id} not exists");
        //delete the original engineer
        DataSource.Engineers.Remove(DataSource.Engineers.Find(x => x.Id == item.Id)!);

        //create the updated engineer
        DataSource.Engineers.Add(item);
    }
}
