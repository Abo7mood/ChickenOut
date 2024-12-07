using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChickenOut.Battle.Managers
{
    public class HUDManager : MonoBehaviour
    {
        #region Stuff
        public static HUDManager instance;

        [SerializeField] protected GameObject[] _panels;
        [SerializeField] protected GameObject _controlsPanel, _LSJoystick, _LSArrows, _RSJoystick, _RS3Joycsticks;
        [SerializeField] protected Transform _cooldownTimersHolder, _heartsHolder, _scoresHolder, _endGamePanelsHolder;

        [SerializeField] protected TMP_Text _pingText, _endGameText;
        [SerializeField] protected CooldownTimer _cooldownTimerPrefab;

        protected ChickenInfo[] _chickenInfos;
        protected ScoreHolder[] _scores;
        protected Image[] _heartSprites;
        protected SpriteChanger[] _spriteChangers;

        public Sprite _burgerSprite;
        [SerializeField] private GameObject transition;
        private Animator transitionA;

        [SerializeField] protected ParticleSystem _fireworksPS;
        #endregion

        #region Starting
        protected virtual void Awake()
        {
            _spriteChangers = _panels[2].GetComponentsInChildren<SpriteChanger>(true);
            _heartSprites = _heartsHolder.GetComponentsInChildren<Image>(true);
            _scores = _scoresHolder.GetComponentsInChildren<ScoreHolder>(true);

            transitionA = transition.GetComponent<Animator>();
        }
        protected virtual void Start()
        {
            transitionA.gameObject.SetActive(true);
            transitionA.SetTrigger("Fight");
        }
        #endregion

        public void StartTimer(Item item) => Instantiate(_cooldownTimerPrefab, _cooldownTimersHolder).Setup(item.sprite, item.duration);

        protected void UpdateHP(float HP, float HPMax)
        {
            for (int i = 0; i < _heartSprites.Length; i++)
                _heartSprites[i].fillAmount = Mathf.Clamp01((_heartSprites.Length * (HP / HPMax)) - i);
        }

        protected virtual void UpdateNames()
        {
            for (int i = 0; i < _scores.Length; i++)
                _scores[i].nameText.text = _chickenInfos[i].myName;
        }

        protected void UpdateScore()
        {
            for (int i = 0; i < _scores.Length; i++)
                _scores[i].scoreText.text = $"{_chickenInfos[i].score}";
        }

        protected void UpdateControls(RSControls rightControls, LSControls leftControls)
        {

            if (!Application.isMobilePlatform)
                return;

            _RS3Joycsticks.SetActive(rightControls == RSControls.ThreeJoysticks);
            _RSJoystick.SetActive(rightControls == RSControls.OneJoystick);

            _LSJoystick.SetActive(leftControls == LSControls.Joystick);
            _LSArrows.SetActive(leftControls == LSControls.Arrows);
        }
    }
}