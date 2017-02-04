using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace R5_D8.Modules
{
    class TrustedInvite : IModule
    {

        private Dictionary<ulong, ulong> trusteeList = new Dictionary<ulong, ulong>();
        private string ownerID = ConfigurationSettings.AppSettings["adminIds"];

        public TrustedInvite(DiscordClient discordClient) : base(discordClient)
        {
            discordClient.GetService<CommandService>().CreateGroup("tinvite", x =>
            {
                x.CreateCommand("add").Description("Adds a trusted user").Parameter("id", ParameterType.Required).Do(AddTrustee);
                x.CreateCommand("remove").Description("Removes a trusted user").Parameter("id", ParameterType.Required).Do(RemoveTrustee);
                x.CreateCommand("join").Description("Invites a trusted user").Do(InviteTrustee);
            });
        }

        async Task AddTrustee(CommandEventArgs e)
        {
            Console.WriteLine("TRUST ADD: {0} | {1}", e.User.Id, ownerID);
            if (e.User.Id.ToString().Equals(ownerID))
            {
                trusteeList.Add(ulong.Parse(e.Args[0]), e.Server.Id);
                await e.Channel.SendMessage(string.Format("Added {0} to trusted list.", e.Server.GetUser(e.User.Id)));

            } else
            {
                await e.Channel.SendMessage("You're not authorised for this.");
            }
        }

        async Task RemoveTrustee(CommandEventArgs e)
        {
            if (e.User.Id.ToString().Equals(ownerID))
            {
                trusteeList.Remove(ulong.Parse(e.Args[0]));
                await e.Channel.SendMessage(string.Format("Removed {0} to trusted list.", e.Server.GetUser(e.User.Id)));
            }
            else
            {
                await e.Channel.SendMessage("You're not authorised for this.");
            }
        }

        async Task InviteTrustee(CommandEventArgs e)
        {
            if(trusteeList.ContainsKey(e.User.Id))
            {
                Console.WriteLine("User is trusted.");
                ulong userId, serverId;
                userId = e.User.Id;
                trusteeList.TryGetValue(userId, out serverId);

                Invite invite = await theClient.GetServer(serverId).CreateInvite(1800, 1, false, true);
                await e.Channel.SendMessage(invite.Url);
            }
        }

    }
}
