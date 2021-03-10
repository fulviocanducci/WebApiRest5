using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace WebApi.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class SigningConfigurations
    {
        /// <summary>
        /// 
        /// </summary>
        public SecurityKey Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        /// <summary>
        /// 
        /// </summary>
        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}