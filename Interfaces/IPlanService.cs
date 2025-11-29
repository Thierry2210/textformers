using RedacaoAPI.DTO_s;
using RedacaoAPI.Models;
using System.Collections.Generic;
using static RedacaoAPI.DTO_s.UserDTO;

namespace RedacaoAPI.Interfaces
{
    public interface IPlanService
    {
        public Plan GetPlanById(int id);
        public IEnumerable<Plan> GetAllPlans();
        public Plan CreatePlan(PlanCreateDTO planDTO);
        public Plan UpdatePlan(int id, PlanUpdateDTO updatedPlan);
        public void DeletePlan(int id);
    }
}
