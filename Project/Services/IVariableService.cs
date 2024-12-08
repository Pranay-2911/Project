using Project.Models;

namespace Project.Services
{
    public interface IVariableService
    {
        public bool UpdateGlobal(GlobalVariables globalVariables);
        public GlobalVariables Get();

        public void AddGlobal(GlobalVariables globalVariables);

    }
}
