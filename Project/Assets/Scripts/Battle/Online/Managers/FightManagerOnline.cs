using System.Collections;
using UnityEngine;
using Cinemachine;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Online.Chickens;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Battle.Online.Managers
{
    public class FightManagerOnline : FightManager
    {
        #region Stuff
        public static FightManagerOnline instance;

        [SerializeField] CinemachineTargetGroup _targetGroup;
        [SerializeField] Transform[] _spawnPoints = new Transform[4];

        float _respawnTime;
        #endregion

        #region Starting
        void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;
        }
        void Start()
        {
            GameManagerOnline gameManager = GameManagerOnline.instance;
            gameManager.onGamePrepared += OnGamePrepared;
            gameManager.onGameStarted += OnGameStarted;

            attacks.Clear();
        }

        void OnGamePrepared(GameMode mode)
        {
            _allGuns = mode.GunsInfo.allGuns;
            _respawnTime = mode.TimersInfo.respawnTime_S;
        }

        void OnGameStarted()
        {
            TargetGroupCam();
        }

        public void TargetGroupCam()
        {
            Chicken[] chickens = FindObjectsOfType<Chicken>();
            for (int i = 0; i < chickens.Length; i++)
                _targetGroup.AddMember(chickens[i].transform, 1, 7.5f);
        }
        #endregion

        #region Respawning
        public void ReSpawnChicken(Chicken chicken)
        {
            if (chicken.Info.score <= 0)
                return;

            StartCoroutine(ReSpawnChicken2(chicken));
        }

        public IEnumerator ReSpawnChicken2(Chicken chicken)
        {
            yield return new WaitForSeconds(_respawnTime);
            chicken.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
            chicken.Undie();
        }
        #endregion
    }
}