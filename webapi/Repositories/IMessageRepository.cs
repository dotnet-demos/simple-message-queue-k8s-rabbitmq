namespace webapi.Repositories
{
    public interface IMessageRepository
    {
        void Send(string message);
        uint Count(string queueName);
    }
}