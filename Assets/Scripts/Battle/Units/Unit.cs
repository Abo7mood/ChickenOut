using UnityEngine;
using UnityEngine.InputSystem;

namespace ChickenOut.Battle
{
    public class Unit : MonoBehaviour
    {
        protected PlayerInput input;
        [HideInInspector] public Rigidbody2D rigidBody;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public Animator animator;

        protected int HPMax = 10;
        protected float HP;
        public bool IsDead { get { if (HP <= 0) return true; else return false; } }

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            input = GetComponent<PlayerInput>();
        }
        protected virtual void Start()
        {
            HP = HPMax;

            rigidBody.sharedMaterial = new PhysicsMaterial2D
            {
                friction = 0,
                bounciness = 0
            };
        }

        public virtual void Setup()
        {

        }

        public virtual void TakeDamage(float dmg, int index)
        {
            HP = Mathf.Clamp(HP - dmg, 0, HPMax);
            if (IsDead)
                Die();
        }

        public virtual void Die()
        {
            
        }
        public virtual void Undie()
        {
            
        }
    }
}