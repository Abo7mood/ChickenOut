using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Match;

namespace ChickenOut.Battle.Online
{
    public class GameManagerOnline : MonoBehaviourPunCallbacks
    {
        #region Stuff
        public static GameManagerOnline instance;

        const string MAIN_MENU = "MainMenu";

        public Action<GameMode> onGamePrepared;
        public Action onGameStarted;
        public Action<EndStates> onGameEnded;

        [SerializeField] Collector _collector; 
        public GameMode mode;
        NetworkManager _networkManager;
        TimersModeInfo info;

        [SerializeField] GameObject _waitMessage1, _waitMessage2;
        [SerializeField] TMP_Text roomnameWhileWaiting;

        #region States
        GameState NextState => (state + 1);
        public GameState State => state;
        GameState state;

        public EndStates EndState => endState;
        EndStates endState;
        #endregion

        [SerializeField] TMP_Text matchTimer, choosingTimer, endGameText;
        
        The_Timer_0 _myTimer;
        bool prepared;
        #endregion

        #region Starting
        void Awake()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            mode = _collector.GameModes[(int)PhotonNetwork.CurrentRoom.CustomProperties["mode"]];
            prepared = false;
        }
        void Start()
        {
            _networkManager = NetworkManager.instance;

            info = mode.TimersInfo;
            _myTimer = new The_Timer_0(info.WaitingTimer);

            ValidateConnection();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers = (byte)mode.MyType.MaxPlayers;
                ChangeState(GameState.waiting);
            }
        }
        #endregion

        void ValidateConnection()
        {
            if (PhotonNetwork.IsConnected)
                return;
            SceneManager.LoadScene(0);
        }

        #region States
        void StateCheck()
        {
            switch (State)
            {
                case GameState.waiting:
                    if (PhotonNetwork.IsMasterClient)
                        roomnameWhileWaiting.text = $"Give the room name [{PhotonNetwork.CurrentRoom.Name}] to your friends to join this room ";
                    _myTimer.ResetartTimer(info.WaitingTimer);
                    break;

                case GameState.Choosing:
                    if (PhotonNetwork.IsMasterClient)
                        roomnameWhileWaiting.gameObject.SetActive(false);
                    _myTimer.ResetartTimer(info.choosingTime_S);
                    onGamePrepared?.Invoke(mode);
                    break;

                case GameState.startGame:
                    _myTimer.ResetartTimer(info.MatchTimer);
                    onGameStarted?.Invoke();
                    break;

                case GameState.endGame:
                    _myTimer.ResetartTimer(info.endTime_S);
                    onGameEnded?.Invoke(EndState);
                    break;
            }
        }

        public void ChangeState(GameState newState, EndStates end = EndStates.TimeEnded) => photonView.RPC(nameof(ChangeStateRPC),
            RpcTarget.All, (int)newState, (int)end);
        [PunRPC]
        void ChangeStateRPC(int newState, int end = (int)EndStates.TimeEnded)
        {
            endState = (EndStates)end;
            state = (GameState)newState;
            StateCheck();
        }
        #endregion

        #region Timers
        void Update()
        {
            if (_myTimer.IsNotReady)
                TimerIsGoing();
            else
                TimerIsDone();
        }

        void TimerIsGoing()
        {
            switch (State)
            {
                case GameState.waiting:
                    bool isFull = PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
                    Waiting(isFull);

                    if (isFull && !prepared)
                    {
                        _myTimer.ResetartTimer(info.preparingTime_S);
                        prepared = true;
                    }
                    break;

                case GameState.Choosing:
                    choosingTimer.text = $"Selecting ends in {Mathf.Floor(_myTimer.TimeLeft)}";
                    break;

                case GameState.startGame:
                    matchTimer.text = _myTimer.ClockTimer(_myTimer.TimeLeft);
                    break;
            }
        }
        void TimerIsDone()
        {
            if (!PhotonNetwork.IsMasterClient || !photonView.IsMine)
                return;

            switch (State)
            {
                case GameState.waiting:
                case GameState.Choosing:
                case GameState.startGame:
                    ChangeState(NextState);
                    break;

                case GameState.endGame:
                    if (photonView.IsMine)
                        PhotonNetwork.LeaveRoom();
                    break;
            }
        }

        void Waiting(bool isFull)
        {
            if (isFull)
            {
                if (_waitMessage1.activeSelf) _waitMessage1.SetActive(false);
                if (!_waitMessage2.activeSelf) _waitMessage2.SetActive(true);
            }
            else
            {
                if (!_waitMessage1.activeSelf) _waitMessage1.SetActive(true);
                if (_waitMessage2.activeSelf) _waitMessage2.SetActive(false);
            }
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(MAIN_MENU);
            base.OnLeftRoom();
        }
        #endregion
    }
}