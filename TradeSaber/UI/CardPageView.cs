using System.Linq;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components;
using TradeSaber.Models;

namespace TradeSaber.UI
{
    [HotReload]
    public class CardPageView : BSMLAutomaticViewController
    {
        [UIValue("cards")]
        public List<object> cards = new List<object>();

        [UIComponent("card-list")]
        public CustomCellListTableData cardList;

        [UIAction("#post-parse")]
        private void Parsed()
        {

        }

        public void SetCards(CardHost[] newCards)
        {
            cards.Clear();
            cardList.tableView.ReloadData();
            cardList.data.AddRange(newCards);
            cardList.tableView.ReloadData();
        } 
    }
}