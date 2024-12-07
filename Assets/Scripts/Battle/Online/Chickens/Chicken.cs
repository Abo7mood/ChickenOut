using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Online.Managers;
using ChickenOut.Battle.Online.Combatings.Defence;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Battle.Online.Chickens
{
    [RequireComponent(typeof(ChickenCombater))]
    [RequireComponent(typeof(ChickenMovement))]
    public class Chicken : Battle.Chickens.Chicken
    {
        #region Stuff
        FightManagerOnline _fightManager;
        GameManagerOnline _gameManager;
        NetworkManager _networkManager;
        PhotonView _photonView;

        [HideInInspector] public ProfileData playerProfile { get; }
        [HideInInspector] public ProfileData data { get; }
        public ChickenInfo Info => _myInfo;
        public bool IsMine => _photonView.IsMine;
        #endregion

        #region Starting
        protected override void Awake()
        {
            base.Awake();

            _networkManager = FindObjectOfType<NetworkManager>();

            _movement = GetComponent<ChickenMovement>();
            _combater = GetComponent<ChickenCombater>();
            _shield = GetComponentInChildren<Shield>();
            _photonView = GetComponent<PhotonView>();

            GetComponentInChildren<Shield>().Setup(this);

            if (!_photonView.IsMine)
                return;

            _combater.DoControls(_input);
            _movement.DoControls(_input);
        }
        protected override void Start()
        {
            base.Start();

            _fightManager = FightManagerOnline.instance;

            _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameStarted += OnGameStarted;
            _gameManager.onGameEnded += OnGameEnded;

            onDamageTaken += CallTakeDamageRPC;
            onDied += CallDieRPC;

            if (!_photonView.IsMine)
                return;

            ItemPicker picker = GetComponent<ItemPicker>();
            picker.onGetInBoxCalled += CallGetInBoxRPC;
            picker.onHealCollected += CallHealRPC;

            _myPointer.SetActive(true);
        }
        void OnGamePrepared(GameMode mode)
        {
            _maxHP = mode.ChickenInfo.maxHP;
            _currentHP = _maxHP;
        }
        void OnGameStarted()
        {
            if (!_photonView.IsMine)
                return;

            _combater.ResetGun();
        }
        void OnGameEnded(EndStates end)
        {
            if (!_photonView.IsMine)
                return;

            _input.enabled = false;
        }
        #endregion

        #region Damage and Score
        void CallTakeDamageRPC(float HP) => _photonView.RPC(nameof(TakeDamageRPC), RpcTarget.All, _currentHP);
        [PunRPC] void TakeDamageRPC(float HP) => BeDamaged(HP);

        void CallDieRPC(int index)
        {
            _photonView.RPC(nameof(IncreaseScoreRPC), RpcTarget.MasterClient, _myInfo.Index, index);
            _photonView.RPC(nameof(DieRPC), RpcTarget.All);
        }
        [PunRPC] void IncreaseScoreRPC(int myIndex, int attackerIndex)
        {
            if (myIndex != attackerIndex)
                _networkManager[attackerIndex].score++;

            _myInfo.score--;

            int[] indexes = new int[] { myIndex, attackerIndex };
            int[] scores = new int[] { _networkManager[myIndex].score, _networkManager[attackerIndex].score };
            _photonView.RPC(nameof(TakeScore), RpcTarget.Others, indexes, scores);

            onScoreChanged?.Invoke();
            _networkManager.CheckEndGame();
        }
        [PunRPC] void TakeScore(int[] indexes, int[] scores)
        {
            for (int i = 0; i < indexes.Length; i++)
                _networkManager[indexes[i]].score = scores[i];

            onScoreChanged?.Invoke();
        }
        #endregion

        #region Dying
        [PunRPC]
        public void DieRPC()
        {
            if (_photonView.IsMine)
                _fightManager.ReSpawnChicken(this);
            Dying(_photonView.IsMine);
        }

        public void Undie() => _photonView.RPC(nameof(UndieRPC), RpcTarget.All);
        [PunRPC]
        public void UndieRPC() => UnDying(_photonView.IsMine);
        #endregion

        #region Pickup

        #region Heal
        void CallHealRPC(float duration) => _photonView.RPC(nameof(HealRPC), RpcTarget.All, duration);
        [PunRPC]
        void HealRPC(float duration) => Heal(duration);
        #endregion

        #region Box
        void CallGetInBoxRPC(float duration) => _photonView.RPC(nameof(GetInBoxRPC), RpcTarget.All, duration);
        [PunRPC]
        void GetInBoxRPC(float duration) => GetInBox(duration, _photonView.IsMine);
        #endregion

        #endregion

        #region RPCs
        public void CallGetInfoRPC(int index) => _photonView.RPC(nameof(GetInfoRPC), RpcTarget.AllBuffered, index);
        [PunRPC] public void GetInfoRPC(int index)
        {
            _myInfo = _networkManager[index];

            _combater.GetInfo(_myInfo);
            _shield.GetInfo(_myInfo);
            _movement.GetInfo(_myInfo);

            if (index % 2 == 0)
                _myInfo.team = Team.Team2;
            else
                _myInfo.team = Team.Team1;
        }

        public void CallChooseWeaponRPC(int gunIndex) => _photonView.RPC(nameof(ChooseWeaponRPC), RpcTarget.All, gunIndex);
        [PunRPC] void ChooseWeaponRPC(int gunIndex) => _myInfo.gunIndex = gunIndex;

        public void CallChooseToggleRPC(bool shootOnRelease) => _photonView.RPC(nameof(ChooseToggleRPC), RpcTarget.All, shootOnRelease);
        [PunRPC] void ChooseToggleRPC(bool shootOnRelease) => _myInfo.shootOnRelease = shootOnRelease;

        public void CallChooseControlsRPC()
        {
            ProfileData data = Data.LoadProfile();
            _photonView.RPC(nameof(ChooseControlsRPC), RpcTarget.All, data.userName, (int)data.leftControls, (int)data.rightControls);
        }

        [PunRPC] void ChooseControlsRPC(string username, int lCtrls, int rCtrls)
        {
            _myInfo.leftControls = (LSControls)lCtrls;
            _myInfo.rightControls = (RSControls)rCtrls;

            _photonView.RPC(nameof(TakeNameRPC), RpcTarget.All, _myInfo.Index, username);
        }

        [PunRPC] void TakeNameRPC(int index, string username)
        {
            _networkManager[index].myName = username;

            _playerNameText.text = _myInfo.myName;
            onNameChanged?.Invoke();

            UpdateTeams();
        }

        void UpdateTeams()
        {
            if (!_gameManager.mode.MyType.IsTeam)
                return;

            Color teamColor = _myInfo.team.GetColor();
            _playerNameText.color = teamColor;
            onTeamingUp?.Invoke(teamColor);
        }
        #endregion
    }
}