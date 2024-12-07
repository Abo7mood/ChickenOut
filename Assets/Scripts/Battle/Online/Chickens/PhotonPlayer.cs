using UnityEngine;
using Photon.Pun;

namespace ChickenOut.Battle.Online
{
    public class PhotonPlayer : MonoBehaviourPunCallbacks
    {
        const string CHICKEN_NAME = "Chicken", CHICKEN_PATH = "Prefabs/Units/";
        
        GameObject _myPlayer;

        void Awake()
        {
            if (photonView.IsMine)
                if (_myPlayer == null)
                    _myPlayer = PhotonNetwork.Instantiate($"{CHICKEN_PATH}{CHICKEN_NAME}", Vector3.zero, Quaternion.identity, 0);
        }
    }
}