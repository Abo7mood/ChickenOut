using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class SuperBurgerPickup : ItemPickup
    {
        SuperBurgerSpawner burgerSpawner;

        public void Setup(SuperBurgerSpawner burgerSpawner)
        {
            this.burgerSpawner = burgerSpawner;

            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            spriteRenderer.sprite = ItemSprite;
            animator.runtimeAnimatorController = item.animator;
            gameObject.SetActive(false);
        }
    }
}
