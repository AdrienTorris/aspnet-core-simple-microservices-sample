namespace Actio.Common.Auth
{
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;

    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler= new JwtSecurityTokenHandler();
        private readonly JwtOptions options;
        private readonly SecurityKey issuerSigningKey;
        private readonly SigningCredentials signingCredentials;
        private readonly JwtHeader jwtHeader;
        private readonly TokenValidationParameters tokenValidationParameters; 

        public JwtHandler(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
            this.issuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey));
            this.signingCredentials = new SigningCredentials(this.issuerSigningKey, SecurityAlgorithms.HmacSha256);
            this.jwtHeader = new JwtHeader(this.signingCredentials);
            this.tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer=this.options.Issuer,
                IssuerSigningKey=this.issuerSigningKey
            };
        }

        public JsonWebToken Create(Guid userId)
        {
            var nowUtc = DateTime.Now;
            var expires = nowUtc.AddMinutes(this.options.ExpiryMinutes);
            // var centuryBegin = nowUtc.AddYears(100 * -1).ToUniversalTime();
            var centuryBegin = new DateTime(1989,1,1).ToUniversalTime();
            //var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var exp = ((DateTimeOffset)expires).ToUnixTimeSeconds();
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss",options.Issuer},
                {"iat",now},
                {"exp",exp},
                {"unique_name",userId}
            };
            var jwt = new JwtSecurityToken(this.jwtHeader, payload);
            var token = this.jwtSecurityTokenHandler.WriteToken(jwt);

            return new JsonWebToken{
                Token = token,
                Expires = exp
            };
        }
    }
}