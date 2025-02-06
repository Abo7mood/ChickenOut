using UnityEngine;
using UnityEngine.UI;

namespace ChickenOut.Battle.Offline.Managers
{
    public class HUDManager : Battle.Managers.HUDManager
    {
        //EndGamePanel[] _endGamePanels;

        #region Starting
        void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            //_chickenInfos = GetComponent<NetworkManager>().Infos;

            _spriteChangers = _panels[2].GetComponentsInChildren<SpriteChanger>(true);
            _heartSprites = _heartsHolder.GetComponentsInChildren<Image>(true);
            _scores = _scoresHolder.GetComponentsInChildren<ScoreHolder>(true);
        }
        void Start()
        {
            Invoke(nameof(ConnectToChicken), .5f);
            UpdateScore();
            UpdateNames();
        }

        /*void OnGameEnded(EndStates end)
        {
            EndGame(end);
        }*/
        #endregion

        void ConnectToChicken()
        {
            /*foreach (ChickenOffline chicken in FindObjectsOfType<ChickenOffline>())
                if (chicken.photonView.IsMine)
                {
                    chicken.GetComponent<ChickenCombaterOffline>().onChangeCombatingCalled += CombatingChanged;

                    chicken.onItemCollected += StartTimer;
                    chicken.onScoreChanged += UpdateScore;
                    chicken.onHPChanged += UpdateHP;
                    UpdateControls(chicken.Info.rightControls, chicken.Info.leftControls);
                }*/
        }
        void CombatingChanged(Combating combat)
        {
            foreach (SpriteChanger changer in _spriteChangers)
                if (combat == Combating.Super && changer.IsOriginal)
                    changer.ChangeSprite(_burgerSprite);
                else if (!changer.IsOriginal)
                    changer.ChangeSprite();
        }

        #region End game
        /*public void EndGame(EndStates endGame)
        {
            int winnerIndex = -1;
            for (int i = 0; i < 4; i++)
                if (_chickenInfos[i].Score == _chickenInfos.Max(ci => ci.Score))
                    winnerIndex = i;

            string message = endGame switch
            {
                EndStates.MaxScoreReached => $"Points goal has been reached by {_chickenInfos[winnerIndex].myName}",
                EndStates.LastManStanding => $"{_chickenInfos[winnerIndex].myName} is the last man standing",
                EndStates.TimeEnded => $"Time out, the player with the most points is {_chickenInfos[winnerIndex].myName}",
                _ => $"Winner: {_chickenInfos[winnerIndex].myName}",
            };

            _endGamePanels = FindObjectsOfType<EndGamePanel>(true);
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                _chickenInfos[i].isWinner = (i == winnerIndex);

                _endGamePanels[i].UpdateUI(_chickenInfos[i]);
                _endGamePanels[i].gameObject.SetActive(true);
            }

            _endGameText.text = message;

            _fireworksPS.Play();
        }*/
        #endregion
    }
}