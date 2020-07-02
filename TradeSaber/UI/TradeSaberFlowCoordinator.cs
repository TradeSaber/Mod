using HMUI;
using System.Linq;
using Newtonsoft.Json;
using TradeSaber.Models;
using BeatSaberMarkupLanguage;
using System.Collections.Generic;

namespace TradeSaber.UI
{
    public class TradeSaberFlowCoordinator : FlowCoordinator
    {
        private HomeView _homeView;
        private DiscordPinger _pinger;
        private CardPageView _cardPageView;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "TradeSaber";
                showBackButton = true;
                _homeView = BeatSaberUI.CreateViewController<HomeView>();
                _homeView.Setup(Login, DisplayOwnedCards);
            }
            ProvideInitialViewControllers(_homeView);
        }

        private void Login()
        {
            Plugin.Log.Info("Login Requested");
            _homeView.SetModal(true, true, true, "Check Discord");
            _pinger = new DiscordPinger
            {
                allowPing = true
            };
            _pinger.Ping();
            _pinger.RequestLogin(LoginResults);
        }

        private async void DisplayOwnedCards()
        {
            List<CardHost> cards = new List<CardHost>();
            var response = await Plugin.Client.GetAsync(Config.APIURL + "/users/self");
            if (response.IsSuccessStatusCode)
            {
                User currentUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                foreach (var cardItem in currentUser.Inventory)
                {
                    if (!Plugin.CachedCards.Any(c => c.Id == cardItem))
                    {
                        var response2 = await Plugin.Client.GetAsync(Config.APIURL + "/cards/" + cardItem);
                        if (response2.IsSuccessStatusCode)
                        {
                            var content = await response2.Content.ReadAsStringAsync();
                            Card card = JsonConvert.DeserializeObject<Card>(content);
                            Plugin.CachedCards.Add(card);
                            cards.Add(new CardHost(card, null));
                        }
                    }
                    else
                    {
                        cards.Add(new CardHost(Plugin.CachedCards.First(c => c.Id == cardItem), null));
                    }
                }
                _cardPageView = BeatSaberUI.CreateViewController<CardPageView>();
                _cardPageView.cards.AddRange(cards.Select(c => c));
                ReplaceTopViewController(_cardPageView, null, false, ViewController.SlideAnimationDirection.Up);
                ProvideInitialViewControllers(_cardPageView);
            }
            else
            {
                Plugin.Log.Info("oh no");
            }

            
        }

        private async void LoginResults(bool success, string result)
        {
            if (success)
            {
                User user = await Plugin.Instance.Login(result);
                if (user != null)
                {
                    _homeView.SetModal(true, false, true, "Login Successful");
                    _homeView.ViewCardsInteractable = true;
                    return;
                }
                result = "Could not connect to TradeSaber API";
            }
            _homeView.SetModal(true, false, true, result);
            _pinger.allowPing = false;
            _pinger = null;
        }

        private void Update()
        {
            if (_pinger != null)
            {
                _pinger.Ping();
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
        }
    }
}