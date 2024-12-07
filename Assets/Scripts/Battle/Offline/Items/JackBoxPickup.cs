using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class JackBoxPickup : ItemPickup
    {
        public Vector3 Target { get; set; }
        [SerializeField] float speed;

        public void Setup()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            spriteRenderer.sprite = ItemSprite;
            animator.runtimeAnimatorController = item.animator;
        }

        void Update() => transform.position += Target * speed * Time.deltaTime;

        public override void Interact()
        {
            base.Interact();
            spriteRenderer.enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}
