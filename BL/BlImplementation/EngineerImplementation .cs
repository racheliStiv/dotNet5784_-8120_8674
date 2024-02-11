using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class EngineerImplementation : BlApi.IEngineer
    {
        public void Create(Engineer engineer)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Engineer?> GetAllEngineers(Func<Engineer, bool>? filter)
        {
            throw new NotImplementedException();
        }

        public Engineer GetEngineerDetails(Engineer engineer)
        {
            throw new NotImplementedException();
        }

        public void Update(Engineer engineer)
        {
            throw new NotImplementedException();
        }
    }
}

//public int Create(Task boTask)
//{
//    DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.StartDate, boTask.PlannedStartDate, boTask.Duration, boTask.PlannedFinishDate, boTask.ComletedDate, boTask.Product, boTask.Remarks, boTask.Engineer?.Id, (DO.EngineerExperience)boTask.ComplexityLevel);
//    try
//    {

//        int idTask = _dal.Task.Create(doTask);
//        return idTask;
//    }
//    catch (DO.DalAlreadyExistsException ex)
//    {
//        throw new BO.BOAlreadyExistsException(ex.Message);

//    }
//}