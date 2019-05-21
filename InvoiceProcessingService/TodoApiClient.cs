using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InvoiceProcessingService
{
    public class TodoApiClient : ITodoApiClient
    {
        private readonly HttpClient _client;
        private readonly ToDoApiConfig _apiConfig;

        public TodoApiClient(HttpClient client, ToDoApiConfig apiConfig)
        {
            _client = client;
            _apiConfig = apiConfig;
        }

        public async Task<List<ToDo>> GetTodosAsync()
        {
            var httpResponse = await _client.GetAsync(_apiConfig.Url);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot get todo items from the web API");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                throw new Exception("No valid todo items retrieved from the web API");
            }

            var todos = JsonConvert.DeserializeObject<List<ToDo>>(content);

            return todos;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}