using UnityEngine;

namespace ChickenOut.Battle
{
    public static class CombatingExtensions
    {
        public static Color GetColor(this Combating combating) => combating switch
        {
            Combating.Rock => Color.cyan,
            Combating.Paper => Color.yellow,
            Combating.Scissors => Color.magenta,
            _ => Color.white,
        };
        public static Color GetColor(this Team team) => team switch
        {
            Team.Team1 => new Vector4(1f, .5f, 0f, 1f),
            Team.Team2 => new Vector4(0f, .5f, 1f, 1f),
            _ => Color.white,
        };

        public static CombatResault GetCombatResault(this Combating defender, Combating attacker) => attacker switch
        {
            Combating.Rock => defender.AgainstRock(),
            Combating.Paper => defender.AgainstPaper(),
            Combating.Scissors => defender.AgainstScissors(),
            Combating.Super => CombatResault.Lose,

            _ => CombatResault.Tie,
        };

        public static CombatResault AgainstRock(this Combating defender) => defender switch
        {
            Combating.Rock => CombatResault.Tie,
            Combating.Paper => CombatResault.Win,
            Combating.Scissors => CombatResault.Lose,
            Combating.Super => CombatResault.Win,

            _ => CombatResault.Tie,
        };
        public static CombatResault AgainstPaper(this Combating defender) => defender switch
        {
            Combating.Rock => CombatResault.Lose,
            Combating.Paper => CombatResault.Tie,
            Combating.Scissors => CombatResault.Win,
            Combating.Super => CombatResault.Win,

            _ => CombatResault.Tie,
        };
        public static CombatResault AgainstScissors(this Combating defender) => defender switch
        {
            Combating.Rock => CombatResault.Win,
            Combating.Paper => CombatResault.Lose,
            Combating.Scissors => CombatResault.Tie,
            Combating.Super => CombatResault.Win,

            _ => CombatResault.Tie,
        };

        public static float CalculateDamage(this CombatResault resault, float dmg) => resault switch
        {
            CombatResault.Win => dmg * 0f,
            CombatResault.Tie => dmg * 1f,
            CombatResault.Lose => dmg * 2f,
            _ => dmg,
        };
    }
}