using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Battle.Online.Items
{
    public class SpawnerOnline : MonoBehaviour
    {
        protected PhotonView _photonView;
        GameManagerOnline _gameManager;

        protected bool _enableMyItems;
        protected The_Timer_0 spawnTimer;

        public bool IsMine => _photonView.IsMine;
        protected bool Cant => !(PhotonNetwork.IsMasterClient && _photonView.IsMine);

        protected bool NotYet => _gameManager.State != GameState.startGame;

        protected float OriginalChargeTime { set => spawnTimer.SetOriginalTime(value); }
        protected float TimeLeft => spawnTimer.TimeLeft;
        protected bool IsReady => spawnTimer.IsReady;

        protected virtual void Awake() => _photonView = GetComponent<PhotonView>();
        protected virtual void Start()
        {
            _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameStarted += OnGameStarted;
        }

        protected virtual void OnGamePrepared(GameMode mode) { }
        protected virtual void OnGameStarted() { }

        protected virtual void ResetTimer(float newChargeTime = 0)
        {
            spawnTimer.SetTime((newChargeTime != 0) ? newChargeTime : spawnTimer.OriginalChargeTime);
            spawnTimer.ResetartTimer();
        }
    }
}