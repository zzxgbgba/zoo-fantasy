using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using ZooFantasy.EffectData;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace ZooFantasy.CardData
{
    class CardDataManager
    {
        public static async Task LoadMinionCardsFromFolder(List<MinionCard> minionCardList, StorageFolder parentFolder)
        {
            foreach (var categoryFolder in await parentFolder.GetFoldersAsync())
            {
                foreach (var item in await categoryFolder.GetFilesAsync())
                {
                    if ((item as StorageFile).FileType == ".txt")
                    {
                        MinionCard newMinionCard = await LoadMinionCardFromFile(item as StorageFile);
                        minionCardList.Add(newMinionCard);
                    }

                }

            }
        }
        public static async Task LoadSpellCardsFromFolder(List<SpellCard> spellCardList, StorageFolder parentFolder)
        {
            foreach (var item in await parentFolder.GetFilesAsync())
            {
                if ((item as StorageFile).FileType == ".txt" && (item as StorageFile).Name != "List.txt")
                {
                    SpellCard newSpellCard = await LoadSpellCardFromFile(item as StorageFile);
                    spellCardList.Add(newSpellCard);
                }

            }

        }

        public static async Task<MinionCard> LoadMinionCardFromFile(StorageFile item)
        {
            int cost, attack, health;
            string name, category, description;
            bool a, b, c, d;
            string p, q;
            using (Stream fileStream = await item.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(fileStream))
                {
                    cost = int.Parse(read.ReadLine());
                    attack = int.Parse(read.ReadLine());
                    health = int.Parse(read.ReadLine());
                    name = read.ReadLine();
                    category = read.ReadLine();
                    description = read.ReadLine();
                    MinionCard tmpMinionCard = new MinionCard(cost, attack, health, name, category, description);
                    while (true)
                    {
                        p = read.ReadLine();
                        if (p == "#End" || p == null) break;
                        switch (p)
                        {
                            case "#BattlecryEffect":
                                {
                                    q = read.ReadLine();
                                    if (q[0] == '1') a = true;
                                    else a = false;
                                    if (q[1] == '1') b = true;
                                    else b = false;
                                    if (q[2] == '1') c = true;
                                    else c = false;
                                    if (q[3] == '1') d = true;
                                    else d = false;
                                    tmpMinionCard.AddEffect(new Effect("Battlecry", a, b, c, d));
                                    break;
                                }
                            case "#Charge":
                                {
                                    q = read.ReadLine();
                                    if (q[0] == '1') a = true;
                                    else a = false;
                                    if (q[1] == '1') b = true;
                                    else b = false;
                                    if (q[2] == '1') c = true;
                                    else c = false;
                                    if (q[3] == '1') d = true;
                                    else d = false;
                                    tmpMinionCard.AddEffect(new Effect("Charge", a, b, c, d));
                                    break;
                                }
                            case "*Requirement":
                                {
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddRequirement(q);
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddRequirementValue(q);
                                    break;
                                }
                            case "*Result":
                                {
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddResult(q);
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddResultValue(q);
                                    break;
                                }
                            case "*AimRequirement":
                                {
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddAimRequirement(q);
                                    q = read.ReadLine();
                                    tmpMinionCard.Effects.Last().AddAimRequirementValue(q);
                                    break;
                                }
                        }
                    }
                    return tmpMinionCard;
                }
            }
        }
        public static async Task<SpellCard> LoadSpellCardFromFile(StorageFile item)
        {
            int cost;
            string name, description;
            bool a, b, c, d;
            string p, q;
            using (Stream fileStream = await item.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(fileStream))
                {
                    cost = int.Parse(read.ReadLine());
                    name = read.ReadLine();
                    description = read.ReadLine();
                    SpellCard tmpSpellCard = new SpellCard(cost, name, description);
                    while (true)
                    {
                        p = read.ReadLine();
                        if (p == "#End" || p == null) break;
                        switch (p)
                        {
                            case "#SpellEffect":
                                {
                                    q = read.ReadLine();
                                    if (q[0] == '1') a = true;
                                    else a = false;
                                    if (q[1] == '1') b = true;
                                    else b = false;
                                    if (q[2] == '1') c = true;
                                    else c = false;
                                    if (q[3] == '1') d = true;
                                    else d = false;
                                    tmpSpellCard.SetEffect(new Effect("Spell", a, b, c, d));
                                    break;
                                }
                            case "*Requirement":
                                {
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddRequirement(q);
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddRequirementValue(q);
                                    break;
                                }
                            case "*Result":
                                {
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddResult(q);
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddResultValue(q);
                                    break;
                                }
                            case "*AimRequirement":
                                {
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddAimRequirement(q);
                                    q = read.ReadLine();
                                    tmpSpellCard.SpellEffect.AddAimRequirementValue(q);
                                    break;
                                }
                        }
                    }
                    return tmpSpellCard;
                }
            }
        }

        public static async void SaveMinionCardToFile(MinionCard minionCard, StorageFile item)
        {
            using (Stream fileStream = await item.OpenStreamForWriteAsync())
            {
                using (StreamWriter write = new StreamWriter(fileStream))
                {
                    write.WriteLine(minionCard.Cost);
                    write.WriteLine(minionCard.Attack);
                    write.WriteLine(minionCard.Health);
                    write.WriteLine(minionCard.Name);
                    write.WriteLine(minionCard.Category);
                    write.WriteLine(minionCard.Description);
                    for (int i = 0; i < minionCard.Effects.Count; i++)
                    {
                        Effect aimEffect = minionCard.Effects[i];
                        switch (aimEffect.Category)
                        {
                            case EffectCategory.Battlecry:
                                {
                                    write.WriteLine("#BattlecryEffect");
                                    break;
                                }
                            case EffectCategory.Charge:
                                {
                                    write.WriteLine("#Charge");
                                    break;
                                }
                        }
                        if (aimEffect.EffectAim.playerMinion) write.Write('1');
                        else write.Write('0');
                        if (aimEffect.EffectAim.enemyMinion) write.Write('1');
                        else write.Write('0');
                        if (aimEffect.EffectAim.playerHero) write.Write('1');
                        else write.Write('0');
                        if (aimEffect.EffectAim.enemyHero) write.Write('1');
                        else write.Write('0');
                        write.WriteLine("");
                        for (int j = 0; j < aimEffect.Requirements.Count; j++)
                        {
                            write.WriteLine("*Requirement");
                            write.WriteLine(aimEffect.Requirements[j]);
                            write.WriteLine(aimEffect.RequirementValues[j]);
                        }
                        for (int j = 0; j < aimEffect.Results.Count; j++)
                        {
                            write.WriteLine("*Result");
                            write.WriteLine(aimEffect.Results[j]);
                            write.WriteLine(aimEffect.ResultValues[j]);
                        }
                        for (int j = 0; j < aimEffect.AimRequirements.Count; j++)
                        {
                            write.WriteLine("*AimRequirement");
                            write.WriteLine(aimEffect.AimRequirements[j]);
                            write.WriteLine(aimEffect.AimRequirementValues[j]);
                        }
                        write.WriteLine("#End");
                    }
                    write.WriteLine("#End");
                }

            }
        }
        public static async void SaveSpellCardToFile(SpellCard spellCard, StorageFile item)
        {
            using (Stream fileStream = await item.OpenStreamForWriteAsync())
            {
                using (StreamWriter write = new StreamWriter(fileStream))
                {
                    write.WriteLine(spellCard.Cost);
                    write.WriteLine(spellCard.Name);
                    write.WriteLine(spellCard.Description);
                    Effect aimEffect = spellCard.SpellEffect;
                    switch (aimEffect.Category)
                    {
                        case EffectCategory.Spell:
                            {
                                write.WriteLine("#SpellEffect");
                                break;
                            }
                    }
                    if (aimEffect.EffectAim.playerMinion) write.Write('1');
                    else write.Write('0');
                    if (aimEffect.EffectAim.enemyMinion) write.Write('1');
                    else write.Write('0');
                    if (aimEffect.EffectAim.playerHero) write.Write('1');
                    else write.Write('0');
                    if (aimEffect.EffectAim.enemyHero) write.Write('1');
                    else write.Write('0');
                    write.WriteLine("");
                    for (int j = 0; j < aimEffect.Requirements.Count; j++)
                    {
                        write.WriteLine("*Requirement");
                        write.WriteLine(aimEffect.Requirements[j]);
                        write.WriteLine(aimEffect.RequirementValues[j]);
                    }
                    for (int j = 0; j < aimEffect.Results.Count; j++)
                    {
                        write.WriteLine("*Result");
                        write.WriteLine(aimEffect.Results[j]);
                        write.WriteLine(aimEffect.ResultValues[j]);
                    }
                    for (int j = 0; j < aimEffect.AimRequirements.Count; j++)
                    {
                        write.WriteLine("*AimRequirement");
                        write.WriteLine(aimEffect.AimRequirements[j]);
                        write.WriteLine(aimEffect.AimRequirementValues[j]);
                    }
                    write.WriteLine("#End");
                }

            }
        }

        public static void AddCardToGridView_Middle(GridView gridView, MinionCard addMinionCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addMinionCard.Name,
                Height = 235,
                Width = 150,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addMinionCard,
                IsRightTapEnabled = true
            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addMinionCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addMinionCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 24,
                Margin = new Thickness(0, 152, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addMinionCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 26,
                Margin = new Thickness(0, 176, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardAttack = new TextBlock()
            {
                Text = addMinionCard.Attack.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardHealth = new TextBlock()
            {
                Text = addMinionCard.Health.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardCategory = new TextBlock()
            {
                Text = addMinionCard.Category.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            if (cardName.Text.Length > 8)
            {
                cardName.FontSize = 22;
                cardName.Margin = new Thickness(0, 180, 0, 0);
            }
            if (cardName.Text.Length > 14)
            {
                cardName.FontSize = 18;
                cardName.Margin = new Thickness(0, 184, 0, 0);
            }
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            cardPanel.Children.Add(cardAttack);
            cardPanel.Children.Add(cardHealth);
            cardPanel.Children.Add(cardCategory);
            gridView.Items.Add(cardPanel);
        }//将随从牌添加至GridView控件（中等）
        public static void AddCardToGridView_Middle(GridView gridView, SpellCard addSpellCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addSpellCard.Name,
                Height = 235,
                Width = 150,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addSpellCard,
                IsRightTapEnabled = true,

            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addSpellCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addSpellCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 24,
                Margin = new Thickness(0, 152, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addSpellCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 26,
                Margin = new Thickness(0, 176, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            if (cardName.Text.Length > 8)
            {
                cardName.FontSize = 22;
                cardName.Margin = new Thickness(0, 180, 0, 0);
            }
            if(cardName.Text.Length > 14)
            {
                cardName.FontSize = 18;
                cardName.Margin = new Thickness(0, 184, 0, 0);
            }
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            gridView.Items.Add(cardPanel);
        }//将法术牌添加至GridView控件（中等）
        public static void AddCardToGridView_Little(GridView gridView, MinionCard addMinionCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addMinionCard.Name,
                Height = 160,
                Width = 100,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addMinionCard,
                IsRightTapEnabled = true
            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addMinionCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addMinionCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 18,
                Margin = new Thickness(5, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addMinionCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18,
                Margin = new Thickness(0, 120, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardAttack = new TextBlock()
            {
                Text = addMinionCard.Attack.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardHealth = new TextBlock()
            {
                Text = addMinionCard.Health.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardCategory = new TextBlock()
            {
                Text = addMinionCard.Category.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            if (cardName.Text.Length > 8) cardName.FontSize = 14;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            cardPanel.Children.Add(cardAttack);
            cardPanel.Children.Add(cardHealth);
            cardPanel.Children.Add(cardCategory);
            gridView.Items.Add(cardPanel);
        }//将随从牌添加至GridView控件（较小）
        public static void AddCardToGridView_Little(GridView gridView, SpellCard addSpellCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addSpellCard.Name,
                Height = 160,
                Width = 100,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addSpellCard,
                IsRightTapEnabled = true,

            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addSpellCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addSpellCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 18,
                Margin = new Thickness(5, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addSpellCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18,
                Margin = new Thickness(0, 120, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            if (cardName.Text.Length > 8) cardName.FontSize = 12;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            gridView.Items.Add(cardPanel);
        }//将法术牌添加至GridView控件（较小）
        public static void AddCardToGridView_Little(GridView gridView, MinionCard addMinionCard, Card card)
        {
            Grid cardPanel = new Grid()
            {
                Name = addMinionCard.Name,
                Height = 160,
                Width = 100,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = card,
                IsRightTapEnabled = true
            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addMinionCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addMinionCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 18,
                Margin = new Thickness(5, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addMinionCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18,
                Margin = new Thickness(0, 120, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardAttack = new TextBlock()
            {
                Text = addMinionCard.Attack.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardHealth = new TextBlock()
            {
                Text = addMinionCard.Health.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardCategory = new TextBlock()
            {
                Text = addMinionCard.Category.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            if (cardName.Text.Length > 8) cardName.FontSize = 14;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            cardPanel.Children.Add(cardAttack);
            cardPanel.Children.Add(cardHealth);
            cardPanel.Children.Add(cardCategory);
            gridView.Items.Add(cardPanel);
        }//将随从牌添加至GridView控件（较小 带card参数）
        public static void AddCardToGridView_Little(GridView gridView, SpellCard addSpellCard, Card card)
        {
            Grid cardPanel = new Grid()
            {
                Name = addSpellCard.Name,
                Height = 160,
                Width = 100,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = card,
                IsRightTapEnabled = true,

            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addSpellCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addSpellCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 18,
                Margin = new Thickness(5, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addSpellCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18,
                Margin = new Thickness(0, 120, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            if (cardName.Text.Length > 8) cardName.FontSize = 12;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            gridView.Items.Add(cardPanel);
        }//将法术牌添加至GridView控件（较小 带Card参数）
        public static void AddCardToListView(ListView listView, MinionCard addMinionCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addMinionCard.Name,
                Height = 50,
                Width = 190,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addMinionCard,
                IsRightTapEnabled = true,

            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addMinionCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addMinionCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addMinionCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                Margin = new Thickness(15, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            if (cardName.Text.Length > 8) cardName.FontSize = 16;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            listView.Items.Add(cardPanel);
        }//将随从牌添加至ListView控件
        public static void AddCardToListView(ListView listView, SpellCard addSpellCard)
        {
            Grid cardPanel = new Grid()
            {
                Name = addSpellCard.Name,
                Height = 50,
                Width = 190,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = addSpellCard,
                IsRightTapEnabled = true,

            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(addSpellCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            TextBlock cardCost = new TextBlock()
            {
                Text = addSpellCard.Cost.ToString(),
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            TextBlock cardName = new TextBlock()
            {
                Text = addSpellCard.Name,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                Margin = new Thickness(15, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            if (cardName.Text.Length > 8) cardName.FontSize = 16;
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardCost);
            cardPanel.Children.Add(cardName);
            listView.Items.Add(cardPanel);
        }//将随从牌添加至ListView控件

        public static void AddCardToCardList(List<MinionCard> cardList, MinionCard addCard)
        {
            cardList.Add(addCard);
        }

        //排序函数
        private static int _SortCardListByCost(MinionCard a, MinionCard b)
        {
            if (a.Cost > b.Cost)
                return 1;
            if (a.Cost < b.Cost)
                return -1;
            return 0;
        }
        public static void SortCardListByCost(List<MinionCard> list)
        {
            list.Sort(_SortCardListByCost);
        }
        private static int _SortCardListByCost(SpellCard a, SpellCard b)
        {
            if (a.Cost > b.Cost)
                return 1;
            if (a.Cost < b.Cost)
                return -1;
            return 0;
        }
        public static void SortCardListByCost(List<SpellCard> list)
        {
            list.Sort(_SortCardListByCost);
        }

        public static MinionCard FindCardByName(List<MinionCard> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return list[i];
            }
            return new MinionCard();
        }
        public static SpellCard FindCardByName(List<SpellCard> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return list[i];
            }
            return new SpellCard(0, "", "");
        }
    }
}
