using System.Collections.Generic;
using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class JackBoxSpawner : Spawner
    {
        [SerializeField] JackBoxPickup jackBoxPickupPrefab;
        List<JackBoxPickup> jackBoxes;

        [SerializeField] float mapEdge, mapHeghit;

        public void Setup()
        {
            jackBoxes = new List<JackBoxPickup>();
            ResetTimer();
        }

        void Update()
        {
            jackBoxes.RemoveAll(box => box == null);
            for (int i = 0; i < jackBoxes.Count; i++)
                if (Mathf.Abs(jackBoxes[i].transform.position.x) > mapEdge || Mathf.Abs(jackBoxes[i].transform.position.y) > mapHeghit)
                    ReSpawnJack(jackBoxes[i]);

            if (spawnTimer.IsReady)
                SpawnJack();
        }

        void SpawnJack()
        {
            JackBoxPickup box = Instantiate(jackBoxPickupPrefab);
            box.Setup();
            ReSpawnJack(box);

            jackBoxes.Add(box);
            ResetTimer();
        }
        void ReSpawnJack(JackBoxPickup box)
        {
            Vector2 vector = GetRandomPoint();
            Vector3 directoin = GetDirection(vector);

            box.transform.position = vector;
            box.Target = directoin;
            box.gameObject.SetActive(true);
        }
        Vector3 GetDirection(Vector2 vector)
        {
            if (vector.x == mapEdge)
                return Vector3.left;

            else if (vector.x == -mapEdge)
                return Vector3.right;

            else if (vector.y == mapHeghit)
                return Vector3.down;

            else if (vector.y == -mapHeghit)
                return Vector3.up;

            else
                return Vector3.zero;
        }

        Vector2 GetRandomPoint() => Random.Range(1, 5) switch
        {
            1 => new Vector2(Random.Range(-mapEdge, mapEdge), mapHeghit),
            2 => new Vector2(Random.Range(-mapEdge, mapEdge), -mapHeghit),
            3 => new Vector2(mapEdge, Random.Range(-mapHeghit, mapHeghit)),
            4 => new Vector2(-mapEdge, Random.Range(-mapHeghit, mapHeghit)),
            _ => Vector2.zero,
        };
    }
}
