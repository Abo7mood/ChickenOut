using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        const string collectName = "Collect";

        public Item item;

        public Sprite ItemSprite => item.sprite;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;

        public void Setup(Item item)
        {
            this.item = item;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            spriteRenderer.sprite = ItemSprite;
            animator.runtimeAnimatorController = item.animator;
        }

        public virtual void Interact()
        {
            animator.SetTrigger(collectName);
            gameObject.SetActive(false);
        }
    }
}