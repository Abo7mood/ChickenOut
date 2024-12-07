using Photon.Pun;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle.Online
{
    public class ParticlesController : Battle.ParticlesController
    {
        #region Stuff
        PhotonView _photonView;
        #endregion

        #region Starting
        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            ChickenMovement movement = GetComponent<ChickenMovement>();
            movement.onJumped += CallJumpParticleRPC;
            movement.onLanded += CallLandParticleRPC;
            movement.onDashed += CallDashParticleRPC;

            GetComponent<Chicken>().onDamageTaken += CallFeathersParticleRPC;

            if (!_photonView.IsMine)
                return;

            GetComponent<ItemPicker>().onItemCollected += CallItemParticleRPC;
        }
        #endregion

        #region Other
        void CallItemParticleRPC(Item item) => _photonView.RPC(nameof(ItemParticleRPC), RpcTarget.All, (int)item.type);
        [PunRPC] void ItemParticleRPC(ItemType type) => ItemParticle(type);
        
        void CallFeathersParticleRPC(float nm) => _photonView.RPC(nameof(FeathersParticleRPC), RpcTarget.All, nm);
        [PunRPC] void FeathersParticleRPC(float nm) => FeathersParticle(nm);
        #endregion

        #region Movement
        void CallJumpParticleRPC() => _photonView.RPC(nameof(JumpParticleRPC), RpcTarget.All);
        [PunRPC] void JumpParticleRPC() => JumpParticle();

        void CallLandParticleRPC() => _photonView.RPC(nameof(LandParticleRPC), RpcTarget.All);
        [PunRPC] void LandParticleRPC() => LandParticle();

        void CallDashParticleRPC(int dashDirection, float delay) => _photonView.RPC(nameof(DashParticleRPC), RpcTarget.All, dashDirection, delay);
        [PunRPC] void DashParticleRPC(int dashDirection, float delay) => DashParticle(dashDirection, delay);
        #endregion
    }
}