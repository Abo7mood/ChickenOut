using UnityEngine;

namespace ChickenOut.Battle
{
    [CreateAssetMenu(menuName = "Mine/Gun", fileName = "New gun")]
    public class Gun : ScriptableObject
    {
        public string myName;
        public GunType type;

        [HideInInspector] public float Angle => angle;
        [SerializeField] int angle = 0;

        [HideInInspector] public float Damage => damage;
        [SerializeField] float damage = 1;

        [HideInInspector] public float AttackSpeed => attackSpeed;
        [SerializeField] float attackSpeed = .1f;

        [HideInInspector] public int AttacksPerWave => attacksPerWave;
        [SerializeField] int attacksPerWave = 3;

        [HideInInspector] public float WaveDelay => waveDelay;
        [SerializeField] float waveDelay = .7f;

        [HideInInspector] public float AttackRateDelay => attackRateDelay;
        [SerializeField] float attackRateDelay = .3f;


        [HideInInspector] public bool ShootOnlyOnRelease => shootOnRelease && !shootOnHold;
        [SerializeField] bool shootOnRelease = true;

        [HideInInspector] public bool ShootOnlyOnHold => shootOnHold && !shootOnRelease;
        [SerializeField] bool shootOnHold = true;
    }
}