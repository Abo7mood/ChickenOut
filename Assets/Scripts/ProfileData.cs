using System;

namespace ChickenOut
{
    [Serializable]
    public class ProfileData
    {
        public string userName;
        public RSControls rightControls;
        public LSControls leftControls;

        public ProfileData()
        {
            userName = "Player_0";
            rightControls = RSControls.ThreeJoysticks;
            leftControls = LSControls.Arrows;
        }

        public ProfileData(string username, RSControls rsControls, LSControls lsControls)
        {
            userName = username;
            rightControls = rsControls;
            leftControls = lsControls;
        }
    }
}