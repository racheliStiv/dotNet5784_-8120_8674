namespace DalApi;
using DO;
public interface ITask
{
    int Create(Task item); //Creates new Task object in DAL
    Task? Read(int id); //Reads Task object by its ID 
    List<Task> ReadAll(); //stage 1 only, Reads all Task objects
    void Update(Task item); //Updates Task object
    void Delete(int id); //Deletes a Task object by its Id
}
