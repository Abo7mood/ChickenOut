using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenOut.Battle.Offline
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] GameObject cage;
        [SerializeField] float cageFallSpeed = 8f;
        [SerializeField] float height = 20f;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Chicken"))
            {
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y / 2);
                GameObject trap = Instantiate(cage, new Vector2(transform.position.x , transform.position.y + height), Quaternion.identity);
                trap.transform.localPosition = Vector2.Lerp(trap.transform.localPosition, transform.position, cageFallSpeed);
            }
        }
    }
}
