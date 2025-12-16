using EngineAnalyticsWebApp.Shared.Services;
using EngineAnalyticsWebApp.Shared.Services.Factories;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.TestLazy.Messaging
{
    public partial class MessageComponent
    {
        [Parameter]
        public string? Message { get; set; }
        private string? formattedMessage;

        public MessageComponent(IMessageServiceFactory messageServiceFactory)
        {
            this.messageServiceFactory = messageServiceFactory;
        }

        private IMessageServiceFactory messageServiceFactory { get; }

        protected override void OnInitialized()
        {
            IMessageService messageService = messageServiceFactory.Create();
            if(!string.IsNullOrEmpty(Message))
            {
                formattedMessage = messageService.MessageLogger(Message);

            }
        }
    }
}
