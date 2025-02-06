using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ChickenOut.Battle.Combatings.Offence;

namespace ChickenOut.Battle.Managers
{
    public class FightManager : MonoBehaviour
    {
        #region Stuff
        public Gun this[int index] => _allGuns[index];
        protected Gun[] _allGuns;

        [SerializeField] protected Transform _attacksHolder, _explosionsHolder;

        [SerializeField] protected Attack _attackPrefab;
        [SerializeField] protected Explosion _explosionPrefab;

        protected List<Attack> attacks = new List<Attack>();
        protected List<Explosion> explosions = new List<Explosion>();
        #endregion

        #region Attacks
        protected Attack Attack => attacks.First(a => !a.gameObject.activeInHierarchy);
        protected bool CantUseAttack => attacks.Count == 0 || !attacks.Any(a => !a.gameObject.activeInHierarchy);
        protected Attack NewAttack
        {
            get
            {
                Attack attack = Instantiate(_attackPrefab, _attacksHolder);
                attacks.Add(attack);
                return attack;
            }
        }
        #endregion

        #region Explosions
        protected Explosion Explosion => explosions.First(a => !a.gameObject.activeInHierarchy);
        protected bool CantUseExplosion => explosions.Count == 0 || !explosions.Any(a => !a.gameObject.activeInHierarchy);
        protected Explosion NewExplosion
        {
            get
            {
                Explosion explosion = Instantiate(_explosionPrefab, _explosionsHolder);
                explosions.Add(explosion);
                return explosion;
            }
        }
        #endregion
        public Attack GetAttack
        {
            get
            {
                if (CantUseAttack)
                    return NewAttack;
                else
                    return Attack;
            }
        }
        public Explosion GetExplosion
        {
            get
            {
                if (CantUseExplosion)
                    return NewExplosion;
                else
                    return Explosion;
            }
        }
    }
}
