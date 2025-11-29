using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
using static RedacaoAPI.DTOs.EssayDTO;

namespace RedacaoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EssayController : ControllerBase
    {
        private readonly IEssayService _essayService;

        public EssayController(IEssayService essayService)
        {
            _essayService = essayService;
        }

        private Guid GetUserIdFromToken()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new Exception("Token inválido: usuário não encontrado.");

            return Guid.Parse(claim.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEssayAsync([FromBody] EssayCreateDTO essayDTO)
        {
            try
            {
                Guid userId = GetUserIdFromToken();

                EssayResponseDTO essay = await _essayService.CreateEssayAsync(userId, essayDTO);
                return Ok(essay);
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetEssaysByUser()
        {
            try
            {
                Guid userId = GetUserIdFromToken();

                List<Essay> essays = _essayService.GetEssaysByUser(userId);
                return Ok(essays);
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [Authorize]
        [HttpGet("{essayId}")]
        public IActionResult GetEssayById([FromRoute] Guid essayId)
        {
            try
            {
                Guid userId = GetUserIdFromToken();

                var essay = _essayService.GetEssayById(essayId, userId);
                return Ok(essay);
            }
            catch (Exception exception)
            {
                return NotFound(new { message = exception.Message });
            }
        }

        [Authorize]
        [HttpDelete("{essayId}")]
        public IActionResult SoftDeleteEssay([FromRoute] Guid essayId)
        {
            try
            {
                Guid userId = GetUserIdFromToken();

                _essayService.SoftDelete(essayId, userId);
                return Ok(new { message = "Redação excluída com sucesso." });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }
    }
}
