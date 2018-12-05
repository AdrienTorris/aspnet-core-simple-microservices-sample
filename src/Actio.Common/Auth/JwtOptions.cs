namespace Actio.Common.Auth
{
    using System;

    public class JwtOptions
    {
        public string SecretKey{get;set;}
        public int ExpiryMinutes {get;set;}
        public string Issuer{get;set;}
    }
}