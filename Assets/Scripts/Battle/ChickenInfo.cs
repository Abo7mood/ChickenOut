using UnityEngine;

namespace ChickenOut.Battle
{
    [CreateAssetMenu(menuName = "Mine/ChickenInfo", fileName = "New ChickenInfo")]
    public class ChickenInfo : ScriptableObject
    {
        [HideInInspector] public int Index => index;
        [SerializeField] int index;
        public string myName;

        public Team team;
        public RSControls rightControls = RSControls.OneJoystick;
        public LSControls leftControls = LSControls.Arrows;

        public int score;
        
        public int gunIndex;
        public bool shootOnRelease;

        public int order;
        public bool isWinner;
    }
}
