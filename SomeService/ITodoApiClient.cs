using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceProcessingService
{
    public interface ITodoApiClient
    {
        Task<List<ToDo>> GetTodosAsync();
    }
}