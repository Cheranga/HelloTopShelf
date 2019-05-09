using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessInvoiceService
{
    public interface IInvoiceProcessor : IDisposable
    {
        Task<List<ToDo>> UpsertTodosAsync(params ToDo[] items);
    }
}