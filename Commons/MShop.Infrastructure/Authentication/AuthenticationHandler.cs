using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MShop.Infrastructure.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SecurityKey _issuerSignInKey;
        private readonly SigningCredentials _credentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationHandler(IConfiguration configuration)
        {
            _options = new JwtOptions();
            configuration.GetSection("jwt").Bind(_options);
            _issuerSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _credentials = new SigningCredentials(_issuerSignInKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_credentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _options.Issuer,
                IssuerSigningKey = _issuerSignInKey
            };
        }

        public JwtAuthToken Create(string userId)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var payload = new JwtPayload
            {
                { "sub", userId },
                { "iss", _options.Issuer },
                { "iat", now },
                { "exp", exp },
                { "unique_name", userId }
            };

            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            var jsonToken = new JwtAuthToken
            {
                Token = token,
                Expires = exp
            };

            return jsonToken;
        }
    }
}
