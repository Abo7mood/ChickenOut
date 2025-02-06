using UnityEngine;
using ChickenOut.Battle.Offline.Chickens;

namespace ChickenOut.Battle.Offline.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        Enemy unit;

        [SerializeField] Transform rightTarget, leftTarget;
        Transform target;

        [SerializeField] float range, speed;

        private void Awake()
        {
            
        }
        private void Start()
        {
            
        }
        public void Setup(Enemy unit)
        {
            this.unit = unit;
            target = leftTarget;
        }

        void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirection(), range);
            Debug.DrawRay(transform.position, GetDirection() * range, Color.red);

            if (hit.collider != null)
            {
                Chickens.Chicken chicken = hit.collider.GetComponent<Chickens.Chicken>();

                if (chicken != null)
                    target = chicken.transform;
            }
        }

        void FixedUpdate() => Move();
        void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
            unit.animator.SetFloat("Speed", 1);

            if (Vector2.Distance(transform.position, target.position) <= .1f)
                CheckDirection();
        }

        Vector3 GetDirection()
        {
            /*if (unit.IsFacingLeft)
                return Vector3.left;
            else*/
                return Vector3.right;
        }
        void CheckDirection()
        {
            if (target == leftTarget)
                target = rightTarget;
            else
                target = leftTarget;
        }
    }
}
