namespace Actio.Common.Auth
{
    using System;

    public class JsonWebToken
    {
        public string Token{get;set;}
        public long Expires{get;set;}
    }
}