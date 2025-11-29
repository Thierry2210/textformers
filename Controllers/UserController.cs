using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using RedacaoAPI.Services;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("auth")]
        public IActionResult Auth(UserLoginDTO user)
        {
            try
            {
                object token = _userService.Authenticate(user);
                return Ok(new { token = token });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        private Guid GetUserIdFromToken()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new Exception("Token inválido: usuário não encontrado.");

            return Guid.Parse(claim.Value);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                Guid userId = GetUserIdFromToken();
                User user = _userService.GetUser(userId);
                return Ok(user);
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }


        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterDTO userDTO)
        {
            try
            {
                _userService.CreateUser(userDTO);
                return Ok("Usuário cadastrado com sucesso.");
            }
            catch(Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserUpdateDTO userDTO)
        {
            try
            {
                Guid userId = GetUserIdFromToken();
                _userService.UpdateUser(userId, userDTO);
                return Ok("Usuário atualizado com sucesso.");
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("/permission")]
        public IActionResult UpdatePermission([FromBody] UpdateUserPermissionDTO userDTO)
        {
            try
            {
                _userService.UpdatePermission(userDTO.Id, userDTO.Permission);
                return Ok("Permissão alterada com sucesso.");
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            try
            {
                Guid userId = GetUserIdFromToken();
                _userService.DeleteUser(userId);
                return Ok("Usuário deletado com sucesso.");
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }
    }
}
