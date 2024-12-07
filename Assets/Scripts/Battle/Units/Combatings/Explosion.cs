using System.Collections.Generic;
using UnityEngine;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle.Combatings.Offence
{
    public class Explosion : MonoBehaviour, IOffenceCombat
    {
        #region Stuff
        List<IDefenceCombat> _defenders = new List<IDefenceCombat>();

        protected float _radius, _originalGrowthSpeed, _growthSpeed;
        protected int _index;
        bool _canDamage;

        The_Timer_0 _myTimer;

        SpriteRenderer _spriteRenderer;
        SoundController _soundManager;
        Combater _myCombater;

        [SerializeField] ParticleSystem _explosionPS;
        #endregion

        #region Starting
        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defenders.Clear();
            gameObject.SetActive(false);
        }
        protected virtual void Start() => _soundManager = SoundController.instance;
        public void Setup(Combater combater, bool canDamage, int index, Vector2 positoin)
        {
            transform.position = positoin;
            _myCombater = combater;
            _canDamage = canDamage;
            _index = index;

            transform.localScale = Vector3.zero;
            _defenders.Clear();
            gameObject.SetActive(true);

            Cinemachineshake2.instance.ShakeBurgerExplosion();
            _explosionPS.Play();
        }
        #endregion

        #region Updating
        void Update()
        {
            if (_growthSpeed == 0 && _myTimer.IsReady)
                _growthSpeed = _originalGrowthSpeed;
            else if (_growthSpeed == 0 && _myTimer.IsNotReady)
                return;

            _spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, transform.localScale.x / (_radius * 1.5f)));

            if (transform.localScale.x > _radius)
            {
                Destroying();
                return;
            }

            transform.localScale += Vector3.one * (_growthSpeed * Time.deltaTime);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_canDamage)
                return;

            IDefenceCombat defender = collision.GetComponent<IDefenceCombat>();
            if (defender != null && !_defenders.Contains(defender))
            {
                _defenders.Add(defender);
                _soundManager.DamageSound();
                defender.Defend(Combating.Super, 100f, _index);
            }
        }
        #endregion

        #region Other
        void Destroying()
        {
            gameObject.SetActive(false);

            _myCombater.RemoveMe(this);
            _myTimer.ResetartTimer(0);
        }
        #endregion

        #region Pickup
        public void StopTime(float delay)
        {
            _myTimer.ResetartTimer(delay);
            _growthSpeed = 0;
        }
        #endregion
    }
}