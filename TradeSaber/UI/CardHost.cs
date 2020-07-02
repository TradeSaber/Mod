using System;
using TradeSaber.Models;
using BeatSaberMarkupLanguage.Notify;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;

namespace TradeSaber.UI
{
    public class CardHost : INotifiableHost
    {
        public virtual Card Card { get; protected set; }
        public virtual Action OnClick { get; protected set; }
        
        public string _imageSource;
        [UIValue("image-source")]
        public virtual string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("card-clicked")]
        public void CardClicked() => OnClick?.Invoke();

        protected CardHost() { }
        public CardHost(Card card, Action clickedCard)
        {
            Card = card;
            OnClick = clickedCard;
            ImageSource = Config.BASEURL + card.CoverURL;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                Plugin.Log.Error($"Error Invoking PropertyChanged: {ex.Message}");
                Plugin.Log.Error(ex);
            }
        }
    }
}
