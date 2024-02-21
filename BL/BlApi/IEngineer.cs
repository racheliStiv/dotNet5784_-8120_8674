namespace BlApi;

public interface IEngineer
{
    public IEnumerable<Engineer?> GetAllEngineers(Func<Engineer, bool>? filter = null);
    public Engineer GetEngineerDetails(int id);
    public int Create(Engineer? engineer);
    public void Delete(int id);
    public void Update(Engineer? engineer);
}
