using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Api.Hubs
{
    public class MyHub : Hub
    {
        private readonly AppDbContext _appDbContext;

        public MyHub(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private static List<string> Names = new List<string>();

        private static int ClientCount { get; set; }

        public static int MaxTeamCount { get; set; } = 7;

        public async Task SendMessage(string name)
        {
            if (Names.Count >= MaxTeamCount)
                await Clients.Caller.SendAsync("ErrorTeamCount", $"Takım sayısı {MaxTeamCount}'den fazla olamaz.");
            else
            {
                Names.Add(name);

                await Clients.All.SendAsync("RecieveMessage", name);
            }

        }

        public async Task GetMessage()
        {
            await Clients.All.SendAsync("RecieveNames", Names);
        }

        public async Task SendProduct(Product product)
        {
            await Clients.All.SendAsync("RecieveProduct", product);
        }

        #region " GroupsOperation "

        public async Task AddGroupAsync(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task RemoveGroupAsync(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _appDbContext.Teams.FirstOrDefault(x => x.Name == teamName);

            if (team != null)
            {
                team.Users.Add(new User() { Name = name });
            }
            else
            {
                var newTeam = new Team() { Name = name };
                newTeam.Users.Add(new User() { Name = name });
            }
            await _appDbContext.SaveChangesAsync();
            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", name, team.Id);
        }

        public async Task GetNamesByGroup()
        {
            var teams = _appDbContext.Teams.Include(x => x.Users).Select(x => new
            {
                teamId = x.Id,
                Users = x.Users.ToList()
            }).ToList();

            await Clients.All.SendAsync("ReceiveNamesByGroup", teams);
        }


        #endregion

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            ClientCount++;

            Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;

            Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
