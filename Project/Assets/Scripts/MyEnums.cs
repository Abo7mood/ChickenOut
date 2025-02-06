namespace ChickenOut
{
    public enum Team
    {
        Team1,
        Team2,
    }

    #region Controls
    public enum RSControls
    {
        ThreeJoysticks,
        OneJoystick,
    }
    public enum LSControls
    {
        Joystick,
        Arrows,
    }
    #endregion

    #region Particles
    public enum ChickenParticles
    {
        Feathers,
        Starts,

        Jump,
        Land,
        Dash,
    }
    #endregion

    #region Game states
    public enum GameState
    {
        waiting,
        Choosing,
        startGame,
        endGame,
    }
    public enum EndStates
    {
        MaxScoreReached,
        LastManStanding,
        TimeEnded,
    }
    #endregion
}