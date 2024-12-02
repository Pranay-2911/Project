using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class StateService : IStateService
    {
        private readonly IRepository<State> _stateRepository;
        private readonly IRepository<City> _cityRepository;
        public StateService(IRepository<State> stateRepository, IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
        }
        public State AddState(StateDto stateDto)
        {
            var city = new City() { Name = stateDto.CityName, Satus = true };
            _cityRepository.Add(city);
            var state = new State() { Name = stateDto.StateName};
            state.Cities.Add(city);
            _stateRepository.Add(state);
            return state;
        }
        public List<State> GetAllState()
        {
            return _stateRepository.GetAll().Include(s=>s.Cities).ToList();
        }

        public City AddCity(StateDto stateDto)
        {
            var state = _stateRepository.GetAll().Include(s => s.Cities).Where(s => s.Name == stateDto.StateName).FirstOrDefault();
            var city = new City() { Name = stateDto.CityName, Satus = true };
            _cityRepository.Add(city);
            state.Cities.Add(city);
            _stateRepository.Update(state);
            return city;
        }

        public List<City> GetCities()
        {
            var CityList = _cityRepository.GetAll().ToList();
            return CityList;
        }
    }
}
