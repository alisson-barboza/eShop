using eShop.Core.Messages;
using eShop.Core.Messages.CommomMessages.Notifications;
using System.Threading.Tasks;

namespace eShop.Core.Communication.Mediator
{
    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> SendCommand<T>(T command) where T : Command;

        Task PublishNotification<T>(T notification) where T : DomainNotification;
    }
}