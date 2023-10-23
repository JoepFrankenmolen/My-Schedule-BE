namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IContextBase : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}