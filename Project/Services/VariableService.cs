using Project.Models;
using Project.Repositories;
using Serilog;

namespace Project.Services
{
    public class VariableService : IVariableService
    {
        private readonly IRepository<GlobalVariables> _globalRepository;

        public VariableService(IRepository<GlobalVariables> globalRepository)
        {
            _globalRepository = globalRepository;
        }

        public bool UpdateGlobal(GlobalVariables globalVariables)
        {
            var variable = _globalRepository.GetAll().FirstOrDefault();
            if (variable != null)
            {
                variable.PolicyCancellationPenalty = globalVariables.PolicyCancellationPenalty;
                variable.CommissionWithdrawDeduction = globalVariables.CommissionWithdrawDeduction;
                _globalRepository.Update(variable);
                Log.Information("globalvariable record updated: " + variable.Id);

                return true;
            }
            return false;
        }

        public GlobalVariables Get()
        {
            return _globalRepository.GetAll().FirstOrDefault();
        }
        public void AddGlobal(GlobalVariables globalVariables)
        {
            _globalRepository.Add(globalVariables);
            Log.Information("globalvariable record updated: " + globalVariables.Id);

        }


    }
}
