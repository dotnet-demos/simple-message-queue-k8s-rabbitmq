using System.Threading.Tasks;

namespace message_processor
{
    internal interface IMessageProcessor
    {
        internal Task ProcessMessage(string message);
    }
}