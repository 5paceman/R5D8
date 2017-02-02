using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_D8.Modules
{
    class TrustedInvite : IModule
    {

        private List<string> trusteeList = new List<string>();
        private string ownerID = ConfigurationSettings.AppSettings["ownerId"];
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
            if(e.User.Id.Equals(ownerID))
            {
                trusteeList
            }
        }

        async Task RemoveTrustee(CommandEventArgs e)
        {

        }

        async Task InviteTrustee(CommandEventArgs e)
        {

        }
    }
}
