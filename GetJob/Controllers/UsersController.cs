using GetJob.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GetJob.Controllers
{

    [Route("api/[conroller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetAllAsync();
            return Ok(user);
            

        }
    }
}
