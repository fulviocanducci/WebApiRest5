using System.ComponentModel.DataAnnotations;

namespace Shared
{
    /// <summary>
    /// </summary>
    public class Todo
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required()]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Done { get; set; } = false;
    }
}