namespace ProcessInvoiceService
{
    public class InvoiceService
    {
        private readonly ITodoApiClient _apiClient;
        private readonly IInvoiceProcessor _invoiceProcessor;

        public InvoiceService(ITodoApiClient apiClient, IInvoiceProcessor invoiceProcessor)
        {
            _apiClient = apiClient;
            _invoiceProcessor = invoiceProcessor;
        }

        public void OnStart()
        {

        }

        public void OnStop()
        {

        }
    }
}