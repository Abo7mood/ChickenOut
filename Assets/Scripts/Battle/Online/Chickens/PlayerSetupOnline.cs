using UnityEngine;
using Photon.Pun;

namespace ChickenOut.Battle.Online
{
    public class PlayerSetupOnline : MonoBehaviourPunCallbacks
    {
        [SerializeField] Behaviour[] _components;
        [SerializeField] GameObject[] _myObjects;
        GameObject _cameras;

        void Awake()
        {
            if (!photonView.IsMine)
                return;

            _cameras = GameObject.Find("Cameras");
            _cameras.SetActive(true);

            for (int i = 0; i < _cameras.transform.childCount; i++)
                _cameras.transform.GetChild(i).gameObject.SetActive(true);
        }

        void Start()
        {
            if (photonView.IsMine)
                return;

            foreach (Behaviour go in _components)
                go.enabled = false;

            foreach (GameObject mygo in _myObjects)
                mygo.SetActive(false);
        }
    }
}
