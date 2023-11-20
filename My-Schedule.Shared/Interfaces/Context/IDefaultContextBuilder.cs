using Microsoft.EntityFrameworkCore;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IDefaultContextBuilder
    {
        T CreateContext<T>() where T : DbContext;
    }
}