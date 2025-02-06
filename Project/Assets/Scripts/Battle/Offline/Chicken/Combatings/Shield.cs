using ChickenOut.Battle.Offline.Chickens;

namespace ChickenOut.Battle.Offline.Combatings.Defence
{
    public class Shield : Battle.Combatings.Defence.Shield
    {
        Chickens.Chicken _chicken;

        public void Setup(Chickens.Chicken chicken) => _chicken = chicken;

        public override int GetIndex()
        {
            myIndex = _chicken.info.Index;
            return base.GetIndex();
        }

        public override void Defend(Combating attacker, float dmg, int index) =>
            _chicken.TakeDamage(_combat.GetCombatResault(attacker).CalculateDamage(dmg), index);
    }
}