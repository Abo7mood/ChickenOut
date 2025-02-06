using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Combatings.Defence;

namespace ChickenOut.Battle.Chickens
{
    public class Chicken : MonoBehaviour
    {
        #region Stuff
        #region Events
        public Action<float, float> onHPChanged;
        public Action onScoreChanged;
        public Action onNameChanged;

        public Action<float> onDamageTaken;
        public Action<int> onDied;

        public Action<bool> onTurning;

        public Action<Color> onTeamingUp;
        #endregion

        protected float _maxHP, _currentHP;

        protected SoundController _soundManager;
        protected ChickenInfo _myInfo;
        protected PlayerInput _input;
        BoxCollider2D _boxCollider;

        protected ChickenCombater _combater;
        protected ChickenMovement _movement;

        protected TMP_Text _playerNameText;

        [SerializeField] protected GameObject _cookedChicken, _jackBox, _jackBoxSound, _myPointer;

        [HideInInspector] public Animator _animator;
        protected Shield _shield;

        public bool IsDead => _currentHP <= 0;

        #endregion

        #region Starting
        protected virtual void Awake()
        {
            _playerNameText = GetComponentInChildren<TMP_Text>();
            _animator = GetComponentInChildren<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _input = GetComponent<PlayerInput>();
        }
        protected virtual void Start() => _soundManager = SoundController.instance;
        #endregion

        #region Damage and Score
        public void TakeDamage(float damage, int index)
        {
            if (IsDead)
                return;

            _currentHP = Mathf.Clamp(_currentHP - damage, 0, _maxHP);

            if (damage > 0)
                onDamageTaken?.Invoke(_currentHP);

            if (IsDead)
                onDied?.Invoke(index);
        }
        protected void BeDamaged(float HP)
        {
            _currentHP = HP;
            UpdateHP();

            _animator.SetTrigger("Hit");
            _soundManager.DamageSound();
        }

        protected void UpdateHP() => onHPChanged?.Invoke(_currentHP, _maxHP);
        #endregion

        #region Dying
        protected void Dying(bool isMine = true)
        {
            TurningOnAndOff(false, isMine);
            _shield.gameObject.SetActive(false);

            Invoke(nameof(TurnMeOff), 2f);
        }

        void TurnMeOff() => gameObject.SetActive(false);
        void TurnMyShieldOn() => _shield.gameObject.SetActive(true);

        protected void UnDying(bool isMine = true)
        {
            _currentHP = _maxHP;
            UpdateHP();
            TurningOnAndOff(true, isMine);

            gameObject.SetActive(true);
            Invoke(nameof(TurnMyShieldOn), 2f);
        }

        void TurningOnAndOff(bool on_off, bool isMine)
        {
            _boxCollider.enabled = on_off;
            _movement.enabled = on_off;

            onTurning?.Invoke(on_off);

            _cookedChicken.SetActive(!on_off);

            Shared(on_off, isMine);
        }

        #endregion

        #region Pickup

        #region Heal
        protected void Heal(float duration)
        {
            _currentHP += (1 / 5) * _maxHP;
            UpdateHP();
        }
        #endregion

        #region Box
        protected void GetInBox(float duration)
        {
            InAndOut(false);
            StartCoroutine(GetOutOfBox(duration));
        }
        protected void GetInBox(float duration, bool isMine)
        {
            InAndOut(false, isMine);
            StartCoroutine(GetOutOfBox(duration, isMine));
        }

        IEnumerator GetOutOfBox(float duration, bool isMine = true)
        {
            yield return new WaitForSeconds(duration);
            InAndOut(true, isMine);
        }

        void InAndOut(bool on_off, bool isMine = true)
        {
            Shared(on_off, isMine);

            onTurning?.Invoke(on_off);

            _jackBox.SetActive(!on_off);
            _jackBoxSound.SetActive(!on_off);
        }
        #endregion

        #endregion
        void Shared(bool on_off, bool isMine)
        {
            if (isMine)
                _input.enabled = on_off;
            _combater.gunAnchor.gameObject.SetActive(on_off);
        }
    }
}