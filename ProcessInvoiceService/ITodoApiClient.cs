using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessInvoiceService
{
    public interface ITodoApiClient
    {
        Task<List<ToDo>> GetTodosAsync();
    }
}