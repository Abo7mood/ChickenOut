namespace ChickenOut.Battle
{
    public interface IOffenceCombat : ICombat
    {

    }

    public interface IDefenceCombat : ICombat
    {
        public void Defend(Combating attacker, float dmg, int index);
    }

    public interface ICombat
    {

    }
}