using UnityEngine;
using ChickenOut.Battle.Offline.Items;
using ChickenOut.Battle.Offline.Managers;
using ChickenOut.Battle.Offline.Combatings.Defence;

namespace ChickenOut.Battle.Offline.Chickens
{
    [RequireComponent(typeof(ChickenCombater))]
    public class Chicken : Battle.Chickens.Chicken
    {
        public ChickenInfo info;

        //HUDManager _myHUD;
        //FightManagerOffline _fightManager;

        //[SerializeField] GameObject cookedChicken, jackBox;
        //[SerializeField] ParticleSystem featherPS;

        //protected override void Awake()
        //{
        //    base.Awake();

        //    _combater = GetComponent<ChickenCombater>();
        //    _movement = GetComponent<ChickenMovement>();
        //    _shield = GetComponentInChildren<Shield>();
        //}
        //protected override void Start()
        //{
        //    base.Start();

        //    _fightManager = FightManagerOffline.instance;
        //    _soundManager = SoundManager.instance;
        //    //_myHUD = HUDManager.instance;

        //    _movement.DoControls(input);
        //    _combater.DoControls(input);
        //    _shield.Setup(this);
        //}

        //public void DoPlayerInfo(ChickenInfo info)
        //{
        //    this.info = info;
        //    //combater.SwitchGun(info.gunIndex, info.shootOnRelease);
        //}

        //public override void TakeDamage(float dmg, int index)
        //{
        //    base.TakeDamage(dmg, index);

        //    if (jackBox.activeInHierarchy)
        //        GetOutOfBox();

        //    if (dmg > 0)
        //    {
        //        //_myHUD.UpdateHP(HP, HPMax);
                
        //        featherPS.Play();
        //        animator.SetTrigger("Hit");
        //        //_soundManager.DamageSound(); for testing
        //    }

        //    if (IsDead)
        //        IncreaseScore(-3);
        //}
        //public void IncreaseScore(float score)
        //{
        //    //myHUD.UpdateScore(info.score);
        //}

        //public override void Die()
        //{
        //    base.Die();

        //    TurningOnAndOff(false, 0);
        //    _shield.gameObject.SetActive(false);

        //    Invoke(nameof(TurnMeOff), 2f);
        //}
        //void TurnMeOff() => gameObject.SetActive(false);
        //void TurnMyShieldOn() => _shield.gameObject.SetActive(true);

        //public override void Undie()
        //{
        //    base.Die();

        //    HP = HPMax;
        //    TakeDamage(0, 0);
        //    TurningOnAndOff(true, 7);
            
        //    gameObject.SetActive(true);
        //    Invoke(nameof(TurnMyShieldOn), 2f);
        //}
        //void TurningOnAndOff(bool onOrOff, int layer)
        //{
        //    _movement.enabled = onOrOff;
        //    animator.enabled = onOrOff;
        //    spriteRenderer.enabled = onOrOff;
        //    _combater.gameObject.SetActive(onOrOff);
        //    cookedChicken.SetActive(!onOrOff);

        //    input.enabled = onOrOff;

        //    Transform[] myTransforms = GetComponentsInChildren<Transform>();
        //    foreach (Transform transform in myTransforms)
        //        transform.gameObject.layer = layer;
        //}

        //void GetInBox()
        //{
        //    //input.enabled = false;
        //    //spriteRenderer.enabled = false;
        //    _combater.gameObject.SetActive(false);

        //    jackBox.SetActive(true);
        //}
        //void GetOutOfBox()
        //{
        //    //input.enabled = true;
        //    //spriteRenderer.enabled = true;
        //    _combater.gameObject.SetActive(true);

        //    jackBox.SetActive(false);
        //}
    }
}