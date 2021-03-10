namespace Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class User : Login
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; } = true;
    }
}