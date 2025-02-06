using System.Linq;
using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class ItemSpawner : Spawner
    {
        [SerializeField] Item[] items;
        [SerializeField] ItemPickup itemPickupPrefab;
        ItemPickup[] itemPickups;
        SpawnLine[] spawnLines;

        int nextItem;

        public void Setup()
        {
            spawnLines = FindObjectsOfType<SpawnLine>();

            for (int i = 0; i < items.Length; i++)
            {
                ItemPickup pickup = Instantiate(itemPickupPrefab);
                pickup.Setup(items[i]);
                pickup.gameObject.SetActive(false);
            }

            itemPickups = FindObjectsOfType<ItemPickup>(true);

            nextItem = Random.Range(0, spawnLines.Length);
            ResetTimer();
        }

        void Update()
        {
            if (spawnTimer.IsReady)
                Spawn();
        }

        void Spawn()
        {
            itemPickups[nextItem].transform.position = spawnLines[Random.Range(0, spawnLines.Length)].GetRandomPoint();
            itemPickups[nextItem].gameObject.SetActive(true);

            ResetTimer();
        }

        void ResetTimer()
        {
            spawnTimer.ResetartTimer();
            do
            {
                nextItem = Random.Range(0, itemPickups.Length);
            } while (itemPickups[nextItem].gameObject.activeInHierarchy && itemPickups.Any(i => !i.gameObject.activeInHierarchy));
        }
    }
}
