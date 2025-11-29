using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedacaoAPI.DTO_s;
using RedacaoAPI.Interfaces;
using RedacaoAPI.Services;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] PlanCreateDTO planDTO)
        {
            try
            {
                _planService.CreatePlan(planDTO);
                return Ok("Plano cadastrado com sucesso.");
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPlanById(int id)
        {
            try
            {
                var plan = _planService.GetPlanById(id);
                return Ok(plan);
            }
            catch (Exception exception)
            {
                return NotFound(new { message = exception.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlan(int id)
        {
            try
            {
                _planService.DeletePlan(id);
                return Ok(new { message = "Plano excluído com sucesso." });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] PlanUpdateDTO updatedPlan)
        {
            try
            {
                var plan = _planService.UpdatePlan(id, updatedPlan);
                return Ok(plan);
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }
    }
}
