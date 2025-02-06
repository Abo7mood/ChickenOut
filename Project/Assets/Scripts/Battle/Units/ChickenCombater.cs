using UnityEngine;
using UnityEngine.InputSystem;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Combatings.Offence;

namespace ChickenOut.Battle.Chickens
{
    public class ChickenCombater : Combater
    {
        #region Stuff
        protected const string ROCK = "Rock", PAPER = "Paper", SCISSORS = "Scissors", AIM = "Aim", SHOOT = "Shoot", SWITCH = "Switch";
        protected const string KEYBOARD = "Keyboard", GAMEPAD = "Gamepad";

        protected PlayerInput _input;
        protected InputAction _rockAction, _paperAction, _scissorsAction;
        protected InputAction _aimAction, _shootAction, _switchAction;

        protected ItemPicker _itemPicker;
        protected ChickenInfo _myInfo;
        Camera cam;

        protected bool _shouldShoot;
        #endregion

        #region Starting
        protected override void Awake()
        {
            _itemPicker = GetComponent<ItemPicker>();
            cam = Camera.main;

            base.Awake();
        }
        protected override void Start()
        {
            _shouldShoot = false;
            ChangeCombating((Combating)Random.Range(0, 3));

            myAttacks.Clear();
            myExplosions.Clear();

            base.Start();
        }

        public void DoControls(PlayerInput input)
        {
            _input = input;
            //_switchAction = input.actions[SWITCH];

            _rockAction = input.actions[ROCK];
            _paperAction = input.actions[PAPER];
            _scissorsAction = input.actions[SCISSORS];

            _aimAction = input.actions[AIM];
            _shootAction = input.actions[SHOOT];

            _rockAction.started += ctx => ChangeCombating(Combating.Rock);
            _paperAction.started += ctx => ChangeCombating(Combating.Paper);
            _scissorsAction.started += ctx => ChangeCombating(Combating.Scissors);
        }

        public void GetInfo(ChickenInfo info)
        {
            _myInfo = info;
            myTeam = info.team;
        }

        //void SwitchControls(string newControls)
        //{
        //    switch (newControls)
        //    {
        //        case GAMEPAD:

        //            break;

        //        case KEYBOARD:

        //            break;
        //        default:
        //            return;
        //    }
        //    _input.SwitchCurrentControlScheme(newControls);
        //}
        #endregion

        #region Shooting
        protected void CheckShooting()
        {
            if (_gun == null || _myInfo == null)
                return;

            Vector2 direction = _aimAction.ReadValue<Vector2>();
            switch (_input.currentControlScheme)
            {
                case GAMEPAD:
                    CheckShootingGamepad(direction);
                    break;

                case KEYBOARD:
                    CheckShootingKeyboard(direction);
                    break;
            }
        }

        void CheckShootingGamepad(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                DoRotation(direction);
                ShootOnHold();
            }

            else if (CanShootOnRelease)
                ShootOnRelease();
        }
        void CheckShootingKeyboard(Vector2 direction)
        {
            DoRotation(cam.ScreenToWorldPoint(direction) - transform.position);

            if (_shootAction.phase == InputActionPhase.Performed)
                ShootOnHold();

            else if (CanShootOnRelease)
                ShootOnRelease();
        }

        #region Shooting
        bool CanShootOnRelease => (_myInfo.shootOnRelease || _gun.ShootOnlyOnRelease) && _shouldShoot;
        bool CanShootOnHold => (!_myInfo.shootOnRelease || _gun.ShootOnlyOnHold);
        void ShootOnHold()
        {
            if (!_shouldShoot)
                _shouldShoot = true;

            if (CanShootOnHold)
                CheckAttacking();
        }
        void ShootOnRelease()
        {
            CheckAttacking();
            _shouldShoot = false;
        }
        #endregion
        #endregion

        #region Pickup

        #region BurgerBullet
        protected void BurgerBullet(float duration)
        {
            ChangeCombating(Combating.Super);
            Invoke(nameof(ResetCombat), duration);
        }
        void ResetCombat() => ChangeCombating(_lastCombat);
        #endregion

        #region FireRateBoost
        protected void FireRateBoost(float duration)
        {
            _gun = _fightManager[(int)GunType.MiniGun];
            Invoke(nameof(ResetGun), duration);
            firePS.Play();
        }
        public void ResetGun() => _gun = _fightManager[_myInfo.gunIndex];
        #endregion

        #region Power
        protected void DoublePower(float duration)
        {
            _damageMultiplier *= 2;
            Invoke(nameof(ResetDamage), duration);
        }
        void ResetDamage() => _damageMultiplier /= 2;
        #endregion

        #region StopTime
        protected void StopTime(float duration)
        {
            if (myAttacks != null && myAttacks.Count > 0)
                foreach (Attack attack in myAttacks)
                    attack.StopTime(duration);

            if (myExplosions != null && myExplosions.Count > 0)
                foreach (Explosion explosion in myExplosions)
                    explosion.StopTime(duration);
        }
        #endregion

        #endregion
    }
}