using ChickenOut.Battle.Online.Modes.Combatings;

namespace ChickenOut.Battle.Online.Combatings.Offence
{
    public class Explosion : Battle.Combatings.Offence.Explosion
    {
        ExplosionModeInfo _info;

        protected override void Start()
        {
            base.Start();

            _info = GameManagerOnline.instance.mode.ExplosionInfo;

            _radius = _info.radius;
            _growthSpeed = _info.growthSpeed;
            _originalGrowthSpeed = _info.growthSpeed;
        }
    }
}