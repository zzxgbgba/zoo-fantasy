using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZooFantasy.MinionData;
using ZooFantasy.BattlefieldData;
using ZooFantasy.EffectData;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using System.Xml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZooFantasy.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BattlefieldPage : Page
    {
        private Battlefield mainBattlefield;
        public Battlefield MainBattlefield { get { return mainBattlefield; } }
        private Effect doneEffect;

        public BattlefieldPage()
        {
            draggedMinionCard = new MinionCard();
            mainBattlefield = new Battlefield();
            this.InitializeComponent();
            LoadBattlefield();

        }
        private async void LoadBattlefield()
        {
            await mainBattlefield.BattlefieldInit();
            HandCardInit();
            HerosInit();
        }

        private void PlayerTurn()
        {
            mainBattlefield.PlayerTurnInit();
            UpdateCostTextBlock();
            UpdateHandCardGridView(HandCardGridView);
            foreach (var item in PlayerMinionPanel.Items)
            {
                ((item as Grid).DataContext as Minion).ChangeCanAttack(true);
            }
        }//玩家回合初始化
        private void EnemyTurn()
        {
            MinionManager.AddMinionToGridView(EnemyMinionPanel, new Minion(new MinionCard(1, 1, 1, "Zebra", "Zebra", "Test Zebra")));
            (EnemyMinionPanel.Items.Last() as Grid).DragOver += EnemyMinion_DragOver;
            (EnemyMinionPanel.Items.Last() as Grid).Drop += EnemyMinion_Drop;
            (EnemyMinionPanel.Items.Last() as Grid).PointerEntered += MinionGrid_PointerEntered;
            (EnemyMinionPanel.Items.Last() as Grid).PointerExited += MinionGrid_PointerExited;
        }//敌方回合初始化

        private void PlayerEndTurnButton_Click(object sender, RoutedEventArgs e)
        {
            EnemyTurn();
            PlayerTurn();
            UpdateCostTextBlock();
            PlayerMinionPanel.SelectedItem = null;
        } //回合结束按钮
        private void UpdateCostTextBlock()
        {
            CurrentCostTextBlock.Text = mainBattlefield.CurrentCost.ToString();
            MaxCostTextBlock.Text = '/'+ mainBattlefield.MaxCost.ToString();
        } //更新费用布局

        private void HerosInit()//英雄控件初始化
        {
            PlayerHeroGrid.DataContext = new PlayerHeroData(30);
            EnemyHeroGrid.DataContext = new EnemyHeroData(30);
            UpdateHerosHealth();
        }
        private void HandCardInit()
        {
            #region ChangeVisibility
            OriginHandCardGridView.Visibility = Visibility.Visible;
            ConfirmHandCardButton.Visibility = Visibility.Visible;
            HandCardGridView.Visibility = Visibility.Collapsed;
            CurrentCostTextBlock.Visibility = Visibility.Collapsed;
            PlayerEndTurnButton.Visibility = Visibility.Collapsed;
            #endregion
            HandCardManager.DrawCard(4, mainBattlefield.Player);
            UpdateHandCardGridView(OriginHandCardGridView);
        }//手牌控件初始化
        private void ConfirmHandCardButton_Click(object sender, RoutedEventArgs e)
        {
            #region ChangeVisibility
            OriginHandCardGridView.Visibility = Visibility.Collapsed;
            ConfirmHandCardButton.Visibility = Visibility.Collapsed;
            HandCardGridView.Visibility = Visibility.Visible;
            CostTextBlock.Visibility = Visibility.Visible;
            MaxCostTextBlock.Visibility = Visibility.Visible;
            CurrentCostTextBlock.Visibility = Visibility.Visible;
            PlayerEndTurnButton.Visibility = Visibility.Visible;
            LeftCardNum.Visibility = Visibility.Visible;
            #endregion
            var changeList = OriginHandCardGridView.SelectedItems;
            foreach (var item in changeList)
            {
                Card card = (item as Grid).DataContext as Card;
                if (card.CardType == CardTypes.MinionCard)
                {
                    mainBattlefield.Player.HandCard.Remove(card);
                    HandCardManager.DrawCard(mainBattlefield.Player);
                    HandCardManager.ShuffleIntoDeck((MinionCard)card.CardData, mainBattlefield.Player);
                }
                else
                {
                    mainBattlefield.Player.HandCard.Remove(card);
                    HandCardManager.DrawCard(mainBattlefield.Player);
                    HandCardManager.ShuffleIntoDeck((SpellCard)card.CardData, mainBattlefield.Player);
                }
            }
            PlayerTurn();
        }//换牌按钮点击

        private MinionCard draggedMinionCard;
        private SpellCard draggedSpellCard;
        private Card draggedCard;
        private Minion draggedMinion;
        private Grid draggedGrid;
        private string draggingType;

        private void HandCardGridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedCard = (e.Items.FirstOrDefault() as Grid).DataContext as Card;
            if (draggedCard.CardType == CardTypes.MinionCard)
            {
                draggedMinionCard = draggedCard.CardData as MinionCard;
                if (PlayerMinionPanel.Items.Count >= 7)
                {
                    e.Cancel = true;
                    return;
                }
                if (mainBattlefield.CurrentCost < draggedMinionCard.Cost)
                {
                    e.Cancel = true;
                    return;
                }
                draggingType = "MinionCard";
            }
            else
            {
                draggingType = "SpellCard";
                draggedSpellCard = draggedCard.CardData as SpellCard;
                if (mainBattlefield.CurrentCost < draggedSpellCard.Cost)
                {
                    e.Cancel = true;
                    return;
                }
                doneEffect = draggedSpellCard.SpellEffect;
                if (EffectRequirementCheck.CheckRequirments(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionPanel, EnemyMinionPanel, mainBattlefield, doneEffect.Requirements, doneEffect.RequirementValues))
                {
                    EffectAim aim = doneEffect.EffectAim;
                    if (!(aim.enemyHero || aim.enemyMinion || aim.playerHero || aim.playerMinion))
                    {
                        MainGrid.DragOver += CastSpell_DragOver;
                        MainGrid.Drop += CastSpell_Drop;
                        return;
                    }
                    FocusAimForSpell(doneEffect.EffectAim);
                    return;
                }
                else
                {
                    e.Cancel = true;
                }

            }
        }//拖拽卡牌
        private void PlayerMinionPanel_DragOver(object sender, DragEventArgs e)
        {
            if (draggingType == "MinionCard")
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = "Summon " + draggedMinionCard.Name;
            }

        }//拖拽卡牌
        private void PlayerMinionPanel_Drop(object sender, DragEventArgs e)
        {
            if (draggingType == "MinionCard")
            {
                SummonMinionFromHandCard(draggedMinionCard);
                mainBattlefield.Player.HandCard.Remove(draggedCard);
                UpdateHandCardGridView(HandCardGridView);
                MinionManager.AddMinionToGridView(PlayerMinionPanel, new Minion(draggedMinionCard));
                (PlayerMinionPanel.Items.Last() as Grid).DragStarting += Minion_DragStarting;
                (PlayerMinionPanel.Items.Last() as Grid).PointerEntered += MinionGrid_PointerEntered;
                (PlayerMinionPanel.Items.Last() as Grid).PointerExited += MinionGrid_PointerExited;
                UpdateCostTextBlock();
                draggingType = "None";
            }
        }//拖拽卡牌（Drop）

        private void MinionGrid_PointerEntered(object sender, PointerRoutedEventArgs e) //显示随从详情
        {
            Minion minion = (sender as Grid).DataContext as Minion;
            DetailImage.Source = new BitmapImage(new Uri(minion.OriginMinionCard.ImagePath));
            DetailName.Text = minion.OriginMinionCard.Name;
            DetailCost.Text = minion.OriginMinionCard.Cost.ToString();
            DetailCategory.Text = minion.OriginMinionCard.Category;
            DetailDes.Text = minion.OriginMinionCard.Description;
            DetailAttack.Text = "Attack: " + minion.CurrentAttack.ToString();
            DetailHealth.Text = "Health: " + minion.CurrentHealth.ToString();
            DetailRelativePanel.Visibility = Visibility.Visible;
        }
        private void MinionGrid_PointerExited(object sender, PointerRoutedEventArgs e) //取消显示随从详情
        {
            DetailRelativePanel.Visibility = Visibility.Collapsed;
        }
        private void Card_PointerEntered(object sender, PointerRoutedEventArgs e) //显示卡牌详情
        {
            if (((sender as Grid).DataContext as Card).CardType == CardTypes.MinionCard)
            {
                MinionCard card = ((sender as Grid).DataContext as Card).CardData as MinionCard;
                DetailImage.Source = new BitmapImage(new Uri(card.ImagePath));
                DetailName.Text = card.Name;
                DetailCost.Text = card.Cost.ToString();
                DetailCategory.Text = card.Category;
                DetailDes.Text = card.Description;
                DetailAttack.Text = "Attack: " + card.Attack.ToString();
                DetailHealth.Text = "Health: " + card.Health.ToString();
            }
            if (((sender as Grid).DataContext as Card).CardType == CardTypes.SpellCard)
            {
                SpellCard card = ((sender as Grid).DataContext as Card).CardData as SpellCard;
                DetailImage.Source = new BitmapImage(new Uri(card.ImagePath));
                DetailName.Text = card.Name;
                DetailCost.Text = card.Cost.ToString();
                DetailCategory.Text = "";
                DetailDes.Text = card.Description;
                DetailAttack.Text = "";
                DetailHealth.Text = "";
            }
            DetailRelativePanel.Visibility = Visibility.Visible;
        }
        private void Card_PointerExited(object sender, PointerRoutedEventArgs e) //取消显示卡牌详情
        {
            DetailRelativePanel.Visibility = Visibility.Collapsed;
        }

        private void Minion_DragStarting(UIElement sender, DragStartingEventArgs args) //拖拽随从攻击
        {
            draggedGrid = (sender as Grid);
            if ((draggedGrid.DataContext as Minion).CanAttack)
            {
                draggingType = "PlayerMinion";
            }
            else
            {
                args.Cancel = true;
            }
        }
        private void EnemyMinion_DragOver(object sender, DragEventArgs e) //拖拽随从攻击（Minion_DragOver）
        {
            if (draggingType == "PlayerMinion")
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = "Attack " + ((sender as Grid).DataContext as Minion).OriginMinionCard.Name;
            }
        }
        private void EnemyHeroGrid_DragOver(object sender, DragEventArgs e) //拖拽随从攻击（HeroGrid_DragOver）& 拖拽施放法术（HeroGrid_DragOver）
        {
            if (draggingType == "PlayerMinion")
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = "Attack Hero";
            }
            if (draggingType == "SpellCard")
            {

            }
        }
        private void EnemyMinion_Drop(object sender, DragEventArgs e) //拖拽随从攻击（Minion_Drop）
        {
            if (draggingType == "PlayerMinion")
            {
                //(e.Data as Grid).DataContext 
                MinionManager.MinionAttack(draggedGrid.DataContext as Minion, (sender as Grid).DataContext as Minion);
                UpdateAllMinionGrid();
            }
        }
        private void EnemyHeroGrid_Drop(object sender, DragEventArgs e) //拖拽随从攻击（EnemyHero_Drop）& 拖拽施放法术（EnemyHero_Drop）
        {
            if (draggingType == "PlayerMinion")
            {
                //(e.Data as Grid).DataContext 
                MinionManager.MinionAttack(draggedGrid, (sender as Grid));
                UpdateAllMinionGrid();
                UpdateHerosHealth();
            }
            if (draggingType == "SpellCard")
            {

            }
        }

        private void SummonMinionFromHandCard(MinionCard summoningMinion) //从手牌中召唤随从
        {
            mainBattlefield.SummonPlayerMinion(new Minion(summoningMinion));
            for (int i = 0; i < summoningMinion.Effects.Count; i++) //战吼效果检查
            {
                Effect aimEffect = summoningMinion.Effects[i];
                if (aimEffect.Category == EffectData.EffectCategory.Battlecry)
                {
                    DealEffect(aimEffect);
                }
            }
        }

        private void DealEffect(Effect effect) //启动特效触发流程
        {
            if (EffectRequirementCheck.CheckRequirments(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionPanel, EnemyMinionPanel, mainBattlefield, effect))
            {
                doneEffect = effect;
                EffectAim aim = effect.EffectAim;
                if (!(aim.enemyHero || aim.enemyMinion || aim.playerHero || aim.playerMinion))
                {
                    EffectAim_Tapped(new Grid(), new TappedRoutedEventArgs());
                    return;
                }
                if (effect.Category != EffectCategory.Spell) FocusAim(effect.EffectAim);
                else FocusAimForSpell(effect.EffectAim);
            }
            else
            {
                return;
            }
        }
        private void FocusAim(EffectAim aim)
        {
            ChangeVisibilityForAim();
            if (aim.enemyMinion)
            {
                foreach (var item in EnemyMinionPanel.Items)
                {
                    if (EffectAimRequirementCheck.AimRequirementsCheck((item as Grid).DataContext, doneEffect))
                    {
                        (item as Grid).BorderBrush = new SolidColorBrush(Colors.Green);
                        (item as Grid).BorderThickness = new Thickness(5);
                        (item as Grid).Tapped += EffectAim_Tapped;
                    }
                }
            }
            if (aim.playerMinion)
            {
                foreach (var item in PlayerMinionPanel.Items)
                {
                    if (EffectAimRequirementCheck.AimRequirementsCheck((item as Grid).DataContext, doneEffect))
                    {
                        (item as Grid).BorderBrush = new SolidColorBrush(Colors.Green);
                        (item as Grid).BorderThickness = new Thickness(5);
                        (item as Grid).Tapped += EffectAim_Tapped;
                    }
                }
            }
            if (aim.playerHero)
            {
                if (EffectAimRequirementCheck.AimRequirementsCheck(PlayerHeroGrid.DataContext, doneEffect))
                {
                    PlayerHeroGrid.BorderBrush = new SolidColorBrush(Colors.Green);
                    PlayerHeroGrid.BorderThickness = new Thickness(5);
                    PlayerHeroGrid.Tapped += EffectAim_Tapped;
                }
            }
            if (aim.enemyHero)
            {
                if (EffectAimRequirementCheck.AimRequirementsCheck(EnemyHeroGrid.DataContext, doneEffect))
                {
                    EnemyHeroGrid.BorderBrush = new SolidColorBrush(Colors.Green);
                    EnemyHeroGrid.BorderThickness = new Thickness(5);
                    EnemyHeroGrid.Tapped += EffectAim_Tapped;
                }
            }
        } //显示选定边框
        private void FocusAimForSpell(EffectAim aim) //显示选定边框（法术）
        {
            ChangeVisibilityForAim();
            if (aim.enemyMinion)
            {
                foreach (var item in EnemyMinionPanel.Items)
                {
                    if (EffectAimRequirementCheck.AimRequirementsCheck((item as Grid).DataContext, doneEffect))
                    {
                        (item as Grid).BorderBrush = new SolidColorBrush(Colors.Green);
                        (item as Grid).BorderThickness = new Thickness(5);
                        (item as Grid).Drop += CastSpell_Drop;
                        (item as Grid).DragOver += CastSpell_DragOver;
                    }
                }
            }
            if (aim.playerMinion)
            {
                foreach (var item in PlayerMinionPanel.Items)
                {
                    if (EffectAimRequirementCheck.AimRequirementsCheck((item as Grid).DataContext, doneEffect))
                    {
                        (item as Grid).BorderBrush = new SolidColorBrush(Colors.Green);
                        (item as Grid).BorderThickness = new Thickness(5);
                        (item as Grid).Drop += CastSpell_Drop;
                        (item as Grid).DragOver += CastSpell_DragOver;
                    }
                }
            }
            if (aim.playerHero)
            {
                if (EffectAimRequirementCheck.AimRequirementsCheck(PlayerHeroGrid.DataContext, doneEffect))
                {
                    PlayerHeroGrid.BorderBrush = new SolidColorBrush(Colors.Green);
                    PlayerHeroGrid.BorderThickness = new Thickness(5);
                    PlayerHeroGrid.Drop += CastSpell_Drop;
                    PlayerHeroGrid.DragOver += CastSpell_DragOver;
                }
            }
            if (aim.enemyHero)
            {
                if (EffectAimRequirementCheck.AimRequirementsCheck(EnemyHeroGrid.DataContext, doneEffect))
                {
                    EnemyHeroGrid.BorderBrush = new SolidColorBrush(Colors.Green);
                    EnemyHeroGrid.BorderThickness = new Thickness(5);
                    EnemyHeroGrid.Drop += CastSpell_Drop;
                    EnemyHeroGrid.DragOver += CastSpell_DragOver;
                }
            }
        }

        private void CastSpell_DragOver(object sender, DragEventArgs e)
        {
            if (draggingType == "SpellCard")
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = "Cast " + draggedSpellCard.Name;
            }
        }
        private void CastSpell_Drop(object sender, DragEventArgs e) //拖拽施放法术（Drop）
        {
            MainGrid.DragOver -= CastSpell_DragOver;
            MainGrid.Drop -= CastSpell_Drop;
            MainGrid.DataContext = new MinionCard();
            mainBattlefield.CastPlayerSpell(draggedSpellCard);
            EffectResultDo.DoResults(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionPanel, EnemyMinionPanel, mainBattlefield, (sender as Grid).DataContext, doneEffect);
            mainBattlefield.Player.HandCard.Remove(draggedCard);
            UpdateHandCardGridView(HandCardGridView);
            UpdateAllMinionGrid();
            UpdateHerosHealth();
            UpdateCostTextBlock();
            CollapseAimForSpell();
        }

        private void EffectAim_Tapped(object sender, TappedRoutedEventArgs e) //目标选中
        {
            EffectResultDo.DoResults(PlayerHeroGrid, EnemyHeroGrid, PlayerMinionPanel, EnemyMinionPanel, mainBattlefield, (sender as Grid).DataContext, doneEffect);
            UpdateAllMinionGrid();
            UpdateHerosHealth();
            CollapseAim();
        }
        private void CollapseAim() //隐藏选定边框
        {
            if (HandCardGridView.Visibility == Visibility.Collapsed)
            {
                HandCardGridView.Visibility = Visibility.Visible;
            }
            if (RightBottomRelativePanel.Visibility == Visibility.Collapsed)
            {
                RightBottomRelativePanel.Visibility = Visibility.Visible;
            }
            foreach (var item in EnemyMinionPanel.Items)
            {
                (item as Grid).BorderThickness = new Thickness(0);
                (item as Grid).Tapped -= EffectAim_Tapped;
            }
            foreach (var item in PlayerMinionPanel.Items)
            {
                (item as Grid).BorderThickness = new Thickness(0);
                (item as Grid).Tapped -= EffectAim_Tapped;
            }
            PlayerHeroGrid.BorderThickness = new Thickness(0);
            PlayerHeroGrid.Tapped -= EffectAim_Tapped;
            EnemyHeroGrid.BorderThickness = new Thickness(0);
            EnemyHeroGrid.Tapped -= EffectAim_Tapped;

        }
        private void CollapseAimForSpell() //隐藏选定边框（法术）
        {
            if (HandCardGridView.Visibility == Visibility.Collapsed)
            {
                HandCardGridView.Visibility = Visibility.Visible;
            }
            if (RightBottomRelativePanel.Visibility == Visibility.Collapsed)
            {
                RightBottomRelativePanel.Visibility = Visibility.Visible;
            }
            foreach (var item in EnemyMinionPanel.Items)
            {
                (item as Grid).BorderThickness = new Thickness(0);
                (item as Grid).Drop -= CastSpell_Drop;
                (item as Grid).DragOver -= CastSpell_DragOver;
            }
            foreach (var item in PlayerMinionPanel.Items)
            {
                (item as Grid).BorderThickness = new Thickness(0);
                (item as Grid).Drop -= CastSpell_Drop;
                (item as Grid).DragOver -= CastSpell_DragOver;
            }
            PlayerHeroGrid.BorderThickness = new Thickness(0);
            PlayerHeroGrid.Drop -= CastSpell_Drop;
            PlayerHeroGrid.DragOver -= CastSpell_DragOver;

            EnemyHeroGrid.BorderThickness = new Thickness(0);
            EnemyHeroGrid.Drop -= CastSpell_Drop;
            EnemyHeroGrid.DragOver -= CastSpell_DragOver;

        }
        private void ChangeVisibilityForAim()//更改可视性
        {
            if (HandCardGridView.Visibility == Visibility.Visible)
            {
                HandCardGridView.Visibility = Visibility.Collapsed;
            }
            else
            {
                HandCardGridView.Visibility = Visibility.Visible;
            }
            if (RightBottomRelativePanel.Visibility == Visibility.Visible)
            {
                RightBottomRelativePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                RightBottomRelativePanel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateHandCardGridView(GridView gridView)
        {
            LeftCardNum.Text = (mainBattlefield.Player.CurrentDeck.MinionCard.Count() + mainBattlefield.Player.CurrentDeck.SpellCard.Count()).ToString() + " cards in deck";
            HandCardManager.UpdateHandCardGirdView(gridView, mainBattlefield.Player);
            foreach (var item in gridView.Items)
            {
                (item as Grid).PointerEntered += Card_PointerEntered;
                (item as Grid).PointerExited += Card_PointerExited;
            }
        }

        private void UpdateMinionGrid(Grid minion)//更新随从Grid控件
        {
            int health = (minion.DataContext as Minion).CurrentHealth;
            int attack = (minion.DataContext as Minion).CurrentAttack;
            (minion.Children.ElementAt(2) as TextBlock).Text = health.ToString();
            (minion.Children.ElementAt(1) as TextBlock).Text = attack.ToString();
        }
        private void UpdateAllMinionGrid() //更新全部随从Grid控件
        {
            foreach (var item in PlayerMinionPanel.Items)
            {
                UpdateMinionGrid(item as Grid);
            }
            foreach (var item in EnemyMinionPanel.Items)
            {
                UpdateMinionGrid(item as Grid);
            }
            for (int i = 0; i < PlayerMinionPanel.Items.Count; i++)
            {
                if(((PlayerMinionPanel.Items.ElementAt(i) as Grid).DataContext as Minion).CurrentHealth <= 0)
                {
                    PlayerMinionPanel.Items.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < EnemyMinionPanel.Items.Count; i++)
            {
                if (((EnemyMinionPanel.Items.ElementAt(i) as Grid).DataContext as Minion).CurrentHealth <= 0)
                {
                    EnemyMinionPanel.Items.RemoveAt(i);
                    i--;
                }
            }
        }
        private void UpdateHerosHealth()//更新英雄Grid控件
        {
            PlayerHeroHealthTextBlock.Text = (PlayerHeroGrid.DataContext as PlayerHeroData).Health.ToString();
            EnemyHeroHealthTextBlock.Text = (EnemyHeroGrid.DataContext as EnemyHeroData).Health.ToString();
            if ((EnemyHeroGrid.DataContext as EnemyHeroData).Health <= 0) Victory();
            if ((PlayerHeroGrid.DataContext as PlayerHeroData).Health <= 0) Defeat();
        }

        private void AddPlayerMinionAndEventToGridView(GridView gridView, Minion minion)
        {
            MinionManager.AddMinionToGridView(gridView, minion);
            (gridView.Items.Last() as Grid).DragStarting += Minion_DragStarting;
        }

        private async void MainPageButton_Click(object sender, RoutedEventArgs e) //返回主菜单按钮
        {
            var msgDialog = new ContentDialog()
            {
                Title = "",
                Content = "Exit Game?",
                FontSize = 24,
                PrimaryButtonText = "Confirm",
                SecondaryButtonText = "Cancel",
                FullSizeDesired = false,
            };
            msgDialog.PrimaryButtonClick += MsgDialog_PrimaryButtonClick;
            await msgDialog.ShowAsync();
        }
        private void MsgDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) //确定返回主菜单
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Victory()
        {
            var msgDialog = new ContentDialog()
            {
                Title = "Victory!",
                Content = "",
                FontSize = 40,
                PrimaryButtonText = "Confirm",
                FullSizeDesired = false,
            };
            msgDialog.PrimaryButtonClick += MsgDialog_PrimaryButtonClick;
            await msgDialog.ShowAsync();
        }
        private async void Defeat()
        {
            var msgDialog = new ContentDialog()
            {
                Title = "Defeat!",
                Content = "",
                FontSize = 40,
                PrimaryButtonText = "Confirm",
                FullSizeDesired = false,
            };
            msgDialog.PrimaryButtonClick += MsgDialog_PrimaryButtonClick;
            await msgDialog.ShowAsync();
        }

        private void HandCardGridView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if (draggingType != "SpellCard") return;
            CollapseAimForSpell();
            if (HandCardGridView.Visibility == Visibility.Collapsed)
            {
                HandCardGridView.Visibility = Visibility.Visible;
            }
            if (RightBottomRelativePanel.Visibility == Visibility.Collapsed)
            {
                RightBottomRelativePanel.Visibility = Visibility.Visible;
            }
        }

        private void MainGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (draggingType != "SpellCard") return;
            CollapseAimForSpell();
            if (HandCardGridView.Visibility == Visibility.Collapsed)
            {
                HandCardGridView.Visibility = Visibility.Visible;
            }
            if (RightBottomRelativePanel.Visibility == Visibility.Collapsed)
            {
                RightBottomRelativePanel.Visibility = Visibility.Visible;
            }
        }
    }
}
