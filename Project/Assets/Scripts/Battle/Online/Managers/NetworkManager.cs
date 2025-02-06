using System.Linq;
using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Chickens;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Chickens;

namespace ChickenOut.Battle.Online
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Stuff
        public static NetworkManager instance;

        public ChickenInfo this[int index] => Infos[index];
        public ChickenInfo[] Infos => _chickenInfos;
        [SerializeField] ChickenInfo[] _chickenInfos;

        GameManagerOnline _gameManager;

        int _winScore;
        int _gunIndex;
        bool _shootOnRelease;
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
            _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameStarted += OnGameStarted;
        }
        void OnGamePrepared(GameMode mode)
        {
            ChickenModeInfo info = mode.ChickenInfo;
            _winScore = info.startingPoints * 2;

            for (int i = 0; i < Infos.Length; i++)
                Infos[i].score = i < PhotonNetwork.CurrentRoom.PlayerCount ? info.startingPoints : 0;

            if (PhotonNetwork.IsMasterClient)
            {
                Chicken[] chickens = FindObjectsOfType<Chicken>();
                for (int i = 0; i < chickens.Length; i++)
                    chickens[i].CallGetInfoRPC(i);
            }
        }
        void OnGameStarted()
        {
            Chicken[] chickens = FindObjectsOfType<Chicken>();
            for (int i = 0; i < chickens.Length; i++)
            {
                if (chickens[i].IsMine)
                {
                    chickens[i].CallChooseWeaponRPC(_gunIndex);
                    chickens[i].CallChooseToggleRPC(_shootOnRelease);
                    chickens[i].CallChooseControlsRPC();
                }

                SpawnLine[] _spawnLines = FindObjectsOfType<SpawnLine>();
                chickens[i].transform.position = _spawnLines[i].transform.position;
            }
        }
        #endregion

        #region Choosings
        public void ChooseWeapon(int gunIndex) => _gunIndex = gunIndex;
        public void ChooseToggle(bool shootOnRelease) => _shootOnRelease = shootOnRelease;
        #endregion

        #region End
        public void CheckEndGame()
        {
            if (CantCheckEnd)
                return;

            if (MaxScoreWasReached)
                _gameManager.ChangeState(GameState.endGame, EndStates.MaxScoreReached);

            if (AllReachedZeroExceptOne)
                _gameManager.ChangeState(GameState.endGame, EndStates.LastManStanding);
        }
        #region bools
        bool CantCheckEnd => !PhotonNetwork.IsMasterClient || !photonView.IsMine || _gameManager.State == GameState.endGame;
        bool MaxScoreWasReached => Infos.Max(info => info.score) == _winScore;
        bool AllReachedZeroExceptOne => Infos.Where(info => info.score > 0).ToArray().Length <= 1;
        #endregion
        #endregion
    }
}