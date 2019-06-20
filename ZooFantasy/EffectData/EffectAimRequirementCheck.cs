using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.EffectData
{
    class EffectAimRequirementCheck
    {
        public static bool AimRequirementCheck(object aim, string aimRequirement, string value)
        {
            switch (aimRequirement)
            {
                case "AimUndamaged":
                    {
                        return EffectAimRequirement.AimUndamaged(aim, value);
                        break;
                    }
            }
            return false;
        }
        public static bool AimRequirementsCheck(object aim, List<string> aimRequirements, List<string> values)
        {
            for (int i = 0; i < aimRequirements.Count; i++)
            {
                if (!AimRequirementCheck(aim, aimRequirements[i], values[i])) return false;
            }
            return true;
        }
        public static bool AimRequirementsCheck(object aim, Effect effect)
        {
            return AimRequirementsCheck(aim, effect.AimRequirements, effect.AimRequirementValues);
        }

    }
}
