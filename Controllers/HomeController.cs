using Microsoft.AspNetCore.Mvc;

namespace QBankApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Bem-vindo à API QBank! Consulte a documentação para saber mais.",
                Endpoints = new
                {
                    Register = "/api/auth/register",
                    Login = "/api/auth/login"
                }
            });
        }
    }
}
