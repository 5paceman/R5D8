using Discord;
using Discord.Audio;
using Discord.Commands;
using R5_D8.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace R5_D8
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();
         
        private DiscordClient theClient;

        private List<IModule> moduleList;

        public void Start()
        {
            theClient = new DiscordClient();

            SetupModules();

            Console.WriteLine("Starting Client...");
            theClient.ExecuteAndWait(async () =>
            {
                await theClient.Connect(ConfigurationSettings.AppSettings["botToken"], TokenType.Bot);
            });
        }

        private void SetupModules()
        {
            Console.WriteLine("Adding Commands...");

            theClient.UsingCommands(x =>
            {
                x.PrefixChar = '.';
                x.HelpMode = HelpMode.Private;
            });

            Console.WriteLine("Adding Audio...");

            theClient.UsingAudio(x =>
            {
                x.Mode = AudioMode.Both;
            });

            Console.WriteLine("Done.");
            Console.WriteLine("Registering Modules...");

            this.moduleList = new List<IModule>(new IModule[]
            {
                new TrustedInvite(theClient),
                new MusicBot(theClient)
            });

            Console.WriteLine("Done.");
        }
    }
}
