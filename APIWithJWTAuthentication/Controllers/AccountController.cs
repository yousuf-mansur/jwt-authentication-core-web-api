using APIWithJWTAuthentication.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using static SharedLibrary.DTOs.ServiceResponse;

namespace APIWithJWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new GeneralResponse(false, "Invalid input"));

            var response = await userAccount.CreateAccount(userDTO);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccount.LoginAccount(loginDTO);
            return Ok(response);
        }
    }
}
