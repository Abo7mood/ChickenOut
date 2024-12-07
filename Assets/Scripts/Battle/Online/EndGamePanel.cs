using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using ChickenOut.Battle.Online.Managers;

namespace ChickenOut.Battle.Online
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] GameObject winner;
        [SerializeField] TMP_Text WName, WOrder, WScore, WGun;
        [Space(10)]
        [SerializeField] GameObject loser;
        [SerializeField] TMP_Text LName, LOrder, LScore, LGun;

        public void UpdateUI(ChickenInfo info)
        {
            string name = $"Name: {info.myName}",
                score = $"Score: {Mathf.Clamp(info.score, 0, GameManagerOnline.instance.mode.ChickenInfo.startingPoints * 2)}",
                gunIndex = $"Gun: {FightManagerOnline.instance[info.gunIndex].myName}",
                order = $"Order: {info.order}";

            if (info.isWinner)
                UpdateWinner(name, score, gunIndex, order);
            else
                UpdateLoser(name, score, gunIndex, order);

            winner.SetActive(info.isWinner);
            loser.SetActive(!info.isWinner);
        }

        void UpdateWinner(string name, string score, string gunName, string order)
        {
            WName.text = name;
            WScore.text = score;
            WGun.text = gunName;
            WOrder.text = order;
        }
        void UpdateLoser(string name, string score, string gunName, string order)
        {
            LName.text = name;
            LScore.text = score;
            LGun.text = gunName;
            LOrder.text = order;
        }
    }
}