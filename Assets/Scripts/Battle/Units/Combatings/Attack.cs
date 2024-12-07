using UnityEngine;
using ChickenOut.Battle.Combatings.Defence;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle.Combatings.Offence
{
    public class Attack : MonoBehaviour, IOffenceCombat
    {
        #region Stuff
        const int GROUND = 3;

        protected int _index;
        float _damage, _originalSpeed, _speed;

        protected bool _isTeams, _canDamage;
        bool _canMove;

        The_Timer_0 _myTimer;
        Vector3 _direction;
        Combating _combat;
        Team _myTeam;

        Combater _myCombater;
        Explosion _myExplosion;

        [SerializeField] Sprite[] _sprites;
        CircleCollider2D _circleCollider;
        SpriteRenderer _spriteRenderer;

        [SerializeField] ParticleSystem _hitPS, _BurgerTrail;
        #endregion

        #region Starting
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
            gameObject.SetActive(false);
        }
        #endregion

        #region Updating
        void Update()
        {
            if (_speed == 0 && _myTimer.IsReady)
                _speed = _originalSpeed;
        }
        void FixedUpdate()
        {
            if (_canMove)
                transform.position += _direction * _speed;
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            IDefenceCombat defender = collision.GetComponent<IDefenceCombat>();
            if (defender != null)
            {
                Shield shield = collision.GetComponent<Shield>();
                bool isAlly = _isTeams && shield._myTeam == _myTeam;

                if (shield.GetIndex() != _index && !isAlly)
                {
                    if (_canDamage)
                        defender.Defend(_combat, _damage, _index);

                    Destroying();
                }
            }

            else if (collision.gameObject.layer == GROUND)
                Destroying();
        }
        #endregion

        #region Shooting
        public void Setup(Combater combater, Team team, bool canDamage, int index, float damage, float speed)
        {
            _originalSpeed = speed;
            _canDamage = canDamage;
            _myCombater = combater;
            _damage = damage;
            _myTeam = team;
            _index = index;
            _speed = speed;
        }
        public void Setup(Combater combater, Explosion explosion, Team team, bool canDamage, int index, float damage, float speed)
        {
            _myExplosion = explosion;
            _myCombater = combater;
            _canDamage = canDamage;
            _originalSpeed = speed;
            _damage = damage;
            _myTeam = team;
            _index = index;
            _speed = speed;
        }

        public void Shoot(int combating, Vector2 position, float ZRotation)
        {
            _combat = (Combating)combating;

            transform.position = position;
            transform.rotation = ZRotation.ToQuaternion();
            transform.localScale = new Vector3(transform.localScale.x, ZRotation.GetYScale(), transform.localScale.z);
            _direction = (ZRotation * Mathf.Deg2Rad).GetDirectionFromAngle();

            Shooting();
        }
        void Shooting()
        {
            _spriteRenderer.sprite = _sprites[(int)_combat];
            _spriteRenderer.color = _combat.GetColor();

            OnAndOff(true);
            gameObject.SetActive(true);
        }
        #endregion

        #region Something
        void Destroying()
        {
            _myCombater.RemoveMe(this);
            _myTimer.ResetartTimer(0);
            OnAndOff(false);

            if (_myExplosion != null)
                _myExplosion.Setup(_myCombater, _canDamage, _index, transform.position);
            else
                _hitPS.Play();

            _myExplosion = null;
            Invoke(nameof(SetMeOff), .5f);
        }
        void SetMeOff() => gameObject.SetActive(false);

        protected void OnAndOff(bool on_off)
        {
            _canMove = on_off;
            _spriteRenderer.enabled = on_off;
            _circleCollider.enabled = on_off;

            _BurgerTrail.gameObject.SetActive(on_off && _combat == Combating.Super);
        }
        #endregion

        #region Pickup
        public void StopTime(float delay)
        {
            _myTimer.ResetartTimer(delay);
            _speed = 0;
        }
        #endregion
    }
}