using Fastdo.API.Mappings;
using Fastdo.Core;
using Fastdo.Core.Hubs;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fastdo.API.Hubs
{
    [Authorize]
    public class TechSupportMessagingHub:Hub<ITechSupportMessageHub>
    {
        private static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();
        private readonly ITechSupportMessageService supportMessageService;

        public TechSupportMessagingHub(ITechSupportMessageService supportMessageService)
        {
            this.supportMessageService = supportMessageService;
        }

        public override Task OnConnectedAsync()
        {
            //var c = Context.User;
            //string userId = GetUserId();
       
            //if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            //{
            //    Connections.Add(userId, Context.ConnectionId);
            //}
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            //string userId = GetUserId();
            //Connections.Remove(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
       
        private string GetUserId()
        {
            return Context.User.Claims.FirstOrDefault(c=>c.Type== Variables.UserClaimsTypes.UserId).Value;
        }

        public static IReadOnlyList<string> GetUserConnections(string USerId)
        {
            return Connections.GetConnections(USerId).ToArray();
        }
        public static IReadOnlyList<string> GetUserConnections(IEnumerable<string> userIds)
        {
            return Connections.GetConnections(userIds).ToArray();
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public  async Task onCustomerSendMessage(string question)
        {
            if (!string.IsNullOrEmpty(question))
            {
              await  supportMessageService.OnCustomerAddNewQuestionMessage(new SendTechSupportViewModel(question), Context.User);
            }
        }
    }
}
