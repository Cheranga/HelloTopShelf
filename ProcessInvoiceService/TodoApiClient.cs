using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProcessInvoiceService
{
    public class TodoApiClient : ITodoApiClient
    {
        private readonly ToDoApiConfig _apiConfig;
        private readonly HttpClient _client;

        public TodoApiClient(ToDoApiConfig apiConfig, HttpClient client)
        {
            _apiConfig = apiConfig;
            _client = client;
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
    }


}