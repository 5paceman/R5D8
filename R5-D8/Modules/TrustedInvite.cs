using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_D8.Modules
{
    class TrustedInvite : IModule
    {
        public TrustedInvite(DiscordClient discordClient) : base(discordClient)
        {
            discordClient.GetService<CommandService>().CreateGroup("tinvite", x =>
            {
                x.CreateCommand("add").Description("Adds a trusted user").Parameter("name", ParameterType.Required);
            });
        }
    }
}
