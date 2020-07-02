using System;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace TradeSaber.UI
{
    [HotReload]
    public class HomeView : BSMLAutomaticViewController
    {
        [UIParams]
        public BSMLParserParams parserParams;

        private Action _loginPressed;
        private Action _viewCardsPressed;

        [UIAction("login")]
        public void Login() => _loginPressed?.Invoke();

        [UIAction("view-cards")]
        public void ViewCards() => _viewCardsPressed?.Invoke();

        private bool _viewCardsInteractable = false;
        [UIValue("view-cards-interactable")]
        public bool ViewCardsInteractable
        {
            get => _viewCardsInteractable;
            set
            {
                _viewCardsInteractable = value;
                NotifyPropertyChanged();
            }
        }

        private string _modalText = "";
        [UIValue("modal-text")]
        public string ModalText
        {
            get => _modalText;
            set
            {
                _modalText = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showLoading = true;
        [UIValue("show-loading")]
        public bool ShowLoading
        {
            get => _showLoading;
            set
            {
                _showLoading = value;
                NotifyPropertyChanged();
            }
        }

        private bool _showContent = false;
        [UIValue("show-content")]
        public bool ShowContent
        {
            get => _showContent;
            set
            {
                _showContent = value;
                NotifyPropertyChanged();
            }
        }

        public void Setup(Action loginAction, Action viewCardsAction)
        {
            _loginPressed = loginAction;
            _viewCardsPressed = viewCardsAction;
        }

        public void SetModal(bool show, bool showLoading = true, bool showText = false, string modalText = "")
        {
            parserParams.EmitEvent(show ? "show-modal" : "hide-modal");
            if (show)
            {
                ModalText = modalText;
                ShowContent = showText;
                ShowLoading = showLoading;
            }
        }
    }
}