namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public interface IRabbmitMQCartMessageSender
    {
        void SendMessage(object message,string queueName);
    }
}
