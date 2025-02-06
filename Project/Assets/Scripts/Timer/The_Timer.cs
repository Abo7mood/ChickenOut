using System;
using UnityEngine;

namespace ChickenOut
{
    public class The_Timer : MonoBehaviour
    {
        #region Stuff

        #region Events
        public Action onTimerRestarted;
        public Action onTimerDone;
        #endregion

        The_Timer_0 _myTimer;
        #endregion

        #region Starting
        void Awake()
        {

        }
        void Start()
        {

        }
        #endregion

        void Update()
        {
            if (_myTimer.IsReady)
                OnTimerDone();
        }
        void OnTimerDone() => onTimerDone?.Invoke();

        public void RestartTimer()
        {
            _myTimer.ResetartTimer();
            onTimerRestarted?.Invoke();
        }
        public void RestartTimer(float time)
        {
            _myTimer.ResetartTimer(time);
            onTimerRestarted?.Invoke();
        }
    }

    [Serializable]
    public struct The_Timer_0
    {
        #region Stuff

        #region Properties

        #region Different
        public float TimeLeftPercentageInverted => 1 - Mathf.Clamp01(TimeLeft / chargeTime);
        public float TimeLeftPercentage => Mathf.Clamp01(TimeLeft / chargeTime);

        public float TimeLeft => Mathf.Clamp(timer - Time.time, 0, Mathf.Infinity);

        public bool IsReady => TimeLeft == 0;
        public bool IsNotReady => !(TimeLeft == 0);
        #endregion

        #region Normal
        public float OriginalChargeTime => originalChargeTime;
        public float ChargeTime => chargeTime;
        public float MyTimer => timer;
        #endregion

        #endregion

        #region Variables
        float originalChargeTime;
        float chargeTime;
        float timer;
        #endregion

        #endregion

        #region Methods
        public void ResetartTimer() => timer = chargeTime + Time.time;
        public void ResetartTimer(float time) => timer = (chargeTime = time) + Time.time;

        public void ResetTimer() => chargeTime = originalChargeTime;

        public void SetTime(float newChargeTime) => chargeTime = newChargeTime;
        public void SetOriginalTime(float newOriginalChargeTime) => originalChargeTime = newOriginalChargeTime;

        public string ClockTimer(float time) => $"{Mathf.Floor(time / 60):00}:{Mathf.Floor(time % 60):00}";
        #endregion

        #region Other
        public The_Timer_0(float time)
        {
            originalChargeTime = time;
            chargeTime = time;
            timer = time;
        }
        #endregion
    }
}