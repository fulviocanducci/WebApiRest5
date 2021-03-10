using System;

namespace WebApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TokenResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Expires { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Created { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Status { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="created"></param>
        /// <param name="expires"></param>
        /// <param name="status"></param>
        public TokenResult(string token, DateTime created, DateTime expires, bool status)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            Created = created;
            Expires = expires;
            Status = status;
        }
    }
}