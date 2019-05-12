using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceProcessingService
{
    public interface ITodoApiClient : IDisposable
    {
        Task<List<ToDo>> GetTodosAsync();
    }
}