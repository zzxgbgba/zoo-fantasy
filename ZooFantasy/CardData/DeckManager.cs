using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ZooFantasy.CardData
{
    class DeckManager
    {
        public static async Task LoadDecks(List<Deck> deckList)
        {
            StorageFolder parentFolder = ApplicationData.Current.LocalFolder;
            if (await parentFolder.TryGetItemAsync("Decks") == null)
                await parentFolder.CreateFolderAsync("Decks");
            parentFolder = await parentFolder.GetFolderAsync("Decks");
            Deck tmpDeck;
            foreach (var item in await parentFolder.GetFoldersAsync())
            {
                tmpDeck = await LoadDeckFromDeckFolder(item);
                deckList.Add(tmpDeck);
            }
        }
        public static async Task LoadDecks(ObservableCollection<Deck> deckList)
        {
            StorageFolder parentFolder = ApplicationData.Current.LocalFolder;
            if (await parentFolder.TryGetItemAsync("Decks") == null)
                await parentFolder.CreateFolderAsync("Decks");
            parentFolder = await parentFolder.GetFolderAsync("Decks");
            Deck tmpDeck;
            foreach (var item in await parentFolder.GetFoldersAsync())
            {
                tmpDeck = await LoadDeckFromDeckFolder(item);
                deckList.Add(tmpDeck);
            }
        }
        public static async Task<Deck> LoadDeckFromDeckFolder(StorageFolder folder)
        {
            Deck newDeck = new Deck(folder.Name);
            MinionCard tmpMinionCard;
            SpellCard tmpSpellCard;
            StorageFolder minionFolder = await folder.GetFolderAsync("Minions");
            StorageFolder spellFolder = await folder.GetFolderAsync("Spells");
            foreach (var item in await minionFolder.GetFilesAsync())
            {
                tmpMinionCard = await CardDataManager.LoadMinionCardFromFile(item as StorageFile);
                newDeck.MinionCard.Add(tmpMinionCard);
            }
            foreach (var item in await spellFolder.GetFilesAsync())
            {
                tmpSpellCard = await CardDataManager.LoadSpellCardFromFile(item as StorageFile);
                newDeck.SpellCard.Add(tmpSpellCard);
            }
            return newDeck;
        }
        public static async Task CreatDeck(Deck newDeck)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("Decks") == null)
                await folder.CreateFolderAsync("Decks");
            folder = await folder.GetFolderAsync("Decks");
            folder = await folder.CreateFolderAsync(newDeck.Name);
            StorageFolder minionFolder = await folder.CreateFolderAsync("Minions");
            StorageFolder spellFolder = await folder.CreateFolderAsync("Spells");
            StorageFile file;
            CardDataManager.SortCardListByCost(newDeck.MinionCard);
            CardDataManager.SortCardListByCost(newDeck.SpellCard);
            for (int i = 0; i < newDeck.MinionCard.Count; i++)
            {
                file = await minionFolder.CreateFileAsync(i.ToString() + ".txt");
                CardDataManager.SaveMinionCardToFile(newDeck.MinionCard[i], file);
            }
            for (int i = 0; i < newDeck.SpellCard.Count; i++)
            {
                file = await spellFolder.CreateFileAsync(i.ToString() + ".txt");
                CardDataManager.SaveSpellCardToFile(newDeck.SpellCard[i], file);
            }
        }
        public static async Task CreatDeck(Deck newDeck, StorageFolder folder)
        {
            folder = await folder.CreateFolderAsync(newDeck.Name);
            StorageFolder minionFolder = await folder.CreateFolderAsync("Minions");
            StorageFolder spellFolder = await folder.CreateFolderAsync("Spells");
            StorageFile file;
            for (int i = 0; i < newDeck.MinionCard.Count; i++)
            {
                file = await minionFolder.CreateFileAsync(i.ToString() + ".txt");
                CardDataManager.SaveMinionCardToFile(newDeck.MinionCard[i], file);
            }
            for (int i = 0; i < newDeck.SpellCard.Count; i++)
            {
                file = await spellFolder.CreateFileAsync(i.ToString() + ".txt");
                CardDataManager.SaveSpellCardToFile(newDeck.SpellCard[i], file);
            }
        }

        public static async Task DeleteDeck(string aimName)
        {
            StorageFolder parentFolder = ApplicationData.Current.LocalFolder;
            if (await parentFolder.TryGetItemAsync("Decks") == null)
                await parentFolder.CreateFolderAsync("Decks");
            parentFolder = await parentFolder.GetFolderAsync("Decks");
            foreach (var item in await parentFolder.GetFoldersAsync())
            {
                if (item.Name == aimName)
                {
                    await item.DeleteAsync();
                    break;
                }
            }


        }

        //临时卡组
        public static async Task CreatCacheDeck(Deck cacheDeck)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("Cache") == null)
                await folder.CreateFolderAsync("Cache");
            folder = await folder.GetFolderAsync("Cache");
            StorageFolder test = await folder.TryGetItemAsync("CacheDeck") as StorageFolder;
            if (test != null)
            {
                await test.DeleteAsync();
            }
            Deck tmpDeck = cacheDeck;
            tmpDeck.ChangeToCacheDeck();
            await CreatDeck(tmpDeck, folder);

        }
        public static async Task<Deck> LoadCacheDeck()
        {
            StorageFolder parentFolder = ApplicationData.Current.LocalFolder;
            if (await parentFolder.TryGetItemAsync("Cache") == null)
                await parentFolder.CreateFolderAsync("Cache");
            parentFolder = await parentFolder.GetFolderAsync("Cache");
            if (await parentFolder.TryGetItemAsync("CacheDeck") == null)
                return new Deck();
            StorageFolder aimFolder = await parentFolder.GetFolderAsync("CacheDeck");
            Deck deck = await LoadDeckFromDeckFolder(aimFolder);
            return deck;
        }

        public static void AddDeckToGridView(GridView gridView, Deck deck)
        {
            Grid deckPanel = new Grid()
            {
                Name = deck.Name,
                Height = 420,
                Width = 300,
                Margin = new Thickness(0, 0, 0, 0),
                BorderBrush = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(1.8)
            };
            GridView childGridView = new GridView()
            {
                Margin = new Thickness(5, 45, 0, 0),
                SelectionMode = ListViewSelectionMode.None,
                //ItemsPanel = new ItemsPanelTemplate()
            };
            TextBlock deckName = new TextBlock()
            {
                Text = deck.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                Margin = new Thickness(0, 10, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            for (int i = 0; i < deck.MinionCard.Count; i++)
            {
                childGridView.Items.Add(
                    new Image()
                    {
                        Source = new BitmapImage(new Uri(deck.MinionCard[i].ImagePath)),
                        Width = 40,
                        Height = 40
                    });
            }
            for (int i = 0; i < deck.SpellCard.Count; i++)
            {
                childGridView.Items.Add(
                    new Image()
                    {
                        Source = new BitmapImage(new Uri(deck.SpellCard[i].ImagePath)),
                        Width = 40,
                        Height = 40
                    });
            }

            deckPanel.Children.Add(deckName);
            deckPanel.Children.Add(childGridView);
            gridView.Items.Add(deckPanel);
        }
    }
}


