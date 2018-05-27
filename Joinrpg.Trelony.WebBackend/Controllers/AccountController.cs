using System.Threading.Tasks;
using Joinrpg.Trelony.WebBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Joinrpg.Trelony.WebBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpPost]
        public Task Login(LoginViewModel viewModel)
        {
            return Task.CompletedTask;
        }

        [HttpPost]
        public Task Logout() => Task.CompletedTask;
    }
}