using System;

namespace webapi.Repositories
{
    class MessageRepository : IMessageRepository
    {
        int IMessageRepository.Count(string queueName)
        {
            throw new NotImplementedException();
        }
        void IMessageRepository.Send(string message)
        {
            throw new NotImplementedException();
        }
    }
}
