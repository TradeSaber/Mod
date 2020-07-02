using Discord;
using System;

namespace TradeSaber
{
    public class DiscordPinger
    {
        internal bool allowPing = false;
        private readonly Discord.Discord _instance;
        
        public DiscordPinger()
        {
            try
            {
                _instance = new Discord.Discord(715912345800409138, (ulong)CreateFlags.Default);
            }
            catch
            {
                
            }
        }

        public void RequestLogin(Action<bool, string> finished)
        {
            if (_instance == null)
            {
                finished?.Invoke(false, "Error: Could not find Discord");
            }
            try
            {
                var appManager = _instance.GetApplicationManager();
                if (appManager == null)
                {
                    finished?.Invoke(false, $"An error has occured: Application Manager not found.\nPlease report to Auros#0001 on Discord");
                    return;
                }
                appManager.GetOAuth2Token((Result result, ref OAuth2Token token) =>
                {
                    switch (result)
                    {
                        case Result.Ok:
                            finished?.Invoke(true, token.AccessToken);
                            break;
                        case Result.NotRunning:
                            finished?.Invoke(false, "Discord is not running! Please launch discord.");
                            break;
                        case Result.NotInstalled:
                            finished?.Invoke(false, "Discord is not installed! Please install Discord in order to use TradeSaber");
                            break;
                        default:
                            finished?.Invoke(false, $"An error has occured: {result}.\nPlease report to Auros#0001 on Discord");
                            break;
                    }
                });
            }
            catch
            {
                finished?.Invoke(false, "Error: Could not find Discord");
            }
        }
        
        public void Ping()
        {
            if (allowPing)
            {
                _instance?.RunCallbacks();
            }
        }
    }
}
