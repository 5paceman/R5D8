using Discord;
using System;
using System.Configuration;

namespace R5_D8
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();
         
        private DiscordClient theClient;

        public void Start()
        {
            theClient = new DiscordClient();

            theClient.ExecuteAndWait(async () =>
            {
                await theClient.Connect(ConfigurationSettings.AppSettings["botToken"], TokenType.Bot);
            });
        }
    }
}
