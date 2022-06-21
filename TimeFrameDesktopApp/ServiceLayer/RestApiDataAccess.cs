using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TimeFrameDesktopApp.DTOs;

namespace TimeFrameDesktopApp.ServiceLayer
{
    class RestApiDataAccess : ITimeFrameService
    {
        public string _baseUri { get; private set; }
        private RestClient _restClient { get; set; }

        public RestApiDataAccess(string baseUri)
        {
            _baseUri = baseUri;
            _restClient = new RestClient(baseUri);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var request = new RestRequest("Users/all", Method.GET);
            var response = await _restClient.ExecuteAsync<IEnumerable<UserDto>>(request);
            return response.Data;
        }

        public async Task<bool> RegisterUserAsync(RegisterDto user)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            RestRequest AddUser = new RestRequest("/users", Method.POST, DataFormat.Json);
            AddUser.AddJsonBody(jsonSerializer.Serialize(user));
            var response = await _restClient.ExecuteAsync<bool>(AddUser);
            return response.Data;
        }

        public IEnumerable<TeamDto> GetAllTeams()
        {
            var request = new RestRequest("Teams", Method.GET);
            return _restClient.Execute<IEnumerable<TeamDto>>(request).Data;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var request = new RestRequest($"Users/{id}", Method.DELETE);
            
            var response = await _restClient.ExecuteAsync<int>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            throw new Exception($"Error deleting user with id = {id}. Message was {response.Content}");
        }
    }
}
