using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZooFantasy.BattlefieldData;
using ZooFantasy.MinionData;

namespace ZooFantasy.EffectData
{
    class EffectAimRequirement
    {
        /// <summary>
        /// 目标为未受伤的单位
        /// </summary>
        /// <param name="value">格式:"" </param>
        public static bool AimUndamaged(object obj, string value)
        {
            if (obj.GetType() == typeof(Minion))
            {
                Minion m = obj as Minion;
                if (m.CurrentHealth >= m.OriginMinionCard.Health)
                    return true;
                else
                    return false;
            }
            if(obj.GetType()==typeof(PlayerHeroData))
            {
                PlayerHeroData h = obj as PlayerHeroData;
                if (h.Health == 30)
                    return true;
                else
                    return false;
            }
            if (obj.GetType() == typeof(EnemyHeroData))
            {
                EnemyHeroData h = obj as EnemyHeroData;
                if (h.Health == 30)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}
