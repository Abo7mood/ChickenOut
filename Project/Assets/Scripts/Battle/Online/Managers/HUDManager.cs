using System.Linq;
using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Chickens;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Battle.Online.Managers
{
    public class HUDManager : Battle.Managers.HUDManager
    {
        #region Stuff
        GameManagerOnline _gameManager;
        PhotonView _photonView;

        EndGamePanel[] _endGamePanels;

        float endTime;
        #endregion

        #region Starting
        protected override void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            base.Awake();

            _endGamePanels = _endGamePanelsHolder.GetComponentsInChildren<EndGamePanel>(true);
            _photonView = GetComponent<PhotonView>();
        }
        protected override void Start()
        {
            base.Start();

            _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameStarted += OnGameStarted;
            _gameManager.onGameEnded += OnGameEnded;

            _chickenInfos = NetworkManager.instance.Infos;

            InvokeRepeating(nameof(UpdatePing), 0f, 3f);
        }

        void OnGamePrepared(GameMode mode)
        {
            endTime = mode.TimersInfo.endTime_S;

            HideExtraUI();
            UpdateState();
        }
        void OnGameStarted()
        {
            Invoke(nameof(ConnectToChicken), .5f);
            UpdateState();
            UpdateScore();
        }
        void OnGameEnded(EndStates end)
        {
            EndGame(end);
            Invoke(nameof(UpdateState), endTime / 3);
        }
        #endregion

        #region Updates
        void UpdateState()
        {
            for (int i = 0; i < _panels.Length; i++)
                _panels[i].gameObject.SetActive(i == (int)_gameManager.State);
        }

        protected override void UpdateNames()
        {
            base.UpdateNames();

            for (int i = 0; i < _scores.Length; i++)
                if (_gameManager.mode.MyType.IsTeam)
                    _scores[i].nameText.color = _chickenInfos[i].team.GetColor();
        }

        void UpdatePing() => _pingText.text = $"{PhotonNetwork.GetPing()}";
        #endregion

        #region Other
        void HideExtraUI()
        {
            for (int i = 0; i < _scores.Length; i++)
                _scores[i].gameObject.SetActive(i < PhotonNetwork.CurrentRoom.PlayerCount);
        }

        void ConnectToChicken()
        {
            foreach (Chicken chicken in FindObjectsOfType<Chicken>())
            {
                if (chicken.IsMine)
                {
                    foreach (SpriteChanger changer in _spriteChangers)
                        chicken.GetComponent<ChickenCombater>().onChangeCombatingCalled += changer.CheckCombating;
                    chicken.GetComponent<Items.ItemPicker>().onItemCollected += StartTimer;

                    chicken.onNameChanged += UpdateNames;
                    chicken.onHPChanged += UpdateHP;
                    UpdateControls(chicken.Info.rightControls, chicken.Info.leftControls);
                }

                chicken.onScoreChanged += UpdateScore;
            }
        }
        #endregion

        #region End game
        public void EndGame(EndStates endGame)
        {
            int winnerIndex = -1;
            for (int i = 0; i < 4; i++)
                if (_chickenInfos[i].score == _chickenInfos.Max(ci => ci.score))
                    winnerIndex = i;

            string message = endGame switch
            {
                EndStates.MaxScoreReached => $"Points goal has been reached by {_chickenInfos[winnerIndex].myName}",
                EndStates.LastManStanding => $"{_chickenInfos[winnerIndex].myName} is the last man standing",
                EndStates.TimeEnded => $"Time out, the player with the most points is {_chickenInfos[winnerIndex].myName}",
                _ => $"Winner: {_chickenInfos[winnerIndex].myName}",
            };

            _photonView.RPC(nameof(EndGameRPC), RpcTarget.All, winnerIndex, message);
        }
        [PunRPC]
        void EndGameRPC(int winnerIndex, string message)
        {
            for (int i = 0; i < _endGamePanels.Length; i++)
            {
                _chickenInfos[i].isWinner = i == winnerIndex;

                _endGamePanels[i].UpdateUI(_chickenInfos[i]);
                _endGamePanels[i].gameObject.SetActive(i < PhotonNetwork.CurrentRoom.PlayerCount);
            }

            _endGameText.text = message;

            _fireworksPS.Play();
        }
        #endregion
    }
}