namespace Actio.Services.Identity.Domain.Models
{
    using System;
    using Actio.Common.Exceptions;
    using Actio.Services.Identity.Domain.Services;

    public class User
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected User()
        { }

        public User(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ActioException("empty_user_email", "User email can not be null");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ActioException("empty_user_name", "User name can not be null");
            }

            this.Id = Guid.NewGuid();
            this.Email = email.ToLowerInvariant();
            this.Name = name;
            this.CreatedAt = DateTime.Now;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ActioException("empty_user_password", "User password can not be null");
            }

            this.Salt = encrypter.GetSalt(password);
            this.Password = encrypter.GetHash(password, this.Salt);
        }

        public bool ValidatePassword(string password, IEncrypter encrypter)
            => this.Password.Equals(encrypter.GetHash(password, this.Salt));
    }
}