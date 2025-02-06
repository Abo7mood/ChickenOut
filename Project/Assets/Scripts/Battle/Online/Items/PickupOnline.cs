using UnityEngine;

namespace ChickenOut.Battle.Online.Items
{
    public class PickupOnline : MonoBehaviour
    {
        const string collectName = "Collect";

        [HideInInspector] public Item Item => _item;
        [SerializeField] protected Item _item;

        SpriteRenderer _spriteRenderer;
        Collider2D _myCollider;
        Animator _animator;

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _myCollider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }
        protected virtual void Start() => gameObject.SetActive(false);

        protected void Setup()
        {
            _spriteRenderer.sprite = Item.sprite;
            _animator.runtimeAnimatorController = Item.animator;
        }

        protected void Collect()
        {
            TurnOnAndOff(false);
            _animator.SetTrigger(collectName);

            Invoke(nameof(TurnMeOff), .5f);
        }
        protected void TurnMeOff() => gameObject.SetActive(false);

        protected void Respawn()
        {
            TurnOnAndOff(true);
            gameObject.SetActive(true);
        }

        void TurnOnAndOff(bool nm)
        {
            _spriteRenderer.enabled = nm;
            _myCollider.enabled = nm;
        }
    }
}