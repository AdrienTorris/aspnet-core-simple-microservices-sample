namespace Actio.Services.Identity.Domain.Services
{
    using System;

    public interface IEncrypter
    {
        string GetSalt(string value);
        string GetHash(string value, string salt);
    }
}