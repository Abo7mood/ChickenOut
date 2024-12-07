using UnityEngine.SceneManagement;
using Photon.Pun;

namespace ChickenOut.Battle.Online
{
    public class PauseMenuOnline : MonoBehaviourPunCallbacks
    {
        const string MAIN_MENU = "MainMenu";

        public void LeaveRoom() => PhotonNetwork.LeaveRoom();
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(MAIN_MENU);
            base.OnLeftRoom();
        }
    }
}