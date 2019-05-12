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

        public TodoApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ToDo>> GetTodosAsync()
        {
            var httpResponse = await _client.GetAsync(@"https://jsonplaceholder.typicode.com/todos");
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