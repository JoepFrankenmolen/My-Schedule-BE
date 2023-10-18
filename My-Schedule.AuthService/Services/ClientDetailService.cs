using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.ClientDetail;

namespace My_Schedule.AuthService.Services
{
    public class ClientDetailService
    {
        private readonly AuthServiceContext _dbContext;

        public ClientDetailService(AuthServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Guid> AddOrCreateClientDetails(string iPAddress, string userAgent)
        {
            var clientDetails = CreateNewClientDetails(iPAddress, userAgent);

            var existingClientDetails = await GetExistingClientDetailsAsync(clientDetails);

            if (existingClientDetails != null)
            {
                return existingClientDetails.Id;
            }
            else
            {
                await SaveNewClientDetailsAsync(clientDetails);

                return clientDetails.Id;
            }
        }

        private async Task<ClientDetails> GetExistingClientDetailsAsync(ClientDetails clientDetails)
        {
            return await _dbContext.ClientDetails
                .FirstOrDefaultAsync(cd => cd.IPAddress == clientDetails.IPAddress && cd.UserAgent == clientDetails.UserAgent);
        }

        private ClientDetails CreateNewClientDetails(string iPAddress, string userAgent)
        {
            return new ClientDetails
            {
                IPAddress = iPAddress,
                UserAgent = userAgent
            };
        }

        private async Task SaveNewClientDetailsAsync(ClientDetails newClientDetails)
        {
            _dbContext.ClientDetails.Add(newClientDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}