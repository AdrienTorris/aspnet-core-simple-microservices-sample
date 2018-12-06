using Actio.Common.Auth;
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
        private readonly IJwtHandler jwtHandler;

        public UserService(IUserRepository userRepository, IEncrypter encrypter
        , IJwtHandler jwtHandler)
        {
            this.userRepository = userRepository;
            this.encrypter = encrypter;
            this.jwtHandler=jwtHandler;
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

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            User user = await this.userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ActioException("invalid_credentials", "Invalid credentials");
            }
            if (!user.ValidatePassword(password, this.encrypter))
            {
                throw new ActioException("invalid_credentials", "Invalid credentials");
            }

            return this.jwtHandler.Create(user.Id);
        }
    }
}