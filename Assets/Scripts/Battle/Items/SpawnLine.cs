using UnityEngine;

namespace ChickenOut.Battle
{
    public class SpawnLine : MonoBehaviour
    {
        [SerializeField] Transform[] points = new Transform[2];

        public Vector2 GetRandomPoint()
        {
            float random = Random.Range(Mathf.Min(points[0].position.x, points[1].position.x),
                Mathf.Max(points[0].position.x, points[1].position.x));

            return new Vector2(random, transform.position.y);
        }
    }
}
