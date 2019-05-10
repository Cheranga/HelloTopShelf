using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeService
{
    public interface ITodoApiClient
    {
        Task<List<ToDo>> GetTodosAsync();
    }
}