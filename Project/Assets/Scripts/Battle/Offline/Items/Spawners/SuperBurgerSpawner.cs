using UnityEngine;

namespace ChickenOut.Battle.Offline.Items
{
    public class SuperBurgerSpawner : Spawner
    {
        [SerializeField] SuperBurgerPickup superBurgerPrefab;
        SuperBurgerPickup superBurger;

        MoveLine[] moveLines;

        [SerializeField] float speed = 3f;
        int currentPoint;

        public void Setup()
        {
            moveLines = FindObjectsOfType<MoveLine>();
            currentPoint = 0;

            superBurger = Instantiate(superBurgerPrefab);
            superBurger.Setup(this);

            ResetTimer();
        }

         void Update()
         {
            if (superBurger != null)
            {
                Vector2 target = moveLines[currentPoint].transform.position;

                if (superBurger.gameObject.activeInHierarchy)
                {
                    superBurger.transform.position = Vector2.MoveTowards(superBurger.transform.position,
                        target, speed * Time.deltaTime);

                    if (Vector2.Distance(superBurger.transform.position, target) <= .1f)
                    {
                        currentPoint++;
                        if (currentPoint >= moveLines.Length)
                            currentPoint = 0;
                    }
                }

                else if (!superBurger.gameObject.activeInHierarchy && spawnTimer.IsReady)
                    SpawnSuper();
            }
         }

        void SpawnSuper()
        {
            superBurger.transform.position = moveLines[Random.Range(0, moveLines.Length)].transform.position;
            superBurger.gameObject.SetActive(true);

            Cinemachineshake2.instance.ShakeBurgerRespawn();
        }
    }
}
