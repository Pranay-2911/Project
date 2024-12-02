using Project.DTOs;
using Project.Models;

namespace Project.Services
{
    public interface IStateService
    {
        public State AddState(StateDto stateDto);
        public List<State> GetAllState();
        public City AddCity(StateDto stateDto);
        public List<City> GetCities();
    }
}
