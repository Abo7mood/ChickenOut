using UnityEngine;
using ChickenOut.Battle.Offline.Managers;
using ChickenOut.Battle.Offline.Combatings.Defence;

namespace ChickenOut.Battle.Offline.Chickens
{
    public class ChickenCombater : Battle.Chickens.ChickenCombater
    {
        [SerializeField] Gun __gun;
        [SerializeField] float __burgerBulletDelay = .5f, __burgerBulletSpeed = .2f;

        [SerializeField] int __damageMultiplier = 1;

        protected override void Awake()
        {
            _shield = GetComponentInChildren<Shield>();

            _gun = __gun;

            _superBulletDelay = __burgerBulletDelay;
            _superBulletSpeed = __burgerBulletSpeed;

            _damageMultiplier = __damageMultiplier;

            base.Awake();
        }

        protected override void Start()
        {
            _fightManager = FightManagerOffline.instance;
            _myInfo = GetComponent<Chicken>().info;

            onChangeCombatingCalled += UpdateCombating;
            onDoGunCooldownCalled += DoGunCooldown;
            onAttackingCalled += Attacking;

            _itemPicker.onFireRateBoostCollected += FireRateBoost;
            _itemPicker.onSuperBurgerCollected += BurgerBullet;
            _itemPicker.onPowerCollected += DoublePower;

            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            CheckShooting();
        }

        void Attacking(Combating combat, float dmg, float speed, float angle = 0) =>
            ShootAttack(true, _myInfo.Index, (int)combat, dmg, speed, _gunPosition, _gunRotaion + angle);
    }
}