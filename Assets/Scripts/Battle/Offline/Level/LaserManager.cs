using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenOut.Battle.Offline
{
    public class LaserManager : MonoBehaviour
    {

       [SerializeField] GameObject[] _lasersGameobjects;
        private void Start()
        {
            Type1();
            Type2();
            Type3();
            Type4();
        }
        private void Type1()
        {
            _lasersGameobjects[0].gameObject.GetComponent<Laser>()._canShoot = true;
            _lasersGameobjects[0].gameObject.GetComponent<Laser>()._time = 2;
           
            _lasersGameobjects[0].gameObject.GetComponent<Laser>()._canOff = true;
        }
        private void Type2()
        {
            _lasersGameobjects[1].gameObject.GetComponent<Laser>()._canShoot = true;
            _lasersGameobjects[1].gameObject.GetComponent<Laser>()._canOff = false;
            _lasersGameobjects[2].gameObject.GetComponent<Laser>().alpha = 1;
        }
        private void Type3()
        {
            _lasersGameobjects[2].gameObject.GetComponent<Laser>()._canOff = true;
            _lasersGameobjects[2].gameObject.GetComponent<Laser>()._canShoot = true;
            _lasersGameobjects[2].gameObject.GetComponent<Laser>()._time = 3;
          

        }
        private void Type4()
        {
            _lasersGameobjects[3].gameObject.GetComponent<Laser>()._canOff = true;
            _lasersGameobjects[3].gameObject.GetComponent<Laser>()._canShoot = true;
            _lasersGameobjects[3].gameObject.GetComponent<Laser>()._time = 4;
       
        }


    }
}
