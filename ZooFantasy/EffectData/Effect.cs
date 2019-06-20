using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.EffectData
{
    public class Effect
    {
        private List<string> requirements;
        public List<string> Requirements { get => requirements; }
        private List<string> requirementValues;
        public List<string> RequirementValues { get => requirementValues; }
        private List<string> results;
        public List<string> Results { get => results; }
        private List<string> resultValues;
        public List<string> ResultValues { get => resultValues; }
        private List<string> aimRequirements;
        public List<string> AimRequirements { get => aimRequirements; }
        private List<string> aimRequirementValues;
        public List<string> AimRequirementValues { get => aimRequirementValues; }
        private EffectCategory category;
        public EffectCategory Category { get => category; }
        private EffectAim effectAim;
        public EffectAim EffectAim { get => effectAim; }

        public Effect()
        {
            requirements = new List<string>();
            results = new List<string>();
            requirementValues = new List<string>();
            resultValues = new List<string>();
            aimRequirements = new List<string>();
            aimRequirementValues = new List<string>();
        }
        public Effect(string EffectCategoryName,bool PlayerMinionAim,bool EnemyMinionAim,bool PlayerHeroAim,bool EnemyHeroAim)
        {
            requirements = new List<string>();
            results = new List<string>();
            requirementValues = new List<string>();
            resultValues = new List<string>();
            aimRequirements = new List<string>();
            aimRequirementValues = new List<string>();
            effectAim = new EffectAim(PlayerMinionAim, EnemyMinionAim, PlayerHeroAim, EnemyHeroAim);
            category = (EffectCategory)Enum.Parse(typeof(EffectCategory), EffectCategoryName);
        }

        public void AddRequirement(string requirement)
        {
            requirements.Add(requirement);
        }
        public void AddResult(string result)
        {
            results.Add(result);
        }
        public void AddRequirementValue(string requirementValue)
        {
            requirementValues.Add(requirementValue);
        }
        public void AddResultValue(string resultValue)
        {
            resultValues.Add(resultValue);
        }
        public void AddAimRequirement(string aimRequiremet)
        {
            AimRequirements.Add(aimRequiremet);
        }
        public void AddAimRequirementValue(string aimRequiremetValue)
        {
            AimRequirementValues.Add(aimRequiremetValue);
        }
    }
    public enum EffectCategory
    {
        Battlecry,
        Charge,
        Deathrattle,

        Spell,
    }
}
