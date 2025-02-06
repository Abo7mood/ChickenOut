using UnityEngine;
using UnityEngine.UI;

namespace ChickenOut.Battle
{
    public class CooldownTimer : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Image[] timerCircles;

        float chargeTime;
        float timer;

        float TimeLeftPercentage { get { return 1 - Mathf.Clamp01(TimeLeft / chargeTime); } }
        float TimeLeft { get { return Mathf.Clamp(timer - Time.time, 0, 100); } }
        bool IsDone { get { if (TimeLeft == 0) return true; else return false; } }

        public void Setup(Sprite sprite, float maxTime)
        {
            image.sprite = sprite;
            chargeTime = maxTime;

            foreach (Image image in timerCircles)
                image.fillAmount = 1f;
            timer = chargeTime + Time.time;
        }

        void Update()
        {
            foreach (Image image in timerCircles)
                image.fillAmount = 1f - TimeLeftPercentage;

            if (IsDone)
                Destroy(gameObject);
        }
    }
}
