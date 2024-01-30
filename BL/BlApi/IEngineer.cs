namespace BlApi;

public interface IEngineer
{
    public IEnumerable<Engineer?> GetAllEngineers(Func<Engineer, bool>? filter);
    public Engineer GetEngineerDetails(Engineer engineer);
    public void Create(Engineer engineer);
    public void Delete(int id);
    public void Update(Engineer engineer);
}
