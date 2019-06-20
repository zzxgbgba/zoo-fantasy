namespace ZooFantasy.EffectData
{
    public struct EffectAim
    {
        /// <summary>
        /// bool PlayerMinion,bool EnemyMinion,bool PlayerHero,bool EnemyHero
        /// </summary>
        public bool playerMinion;
        public bool enemyMinion;
        public bool playerHero;
        public bool enemyHero;
        public EffectAim(bool PlayerMinion,bool EnemyMinion,bool PlayerHero,bool EnemyHero)
        {
            playerMinion = PlayerMinion;
            playerHero = PlayerHero;
            enemyMinion = EnemyMinion;
            enemyHero = EnemyHero; 
        }
    };
}
