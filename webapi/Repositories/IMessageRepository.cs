namespace webapi.Repositories
{
    interface IMessageRepository
    {
        void Send(string message);
        int Count(string queueName);
    }
}
