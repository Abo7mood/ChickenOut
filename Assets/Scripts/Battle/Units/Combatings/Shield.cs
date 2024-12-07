using UnityEngine;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle.Combatings.Defence
{
    public class Shield : MonoBehaviour, IDefenceCombat
    {
        protected int myIndex;

        protected Combating _combat;
        public Team _myTeam;

        //[SerializeField] Sprite[] _sprites;
        SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            GetComponentInParent<ChickenMovement>().onDashed += Dashing;
        }

        public void ChangeCombating(int combat)
        {
            _combat = (Combating)combat;

            if (_spriteRenderer != null)
            {
                //_spriteRenderer.sprite = _sprites[combat];
                _spriteRenderer.color = _combat.GetColor();
            }
        }

        public void GetInfo(ChickenInfo info)
        {
            myIndex = info.Index;
            _myTeam = info.team;
        }

        public virtual int GetIndex() => myIndex;

        public virtual void Defend(Combating attacker, float dmg, int index) { }

        void Dashing(int direction, float delay)
        {
            gameObject.SetActive(false);
            Invoke(nameof(TurnMeOn), delay);
        }
        void TurnMeOn() => gameObject.SetActive(true);
    }
}