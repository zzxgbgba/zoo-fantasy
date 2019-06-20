using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZooFantasy.CardData;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZooFantasy.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DeckChoosePage : Page
    {
        private List<Deck> deckList;

        public DeckChoosePage()
        {
            this.InitializeComponent();
            deckList = new List<Deck>();
            DeckLoad();
        }
        private async void DeckLoad()
        {
            deckList.Clear();
            await DeckManager.LoadDecks(deckList);
            for (int i = 0; i < deckList.Count; i++)
            {
                DeckManager.AddDeckToGridView(DeckGridView, deckList[i]);
            }
        }

        private void DeckListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChooseDeckTip.Visibility = Visibility.Collapsed;
        }
        private void DeckCardListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var minionCardDetail = (MinionCard)e.ClickedItem;
            ShowCardDetail(minionCardDetail);
        }
        private void ChangeVisibilityByCardDetail()
        {

            if (CardDetailRelativePanel.Visibility == Visibility.Visible)
            {
                CardDetailRelativePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                CardDetailRelativePanel.Visibility = Visibility.Visible;
            }

        }

        private void ShowCardDetail(MinionCard minionCardDetail)
        {
            if (CardDetailRelativePanel.Visibility == Visibility.Collapsed)
                ChangeVisibilityByCardDetail();
            DetailMinionCardAttack.Text = minionCardDetail.Attack.ToString();
            DetailMinionCardCost.Text = minionCardDetail.Cost.ToString();
            DetailMinionCardHealth.Text = minionCardDetail.Health.ToString();
            DetailMinionCardName.Text = minionCardDetail.Name;
            DetailMinionCardImage.Source = new BitmapImage(new Uri(minionCardDetail.ImagePath));
        }
        private void CancelDetailButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByCardDetail();
        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckGridView.SelectedItem != null)
            {
                for (int i = 0; i < deckList.Count; i++)
                {
                    if (deckList[i].Name == (DeckGridView.SelectedItem as Grid).Name)
                    {
                        await DeckManager.CreatCacheDeck(deckList[i]);
                        MainPage.Current.Frame.Navigate(typeof(BattlefieldPage));
                        return;
                    }
                }
            }
        }
        private void CardsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CardsPage));
        }
    }
}
