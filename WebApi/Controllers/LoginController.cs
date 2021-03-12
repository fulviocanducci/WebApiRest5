using Canducci.GeneratePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;
using Shared;
using Services;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/login")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class LoginController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenService TokenService { get; }

        /// <summary>
        /// 
        /// </summary>
        public DataService DataService { get; }

        /// <summary>
        /// 
        /// </summary>
        public BCrypt Crypt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="dataService"></param>
        /// <param name="crypt"></param>
        public LoginController(TokenService tokenService, DataService dataService, BCrypt crypt)
        {
            TokenService = tokenService;
            DataService = dataService;
            Crypt = crypt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("auth", Name = "Login - Auth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Auth(Login login)
        {
            CreateUser();
            var user = DataService.User.Where(x => x.Email == login.Email).FirstOrDefault();
            if (user != null)
            {
                if (Crypt.Valid(login.Password, user.Salt, user.Password))
                {
                    return Ok(TokenService.GenerateTokenResult(user));
                }
            }
            return NotFound(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user", Name = "Login - User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [Authorize()]
        public async Task<IActionResult> GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await DataService
                  .User
                  .Where(x => x.Email == User.Identity.Name)
                  .Select(x => new
                  {
                      x.Id,
                      x.Email,
                      x.Name,
                      x.CreatedAt,
                      x.Active
                  })
                  .FirstOrDefaultAsync();
                return Ok(user);
            }
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        [NonAction()]
        private void CreateUser()
        {
            if (!DataService.User.AsNoTrackingWithIdentityResolution().Any())
            {
                BCryptValue hash = Crypt.Hash("123456@@");
                User user = new User
                {
                    Email = "fulviocanducci@hotmail.com",
                    Name = "Fúlvio Cezar Canducci Dias",
                    CreatedAt = System.DateTime.Now,
                    Active = true,
                    Password = hash.Hashed,
                    Salt = hash.Salt
                };
                DataService.User.Add(user);
                DataService.SaveChanges();
                DataService.Entry(user).State = EntityState.Detached;
            }
        }
    }
}