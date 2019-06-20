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
    public sealed partial class CardsPage : Page
    {
        private List<MinionCard> minionCardList;
        private List<SpellCard> spellCardList;
        private List<MinionCard> deckDetailMinionCardList;
        private Deck deckDetail;
        private Deck creatingDeck;
        private ObservableCollection<Deck> deckList;


        public CardsPage()
        {
            minionCardList = new List<MinionCard>();
            spellCardList = new List<SpellCard>();
            deckList = new ObservableCollection<Deck>();
            deckDetailMinionCardList = new List<MinionCard>();
            creatingDeck = new Deck();
            this.InitializeComponent();
            CardLoad();
            DeckLoad();
        }

        private async void CardLoad()
        {
            minionCardList.Clear();
            spellCardList.Clear();
            Uri uri = new Uri("ms-appx:///Resource/CardData/MinionCardData/List.txt");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            StorageFolder parentFolder = await file.GetParentAsync();
            await CardDataManager.LoadMinionCardsFromFolder(minionCardList, parentFolder);
            CardDataManager.SortCardListByCost(minionCardList);//随从牌的载入和排序
            Uri uri2 = new Uri("ms-appx:///Resource/CardData/SpellCardData/List.txt");
            file = await StorageFile.GetFileFromApplicationUriAsync(uri2);
            parentFolder = await file.GetParentAsync();
            await CardDataManager.LoadSpellCardsFromFolder(spellCardList, parentFolder);
            CardDataManager.SortCardListByCost(spellCardList);//法术牌的载入和排序
            for (int i = 0; i < minionCardList.Count; i++)
            {
                CardDataManager.AddCardToGridView_Middle(MinionCardGridView, minionCardList[i]);
                (MinionCardGridView.Items.Last() as Grid).RightTapped += MinionCardGridViewItem_RightTapped;
            }
            for (int i = 0; i < spellCardList.Count; i++)
            {
                CardDataManager.AddCardToGridView_Middle(SpellCardGridView, spellCardList[i]);
                (SpellCardGridView.Items.Last() as Grid).RightTapped += SpellCardGridViewItem_RightTapped;
            }
            MinionCardGridView.Visibility = Visibility.Visible;
        }
        private async void DeckLoad()
        {
            deckList.Clear();
            await DeckManager.LoadDecks(deckList);
            DeckListView.Visibility = Visibility.Visible;
        }

        private void ChangeVisibilityByNewDeck()
        {
            if (DeckCreatGrid.Visibility == Visibility.Visible)
            {
                DeckCreatGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeckCreatGrid.Visibility = Visibility.Visible;
            }

            if (NewDeckButton.Visibility == Visibility.Visible)
            {
                NewDeckButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewDeckButton.Visibility = Visibility.Visible;
            }

            if (DeckListView.Visibility == Visibility.Visible)
            {
                DeckListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeckListView.Visibility = Visibility.Visible;
            }

            if (NewDeckCardListView.Visibility == Visibility.Visible)
            {
                NewDeckCardListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewDeckCardListView.Visibility = Visibility.Visible;
            }

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
        private void ChangeVisibilityByDeckDetail()
        {
            if (DeckDetailStackPanel.Visibility == Visibility.Visible)
            {
                DeckDetailStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeckDetailStackPanel.Visibility = Visibility.Visible;
            }
            if (DeckDetailListView.Visibility == Visibility.Visible)
            {
                DeckDetailListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeckDetailListView.Visibility = Visibility.Visible;
            }
            if (NewDeckButton.Visibility == Visibility.Visible)
            {
                NewDeckButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewDeckButton.Visibility = Visibility.Visible;
            }

            if (DeckListView.Visibility == Visibility.Visible)
            {
                DeckListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeckListView.Visibility = Visibility.Visible;
            }

        }

        //卡牌详情
        private void MinionCardGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
             ShowCardDetail((e.ClickedItem as Grid).DataContext as MinionCard);
        }
        private void SpellCardGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowCardDetail((e.ClickedItem as Grid).DataContext as SpellCard);
        }


        private void CancelDetailButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByCardDetail();
        }

        //右击卡牌事件
        private void MinionCardGridViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if ((sender as Grid).Name == "")
            {
                return;
            }
            NewDeckTip.Visibility = Visibility.Collapsed;
            MinionCard addedCard = (sender as Grid).DataContext as MinionCard;
            creatingDeck.MinionCard.Add(addedCard);
            CardDataManager.AddCardToListView(NewDeckCardListView, addedCard);
            return;
        }
        private void SpellCardGridViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if ((sender as Grid).Name == "")
            {
                return;
            }
            NewDeckTip.Visibility = Visibility.Collapsed;
            SpellCard addedCard = (sender as Grid).DataContext as SpellCard;
            creatingDeck.SpellCard.Add(addedCard);
            CardDataManager.AddCardToListView(NewDeckCardListView, addedCard);
            return;
        }

        //新建卡组
        private void NewDeckButton_Click(object sender, RoutedEventArgs e)
        {
            NewDeckName.Text = "";
            NewDeckTip.Visibility = Visibility.Visible;
            ChangeVisibilityByNewDeck();
        }
        private void DeckCreatBackButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByNewDeck();
            NewDeckTip.Visibility = Visibility.Collapsed;
        }
        private async void CreatButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByNewDeck();
            if (NewDeckName.Text != "")
            {
                creatingDeck.ChangeName(NewDeckName.Text);
            }
            else
            {
                int maxn = 0;
                for (int i = 0; i < deckList.Count; i++)
                {
                    if(deckList[i].Name.Contains("Deck "))
                    {
                        if(int.Parse(deckList[i].Name[5].ToString())>maxn)
                        {
                            maxn = int.Parse(deckList[i].Name[5].ToString());
                        }
                    }
                }
                creatingDeck.ChangeName("Deck "+(maxn+1).ToString());
            }
            //CardDataManager.SortCardListByCost(creatingDeck.MinionCard);
            //CardDataManager.SortCardListByCost(creatingDeck.SpellCard);
            await DeckManager.CreatDeck(creatingDeck);
            NewDeckTip.Visibility = Visibility.Collapsed;
            DeckLoad();
        }

        //卡组详情
        private void DeckListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeckDetailListView.Items.Clear();
            ChangeVisibilityByDeckDetail();
            deckDetailMinionCardList.Clear();
            deckDetail = (Deck)e.ClickedItem;
            for (int i = 0; i < deckDetail.MinionCard.Count; i++)
            {
                CardDataManager.AddCardToListView(DeckDetailListView, deckDetail.MinionCard[i]);
            }
            for (int i = 0; i < deckDetail.SpellCard.Count; i++)
            {
                CardDataManager.AddCardToListView(DeckDetailListView, deckDetail.SpellCard[i]);
            }
        }
        private void DeckDetailBackButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByDeckDetail();
        }
        private async void DeleteDeckButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibilityByDeckDetail();
            string aimName = deckDetail.Name;
            await DeckManager.DeleteDeck(aimName);
            DeckLoad();
        }
        private void DeckDetailListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = (e.ClickedItem as Grid).Name;
            for (int i = 0; i < minionCardList.Count; i++)
            {
                if (minionCardList[i].Name == name)
                {
                    ShowCardDetail(minionCardList[i]);
                    return;
                }
            }
            for (int i = 0; i < spellCardList.Count; i++)
            {
                if (spellCardList[i].Name == name)
                {
                    ShowCardDetail(spellCardList[i]);
                    return;
                }
            }
        }


        private void ShowCardDetail(MinionCard minionCardDetail)
        {
            if (CardDetailRelativePanel.Visibility == Visibility.Collapsed)
                ChangeVisibilityByCardDetail();
            DetailMinionCardAttack.Text = "Attack: " + minionCardDetail.Attack.ToString();
            DetailMinionCardCost.Text = "Cost: " + minionCardDetail.Cost.ToString();
            DetailMinionCardHealth.Text = "Health: " + minionCardDetail.Health.ToString();
            DetailMinionCardCategory.Text = minionCardDetail.Category.ToString();
            DetailMinionCardName.Text = minionCardDetail.Name;
            DetailMinionCardDes.Text = minionCardDetail.Description;
            DetailMinionCardImage.Source = new BitmapImage(new Uri(minionCardDetail.ImagePath));
        }
        private void ShowCardDetail(SpellCard spellCard)
        {
            if (CardDetailRelativePanel.Visibility == Visibility.Collapsed)
                ChangeVisibilityByCardDetail();
            DetailMinionCardAttack.Text = "";
            DetailMinionCardCost.Text = "Cost: " + spellCard.Cost.ToString();
            DetailMinionCardHealth.Text = "";
            DetailMinionCardCategory.Text = "";
            DetailMinionCardName.Text = spellCard.Name;
            DetailMinionCardDes.Text = spellCard.Description;
            DetailMinionCardImage.Source = new BitmapImage(new Uri(spellCard.ImagePath));
        }

        private string DealCreatedDeckName(string name)
        {
            if (name.Length != 0)
            {
                return name;
            }
            else
            {
                int i = 1;
                int j;
                while (true)
                {
                    string newName = "Deck " + i.ToString();
                    for (j = 0; j < deckList.Count; j++)
                    {
                        if (deckList[j].Name == newName) break;
                    }
                    if (j == deckList.Count) return newName;
                    i = i + 1;
                }
            }
        }
        private void ChangeCardGirdViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (MinionCardGridView.Visibility == Visibility.Visible)
            {
                MinionCardGridView.Visibility = Visibility.Collapsed;
                SpellCardGridView.Visibility = Visibility.Visible;
            }
            else
            {
                MinionCardGridView.Visibility = Visibility.Visible;
                SpellCardGridView.Visibility = Visibility.Collapsed;
            }
        }

    }
}
