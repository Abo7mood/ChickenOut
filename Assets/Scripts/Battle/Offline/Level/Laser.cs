using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenOut.Battle.Offline
{
    public class Laser : MonoBehaviour
    {
        #region Constructer
        [Header("Construct")]
        Rigidbody2D _rb;
        BoxCollider2D _box;
        [SerializeField] SpriteRenderer _s;

        #endregion
        #region float&int
        [HideInInspector] public int alpha;
        [HideInInspector] public float _time;
        private float anothertime=0;
        const int GROUND = 3;
        protected int _index;
        protected float _damage, _speed;
        #endregion

        #region Booleans
        [HideInInspector] public bool _canShoot;
        [HideInInspector] public bool _canOff;
        #endregion
        private void Awake()
        {
            _rb = GetComponentInChildren<Rigidbody2D>();
            _box = GetComponentInChildren<BoxCollider2D>();

        }
        private void Start()
        {
            
            _box.enabled = false;
            _s.color = new Color(1, 1, 1, 0);
        }
        private void Update()
        {
            anothertime += Time.deltaTime;
            if (_canShoot)
                LaserShoot();
            else
                return;
        }
        /// <summary>
        /// this method is implement the laser shoot
        /// </summary>
        private void LaserShoot()
        {
          
            if (_s.color.a >= .5f)
            {
                _s.color = new Color(1, 1, 1, 1);
                _box.enabled = true;
                if (_canOff)                
                    StartCoroutine(LaserCooldown(_time));
            }

            else

                _s.color = new Color(1, 1, 1, anothertime);
          ;
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IOffenceCombat>() != null)
                return;
            IOffenceCombat attacker = collision.GetComponent<IOffenceCombat>();
            if (collision.CompareTag("Chicken"))
            {
                

            }
            else
                return;

        }
        IEnumerator LaserCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            _s.color = new Color(1, 1, 1, 0);
            _box.enabled = false;
            anothertime = 0;

        }

    }
}
