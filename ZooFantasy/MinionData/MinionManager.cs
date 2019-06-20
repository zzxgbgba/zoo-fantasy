using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ZooFantasy.BattlefieldData;
using ZooFantasy.EffectData;

namespace ZooFantasy.MinionData
{
    class MinionManager
    {
        public static void MinionAttack(Minion attacker,Minion defender)
        {
            defender.TakeDamage(attacker.CurrentAttack);
            attacker.TakeDamage(defender.CurrentAttack);
            attacker.ChangeCanAttack(false);
        }
        public static void MinionAttack(Grid attacker, Grid defender)
        {
            if (defender.DataContext.GetType() == typeof(Minion)) 
            {
                (defender.DataContext as Minion).TakeDamage((attacker.DataContext as Minion).CurrentAttack);
                (attacker.DataContext as Minion).TakeDamage((defender.DataContext as Minion).CurrentAttack);
                (attacker.DataContext as Minion).ChangeCanAttack(false);
            }
            if (defender.DataContext.GetType() == typeof(EnemyHeroData))
            {
                (defender.DataContext as EnemyHeroData).TakeDamage((attacker.DataContext as Minion).CurrentAttack);
                (attacker.DataContext as Minion).ChangeCanAttack(false);
            }
        }
        public static void AddMinionToGridView(GridView gridView, Minion minion)
        {
            for (int i = 0; i < minion.Effects.Count; i++) //冲锋检查
            {
                if(minion.Effects[i].Category == EffectCategory.Charge)
                {
                    minion.ChangeCanAttack(true);
                }
            }
            Grid cardPanel = new Grid()
            {
                Name = minion.OriginMinionCard.Name,
                Height = 120,
                Width = 120,
                Margin = new Thickness(5, 5, 0, 0),
                DataContext = minion,
                IsRightTapEnabled = true,
                AllowDrop = true,
                CanDrag = true
            };
            Image cardImage = new Image()
            {
                Source = new BitmapImage(new Uri(minion.OriginMinionCard.ImagePath)),
                VerticalAlignment = VerticalAlignment.Top
            };
            TextBlock cardAttack = new TextBlock()
            {
                Text = minion.CurrentAttack.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 26,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            TextBlock cardHealth = new TextBlock()
            {
                Text = minion.CurrentHealth.ToString(),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 26,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
            cardPanel.Children.Add(cardImage);
            cardPanel.Children.Add(cardAttack);
            cardPanel.Children.Add(cardHealth);
            gridView.Items.Add(cardPanel);
        }


    }
}
