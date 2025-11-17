namespace Core.Messages
{
    public class MessageQueues
    {
        public Guid StoreID { get; private set; }

        public string StoreQueue { get; private set; }

        public string RoutingKey { get; private set; }

        public string CentralQueue { get; private set; }

        public MessageQueues(Guid storeID, string centralQueue)
        {
            StoreID = storeID;
            CentralQueue = centralQueue;
            StoreQueue = $"store-{storeID}";
            RoutingKey = $"{storeID}.product";
        }
    }
}
