namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{

    //CRUD of Engineer
    public int Create(Engineer newE)
    {
        //check if the received ID doesn't exsist, throe ex in case not
        if (DataSource.Engineers.FirstOrDefault(item => item.Id == newE.Id) != null)
            throw new DalAlreadyExistsException($"Engineer with ID={newE.Id} already exists");

        //create a new engineer & add its to engineers collection
        DataSource.Engineers.Add(newE);
        return newE.Id;
    }

    public void Delete(int id)
    {
        //check if the ID number received does exist & does not appear in another collection, throw ex in case not
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} doesn't exists");

        if (DataSource.Tasks.FirstOrDefault(x => x.EngineerId == id) != null)
            throw new DalDeletionImpossibleException("This object cann't be deleted");

        //remove the engineer from engineers collection
        DataSource.Engineers.Remove(DataSource.Engineers.FirstOrDefault(x => x.Id == id)!);
    }

    public Engineer? Read(int id)
    {
        //return the engineer of received id, return null in case ID doesn't exsist
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter ) //stage 2
    {
        //return the first engineer that meet the condition
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter)
    {
        //return all engineers collection in case there is no filter
        if (filter == null)
            return DataSource.Engineers.Select(x => x);

        //return the engineers that meet the condition 
        return DataSource.Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        //check if item exsist
        Engineer? src_eng = Read(item.Id);
        if (src_eng == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} not exists");
        //delete the original engineer
        DataSource.Engineers.Remove(src_eng);
        //create the updated engineer
        DataSource.Engineers.Add(item);
    }

    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
