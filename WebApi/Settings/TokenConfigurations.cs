namespace WebApi.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TokenConfigurations
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Hours { get; set; }
    }
}