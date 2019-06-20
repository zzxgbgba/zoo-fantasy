using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;

namespace ZooFantasy.EffectData
{
    class EffectRequirement
    {
        /// <summary>
        /// 英雄的生命值大于
        /// </summary>
        /// <param name="value">格式:"int" </param>
        public static bool HeroHealthMoreThan(Grid PlayerHeroGrid, string value) 
        {
            int num = int.Parse(value);

            if ((PlayerHeroGrid.DataContext as PlayerHeroData).Health > num)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
