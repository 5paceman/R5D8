using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_D8.Modules
{
    abstract class IModule
    {
        protected DiscordClient theClient;
        public IModule(DiscordClient discordClient)
        {
            this.theClient = discordClient;
        }
    }
}
