using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_D8.Modules
{
    class TestModule : IModule
    {
        public TestModule(DiscordClient discordClient) : base(discordClient)
        {
            discordClient.GetService<CommandService>().CreateCommand("test")
                                                      .Description("Test command")
                                                      .Do(OnCommand);
        }

        async Task OnCommand(CommandEventArgs e)
        {
            await e.Channel.SendMessage("Test");
        }
    }
}
