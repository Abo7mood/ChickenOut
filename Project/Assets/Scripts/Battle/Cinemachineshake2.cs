using UnityEngine;
using Cinemachine;

namespace ChickenOut.Battle
{    public class Cinemachineshake2 : MonoBehaviour
    {
        public static Cinemachineshake2 instance { get; private set; }
        public static CinemachineVirtualCamera virtualCamera { get; private set; }
        CinemachineBasicMultiChannelPerlin cvirtual;

        [SerializeField] float shakeTimer;
        [SerializeField] float shakeTimerTotal;
        [SerializeField] float startinginstatntiy;

        void Awake()
        {

            instance = this;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            cvirtual = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        void Start()
        {
            cvirtual.m_AmplitudeGain = 0;
        }
        public void Shaker2(float instantity, float time)
        {
            cvirtual.m_AmplitudeGain = instantity;
            startinginstatntiy = instantity;
            shakeTimer = time;
            shakeTimerTotal = time;
        }

        void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                cvirtual.m_AmplitudeGain = Mathf.Lerp(startinginstatntiy, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
            if (cvirtual.m_AmplitudeGain > 0 && cvirtual.m_AmplitudeGain < 1)
                cvirtual.m_AmplitudeGain = 0;
        }

        public void ShakeBurgerRespawn() => Shaker2(2f, 1.5f);
        public void ShakeBurgerExplosion() => Shaker2(4f, 1.5f);
    }
}
