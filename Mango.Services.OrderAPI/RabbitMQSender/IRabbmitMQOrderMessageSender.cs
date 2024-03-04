namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public interface IRabbmitMQOrderMessageSender
    {
        void SendMessage(object message,string exchangeName);
    }
}
