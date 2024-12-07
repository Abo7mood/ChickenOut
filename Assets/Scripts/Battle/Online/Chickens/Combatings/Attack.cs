namespace ChickenOut.Battle.Online.Combatings.Offence
{
    public class Attack : Battle.Combatings.Offence.Attack
    {
        void Start()
        {
            GameManagerOnline gameManager = GameManagerOnline.instance;
            _isTeams = gameManager.mode.MyType.IsTeam;
        }
    }
}