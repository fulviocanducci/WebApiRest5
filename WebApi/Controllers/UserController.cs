using Canducci.GeneratePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [Authorize()]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DataService Context { get; }

        /// <summary>
        /// 
        /// </summary>
        public BCrypt Crypt { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="crypt"></param>
        public UserController(DataService context, BCrypt crypt)
        {
            Context = context;
            Crypt = crypt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await Context.User.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await Context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (ModelState.IsValid)
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }
                var data = Context.User.Find(id);
                if (data != null && user != null)
                {
                    data.Email = user.Email;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        var hash = Crypt.Hash(user.Password);
                        data.Password = hash.Hashed;
                        data.Salt = hash.Salt;
                    }
                    try
                    {
                        await Context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                var hash = Crypt.Hash(user.Password);
                user.Password = hash.Hashed;
                user.Salt = hash.Salt;
                Context.User.Add(user);
                await Context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await Context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            Context.User.Remove(user);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UserExists(int id)
        {
            return Context.User.Any(e => e.Id == id);
        }
    }
}
