using UnityEngine;
using ChickenOut.Battle.Chickens;
using ChickenOut.Battle.Offline.Managers;
using ChickenOut.Battle.Offline.Combatings.Defence;

namespace ChickenOut.Battle.Offline.Enemies
{
    public class Turret : Combater
    {
        [SerializeField] Gun __gun;

        protected override void Awake()
        {
            base.Awake();

            _shield = GetComponentInChildren<Shield>();
            _gun = __gun;
        }

        protected override void Start()
        {
            base.Start();

            _fightManager = FightManagerOffline.instance;

            onChangeCombatingCalled += UpdateCombating;
            onDoGunCooldownCalled += DoGunCooldown;
            onAttackingCalled += Attacking;
        }

        public void TurretAttaking()
        {
            ChangeCombating((Combating)Random.Range(0, 3));
            onDoGunCooldownCalled?.Invoke(_gun.WaveDelay);
            StartCoroutine(Attacking());
        }

        void Attacking(Combating combat, float dmg, float speed, float angle = 0) =>
            ShootAttack(true, -1, (int)combat, dmg, speed, _gunPosition, _gunRotaion + angle);
    }
}