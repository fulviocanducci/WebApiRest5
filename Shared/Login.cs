using System.ComponentModel.DataAnnotations;

namespace Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 
        /// </summary>
        [Required()]
        [EmailAddress()]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required()]
        [MinLength(4)]
        public string Password { get; set; }
    }
}