using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Serilog;

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
            var existingState = _stateRepository.GetAll().Where(s=>s.Name == stateDto.StateName).FirstOrDefault();
            var existingCity = _cityRepository.GetAll().Where(c=>c.Name == stateDto.CityName).FirstOrDefault();
            if (existingState == null && existingCity == null) 
            {
                var city = new City() { Name = stateDto.CityName, Satus = true };
                _cityRepository.Add(city);
                var state = new State() { Name = stateDto.StateName };
                state.Cities.Add(city);
                _stateRepository.Add(state);
                Log.Information("state record added: " + state.Id);

                return state;
            }
            throw new Exception("State is already exist");
                
        }

        public List<State> GetAllState()
        {
            return _stateRepository.GetAll().Include(s=>s.Cities).ToList();
        }

        public City AddCity(StateDto stateDto)
        {
            var state = _stateRepository.GetAll().Include(s => s.Cities).Where(s => s.Name == stateDto.StateName).FirstOrDefault();
            var existingCity = _cityRepository.GetAll().Where(c=>c.Name==stateDto.CityName).FirstOrDefault();

            if (existingCity == null)
            {
                var city = new City() { Name = stateDto.CityName, Satus = true };
                _cityRepository.Add(city);
                state.Cities.Add(city);
                _stateRepository.Update(state);
                Log.Information("city record added: " + city.Id);
                Log.Information("state record updated: " + state.Id);

                return city;
            }
            throw new Exception("City is already exist");
        }

        public List<City> GetCities()
        {
            var CityList = _cityRepository.GetAll().ToList();
            return CityList;
        }
    }
}
