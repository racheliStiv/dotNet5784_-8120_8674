namespace DalApi;
using DO;
public interface IEngineer
{
    int Create(Engineer item); //Creates new Engineer object in DAL
    Engineer? Read(int id); //Reads Engineer object by its ID 
    List<Engineer> ReadAll(); //stage 1 only, Reads all Engineer objects
    void Update(Engineer item); //Updates Engineer object
    void Delete(int id); //Deletes an Engineer object by its Id
}
