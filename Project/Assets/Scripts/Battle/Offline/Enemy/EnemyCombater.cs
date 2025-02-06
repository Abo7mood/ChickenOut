using UnityEngine;
using ChickenOut.Battle.Chickens;
using ChickenOut.Battle.Offline.Chickens;
using ChickenOut.Battle.Offline.Managers;
using ChickenOut.Battle.Offline.Combatings.Defence;

namespace ChickenOut.Battle.Offline.Enemies
{
    public class EnemyCombater : Combater
    {
        Enemy _enemy;
        Chickens.Chicken _chicken;
        [SerializeField] Gun __gun;

        [SerializeField] LayerMask _mask;

        Vector2 Direction => _chicken.transform.position - transform.position;
        float Distance => Vector2.Distance(transform.position, _chicken.transform.position);

        protected override void Awake()
        {
            base.Awake();

            _chicken = FindObjectOfType<Chickens.Chicken>();

            _enemy = GetComponent<Enemy>();
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

        protected override void Update()
        {
            base.Update();

            DoRotation(Direction);

            if (ShouldShoot())
            {
                ChangeCombating((Combating)Random.Range(0, 3));
                onDoGunCooldownCalled?.Invoke(_gun.WaveDelay);
                StartCoroutine(Attacking());
            }
        }
        public bool ShouldShoot()
        {
            return CanShoot && DidTouchChicken();

            bool DidTouchChicken() => Physics2D.Raycast(transform.position, Direction.normalized, Distance, _mask)
                .collider.GetComponent<Chickens.Chicken>() != null;
        }

        void Attacking(Combating combat, float dmg, float speed, float angle = 0) =>
            ShootAttack(true, -1, (int)combat, dmg, speed, _gunPosition, _gunRotaion + angle);
    }
}