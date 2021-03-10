using System.Text;

namespace WebApi.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public static class Secret
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Value = "fedaf7d8863b48e197b9287d492b708e";

        /// <summary>
        /// 
        /// </summary>
        public static byte[] ValueToByte => Encoding.ASCII.GetBytes(Value);
    }
}