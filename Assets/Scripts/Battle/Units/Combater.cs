using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Combatings.Offence;
using ChickenOut.Battle.Combatings.Defence;

namespace ChickenOut.Battle.Chickens
{
    public class Combater : MonoBehaviour
    {
        #region Stuff
        protected List<Attack> myAttacks = new List<Attack>();
        protected List<Explosion> myExplosions = new List<Explosion>();

        #region Events
        protected Action<Combating, float, float, float> onAttackingCalled;
        public Action<Combating> onChangeCombatingCalled;
        protected Action<float> onDoGunCooldownCalled;
        #endregion

        protected Combating _combat;
        protected Combating _lastCombat;

        protected float _shootingDelayTimer;
        protected float _superBulletDelay = .5f, _superBulletSpeed = .2f;

        protected int _damageMultiplier = 1;
        protected Team myTeam;
        protected Vector2 _direction;

        public Transform gunAnchor;
        CombaterImage _gunImage;
        protected Gun _gun;

        protected float[] _gunPosition => _gunImage.transform.position.ToFloats();
        protected float _gunRotaion => gunAnchor.rotation.ToFloat();

        protected FightManager _fightManager;
        protected Shield _shield;

        protected ParticleSystem firePS;
        #endregion

        #region Starting
        protected virtual void Awake()
        {
            _gunImage = gunAnchor.GetComponentInChildren<CombaterImage>();
            firePS = gunAnchor.GetComponentInChildren<ParticleSystem>();
        }
        protected virtual void Start()
        {

        }
        #endregion

        #region Rotating
        protected virtual void Update()
        {
            if (_gun != null)
                _gunImage.UpdateImage(1 - Mathf.Clamp01(ShootingDelay / _gun.WaveDelay));
        }
        protected void DoRotation(Vector2 vector)
        {
            _direction = vector;
            float angle = vector.GetRadAngle();

            gunAnchor.eulerAngles = new Vector3(0, 0, angle);
            _gunImage.transform.localScale = new Vector3(1f, angle.GetYScale(), 1f);
        }
        #endregion

        #region Combating
        protected void ChangeCombating(Combating combat)
        {
            if (combat != Combating.Super)
                _lastCombat = combat;
            if (_combat != Combating.Super)
                _combat = combat;

            onChangeCombatingCalled?.Invoke(_combat);

            if (combat == Combating.Super)
                onDoGunCooldownCalled?.Invoke(_superBulletDelay);
        }
        protected void UpdateCombating(Combating combat)
        {
            _gunImage.UpdateImage((int)combat);

            if (_shield != null && combat != Combating.Super)
                _shield.ChangeCombating((int)combat);
        }
        #endregion

        #region Shooting
        public void CheckAttacking()
        {
            if (!CanShoot)
                return;

            if (_combat == Combating.Super)
            {
                onAttackingCalled?.Invoke(Combating.Super, 100f * _damageMultiplier, _superBulletSpeed, 0);
                onDoGunCooldownCalled?.Invoke(_gun.WaveDelay);

                _combat = _lastCombat;
                ChangeCombating(_lastCombat);
                onChangeCombatingCalled(_combat);
            }
            else
            {
                onDoGunCooldownCalled?.Invoke(_gun.WaveDelay);

                if (_gun.Angle > 0)
                    AttackingWithAngle();
                else
                    StartCoroutine(Attacking());
            }
        }

        void AttackingWithAngle()
        {
            for (int i = 0; i < _gun.AttacksPerWave; i++)
            {
                onAttackingCalled?.Invoke(_lastCombat, _gun.Damage * _damageMultiplier, _gun.AttackSpeed, _gun.Angle * i);
                if (i > 0)
                    onAttackingCalled?.Invoke(_lastCombat, _gun.Damage * _damageMultiplier, _gun.AttackSpeed, -_gun.Angle * i);
            }
        }
        protected IEnumerator Attacking()
        {
            for (int i = 0; i < _gun.AttacksPerWave; i++)
            {
                onAttackingCalled?.Invoke(_lastCombat, _gun.Damage * _damageMultiplier, _gun.AttackSpeed, 0);

                if (i < _gun.AttacksPerWave - 1)
                    yield return new WaitForSeconds(_gun.AttackRateDelay);
            }
        }

        protected void ShootAttack(bool canDamage, int index, int combat, float dmg, float speed, float[] position, float zRotation)
        {
            Attack attack = _fightManager.GetAttack;
            attack.Setup(this, myTeam, canDamage, index, dmg, speed);
            attack.Shoot(combat, position.ToVector2(), zRotation);

            myAttacks.Add(attack);
        }
        protected void ShootAttack(Explosion explosion, bool canDamage, int index, int combat, float dmg, float speed, float[] position, float zRotation)
        {
            Attack attack = _fightManager.GetAttack;
            attack.Setup(this, explosion, myTeam, canDamage, index, dmg, speed);
            attack.Shoot(combat, position.ToVector2(), zRotation);

            myAttacks.Add(attack);
            myExplosions.Add(explosion);
        }        
        #endregion

        #region Remove
        public void RemoveMe(Attack attack) => myAttacks.Remove(attack);
        public void RemoveMe(Explosion explosion) => myExplosions.Remove(explosion);
        #endregion

        #region Cooldowns and timers
        protected void DoGunCooldown(float delay) => _shootingDelayTimer = delay + Time.time;
        protected bool CanShoot { get { return ShootingDelay == 0; } }
        protected float ShootingDelay { get { return Mathf.Clamp(_shootingDelayTimer - Time.time, 0, 10000); } }
        #endregion
    }
}