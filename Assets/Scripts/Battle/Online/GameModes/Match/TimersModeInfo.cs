using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Match
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Match/TimersModeInfo", fileName = "New TimersModeInfo")]
    public class TimersModeInfo : ScriptableObject
    {
        #region Properities
        public float WaitingTimer => _waitingTime_M * 60;
        public float MatchTimer => _matchTime_M * 60;
        #endregion

        [Header("In minutes")]
        [Min(.5f)] [SerializeField] float _waitingTime_M = .5f;
        [Min(1)] [SerializeField] float _matchTime_M = 1;

        [Header("In seconds")]
        [Min(1)] public float preparingTime_S = 1;
        [Min(5)] public float choosingTime_S = 5;
        [Min(5)] public float endTime_S = 5;
        [Min(1)] public float respawnTime_S = 1;
    }
}