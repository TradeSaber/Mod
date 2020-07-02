using IPA;
using System.Text;
using TradeSaber.UI;
using Newtonsoft.Json;
using System.Net.Http;
using TradeSaber.Models;
using BS_Utils.Utilities;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using System.Collections.Generic;
using IPALogger = IPA.Logging.Logger;
using BeatSaberMarkupLanguage.MenuButtons;
using System.Net.Http.Headers;

namespace TradeSaber
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static string Token { get; set; }
        internal static IPALogger Log { get; set; }
        internal static Plugin Instance { get; private set; }
        internal static HttpClient Client { get; private set; }
        internal static List<Card> CachedCards { get; set; }
        
        private MenuButton _tradeSaberMenuButton;
        private TradeSaberFlowCoordinator _tradeSaberFlow;

        [Init]
        public Plugin(IPALogger logger)
        {
            Log = logger;
            Instance = this;
            Client = new HttpClient();
            CachedCards = new List<Card>();
        }

        [OnEnable]
        public void OnEnable()
        {
            _tradeSaberMenuButton = new MenuButton("TradeSaber", StartTradeSaberUI);
            MenuButtons.instance.RegisterButton(_tradeSaberMenuButton);
            BSEvents.lateMenuSceneLoadedFresh += MenuSceneLoaded;
        }

        [OnDisable]
        public void OnDisable()
        {
            BSEvents.lateMenuSceneLoadedFresh -= MenuSceneLoaded;
        }

        private void StartTradeSaberUI()
        {
            if (_tradeSaberFlow == null)
            {
                _tradeSaberFlow = BeatSaberUI.CreateFlowCoordinator<TradeSaberFlowCoordinator>();
            }
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_tradeSaberFlow);
        }

        private void MenuSceneLoaded(ScenesTransitionSetupDataSO _)
        {
            
        }

        internal async Task<User> Login(string accessToken)
        {
            var response = await Client.PostAsync(Config.APIURL + "/authorize/game", new StringContent(JsonConvert.SerializeObject(new GameLoginBody { AccessToken = accessToken }), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserLoginData>(content);
                Token = data.Token;
                var header = new AuthenticationHeaderValue("Bearer", Token);

                Client.DefaultRequestHeaders.Authorization = header;

                return data.User;
            }
            else
            {
                return null;
            }
        }

        private class UserLoginData
        {
            public string Token { get; set; }
            public User User { get; set; }
        }

        private class GameLoginBody
        {
            public string AccessToken { get; set; }
        }
    }
}