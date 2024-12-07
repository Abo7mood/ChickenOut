using ChickenOut.Battle.Online.Chickens;

namespace ChickenOut.Battle.Online.Combatings.Defence
{
    public class Shield : Battle.Combatings.Defence.Shield
    {
        Chicken _chicken;

        public void Setup(Chicken chicken)
        {
            _chicken = chicken;
        }

        public override void Defend(Combating attacker, float dmg, int index) => 
            _chicken.TakeDamage(_combat.GetCombatResault(attacker).CalculateDamage(dmg), index);
    }
}