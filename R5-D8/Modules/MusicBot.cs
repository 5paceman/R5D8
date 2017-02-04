using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace R5_D8.Modules
{
    class MusicBot : IModule
    {

        private IAudioClient audioClient;
        private bool shouldStop = false;

        public MusicBot(DiscordClient discordClient) : base(discordClient)
        {
            theClient.GetService<CommandService>().CreateGroup("music", x =>
            {
                x.CreateCommand("play").Parameter("url", ParameterType.Required).Description("Plays music").Do(PlayMusic);
                x.CreateCommand("stop").Description("Plays music").Do(StopMusic);
            });
        }

        async Task PlayMusic(CommandEventArgs e)
        {
            Channel foundChannel = FindUserInVoiceChannel(e.User, e.Server);
            if (foundChannel != null)
            {
                Console.WriteLine("Found user in {0}", foundChannel.Name);
                audioClient = await theClient.GetService<AudioService>().Join(foundChannel);
                await e.Channel.SendMessage("Playing...");
                PlayYouTube(audioClient, e.Args[0]);
            }
            else
            {
                await e.Channel.SendMessage("Join a Voice Channel first.");
            }
        }

        private void PlayYouTube(IAudioClient audioClient, string youtubeURL)
        {
            IEnumerable<VideoInfo> downloadUrls = DownloadUrlResolver.GetDownloadUrls(youtubeURL);
            VideoInfo targetVideoInfo = downloadUrls.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);

            if (targetVideoInfo.RequiresDecryption)
                DownloadUrlResolver.DecryptDownloadUrl(targetVideoInfo);
            Console.WriteLine("Found download url {0}", targetVideoInfo.DownloadUrl);

            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {targetVideoInfo.DownloadUrl} -f s16le -ar 48000 -ac 2 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
            Console.WriteLine("Started ffmpeg");
            Thread.Sleep(5000);

            int blockSize = 3840;
            byte[] buffer = new byte[blockSize];
            int byteCount;


            while (!shouldStop)
            {
                byteCount = process.StandardOutput.BaseStream.Read(buffer, 0, blockSize);

                if (byteCount == 0)
                    break;

                audioClient.Send(buffer, 0, byteCount);
            }
            audioClient.Wait();

            if (shouldStop)
            {
                process.Kill();
                shouldStop = false;
                audioClient.Disconnect();
            }
        }

        async Task StopMusic(CommandEventArgs e)
        {
            shouldStop = true;
            await e.Channel.SendMessage("Stopping Music.");
        }

        private Channel FindUserInVoiceChannel(User user, Server server)
        {
            foreach (Channel channel in server.VoiceChannels)
            {
                foreach (User channelUser in channel.Users)
                {
                    if (channelUser.Id == user.Id)
                        return channel;
                }
            }

            return null;
        }

    }
}
