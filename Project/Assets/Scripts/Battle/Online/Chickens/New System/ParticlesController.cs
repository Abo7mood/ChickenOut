using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Chickens.Movement;

namespace ChickenOut.Battle.Chickens
{
    public class ParticlesController : MonoBehaviour
    {
        #region Stuff

        #region Properities
        private ParticleSystem[][] Particles => new ParticleSystem[][] { _chickenParticles, _itemsParticles };

        internal bool IsOnline => PhotonNetwork.IsConnected && PhotonNetwork.InRoom && _photonView != null;
        internal bool IsMine => _photonView != null && _photonView.IsMine;
        internal bool IsNotMine => _photonView != null && !_photonView.IsMine;
        #endregion

        #region Variables
        [SerializeField] private ParticleSystem[] _chickenParticles, _itemsParticles;
        private PhotonView _photonView;

        #region Don't touch
#if UNITY_EDITOR
        enum ParticlesOrder { Chicken, Items }

        [SerializeField] ChickenParticles _toKnowTheOrderOfTheEnum;
        [SerializeField] ItemType _toKnowTheOrderOfTheEnum2;
#endif
        #endregion
        #endregion

        #endregion

        #region Starts
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            GetComponent<Chicken>().onDamageTaken += CallFeathersParticleRPC;

            if (IsNotMine)
                return;

            ChickenMovementSystem movement = GetComponent<ChickenMovementSystem>();
            movement.onJumped += CallJumpParticleRPC;
            movement.onLanded += CallLandParticleRPC;
            movement.onDashed += CallDashParticleRPC;

            GetComponent<ItemPickingController>().onItemTypePickedup += CallItemParticleRPC;
        }
        #endregion

        #region Particles calling

        #region Chicken
        void CallFeathersParticleRPC(float nm) => CallParticleRPC((int)ChickenParticles.Feathers, (int)ParticlesOrder.Chicken);
        void CallJumpParticleRPC() => CallParticleRPC((int)ChickenParticles.Jump, (int)ParticlesOrder.Chicken);
        void CallLandParticleRPC() => CallParticleRPC((int)ChickenParticles.Land, (int)ParticlesOrder.Chicken);
        #endregion

        #region Items
        void CallItemParticleRPC(ItemType type, float duration)
        {
            CallParticleRPC((int)ChickenParticles.Starts, (int)ParticlesOrder.Chicken);
            CallParticleRPC((int)type, (int)ParticlesOrder.Items);
        }
        #endregion

        #region Special case
        void CallDashParticleRPC(int dashDirection, float delay)
        {
            if (IsOnline)
                _photonView.RPC(nameof(DashParticleRPC), RpcTarget.All, dashDirection, delay);
            else
                DashParticleRPC(dashDirection, delay);
        }
        [PunRPC]
        void DashParticleRPC(int dashDirection, float delay)
        {
            int y = dashDirection > 0 ? -90 : dashDirection < 0 ? 90 : 0;
            _chickenParticles[(int)ChickenParticles.Dash].gameObject.transform.eulerAngles = new Vector3(0, y);
            _chickenParticles[(int)ChickenParticles.Dash].Play();

            Invoke(nameof(StopDashing), delay);
        }
        void StopDashing() => _chickenParticles[(int)ChickenParticles.Dash].Stop();
        #endregion

        #endregion

        #region Particles Player
        void CallParticleRPC(int index, int particleType)
        {
            if (IsOnline)
                _photonView.RPC(nameof(ParticleRPC), RpcTarget.All, index, particleType);
            else
                ParticleRPC(index, particleType);
        }
        [PunRPC]
        void ParticleRPC(int index, int particleType)
        {
            //if (Particles != null && Particles[particleType].Length > index && Particles[particleType][index] != null)
                Particles[particleType][index].Play();
        }
        #endregion
    }
}