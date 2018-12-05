using Actio.Common.Exceptions;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEncrypter encrypter;

        public UserService(IUserRepository userRepository, IEncrypter encrypter)
        {
            this.userRepository = userRepository;
            this.encrypter = encrypter;
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await this.userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ActioException("email_in_use", $"Email: '{email}' already in use");
            }

            user = new User(email, name);
            user.SetPassword(password, this.encrypter);
            await this.userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await this.userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioException("invalid_credentials", "Invalid credentials");
            }
            if (!user.ValidatePassword(password, this.encrypter))
            {
                throw new ActioException("invalid_credentials", "Invalid credentials");
            }
        }
    }
}