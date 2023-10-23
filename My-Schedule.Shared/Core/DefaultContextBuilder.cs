using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;

namespace My_Schedule.Shared.Core
{
    public class DefaultContextBuilder : IDefaultContextBuilder
    {
        private readonly IDatabaseSettings _databaseSettings;

        public DefaultContextBuilder(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings ?? throw new ArgumentNullException(nameof(databaseSettings));
        }

        /// <summary>
        /// Creates a DbContext of the specified type with the connection string from settings.
        /// </summary>
        /// <typeparam name="T">Type of the DbContext.</typeparam>
        /// <returns>The created DbContext.</returns>
        public T CreateContext<T>() where T : DbContext
        {
            var connectionString = _databaseSettings.DatabaseConnection;

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connectionString);

            DbContextOptions<T> options = optionsBuilder.Options;
            T context = (T)Activator.CreateInstance(typeof(T), new object[] { options });

            return context;
        }
    }
}