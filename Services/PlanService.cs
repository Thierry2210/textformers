using RedacaoAPI.DTO_s;
using RedacaoAPI.DTOs;
using RedacaoAPI.Interfaces;
using RedacaoAPI.Models;
namespace RedacaoAPI.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _repository;

        public PlanService(IPlanRepository planRepository)
        {
            _repository = planRepository;
        }

        public IEnumerable<Plan> GetAllPlans()
        {
            return _repository.GetAll();
        }

        public Plan CreatePlan(PlanCreateDTO planDTO)
        {
            List<string> errors = ValidatorService.ValidateCreatePlan(planDTO);
            if (errors.Count > 0)
                throw new Exception(string.Join(" ", errors));

            Plan plan = new Plan
            {
                Name = planDTO.Name,
                Price = planDTO.Price,
                MaxCorrections = planDTO.MaxCorrections
            };

            _repository.Create(plan);
            return plan;
        }
        public Plan GetPlanById(int id)
        {
            Plan? plan = _repository.GetById(id);
            if (plan == null)
            {
                throw new Exception("ID de plano inválido ou inexistente");
            }

            return plan;
        }

        public void DeletePlan(int id)
        {
            Plan? plan = GetPlanById(id);

            plan.DeletedAt = DateTime.UtcNow.AddHours(-3);
            _repository.SoftDelete(id);
        }

        public Plan UpdatePlan(int id, PlanUpdateDTO updatedPlan)
        {
            Plan? plan = GetPlanById(id);

            List<string> errors = ValidatorService.ValidateUpdatePlan(updatedPlan);
            if (errors.Count > 0)
                throw new Exception(string.Join(" ", errors));

            plan.Name = updatedPlan.Name ?? plan.Name;
            plan.Price = updatedPlan.Price ?? plan.Price;
            plan.MaxCorrections = updatedPlan.MaxCorrections ?? plan.MaxCorrections;

            _repository.Update(plan);
            return plan;
        }
    }
}
