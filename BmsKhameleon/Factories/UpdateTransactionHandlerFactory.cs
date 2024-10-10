namespace BmsKhameleon.UI.Factories
{
    public class UpdateTransactionHandlerFactory(IEnumerable<IUpdateTransactionHandler> handlers)
    {
        public IUpdateTransactionHandler GetHandler(string transactionType, string transactionMedium)
        {
            return handlers.FirstOrDefault(h => h.CanHandle(transactionType, transactionMedium))
                   ?? throw new InvalidOperationException("No handler found for the transaction type and medium.");
        }
    }
}
