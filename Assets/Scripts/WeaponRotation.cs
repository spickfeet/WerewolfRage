using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class WeaponRotation
    {
        private bool _weaponPointedLeft;

        public WeaponRotation(bool weaponPointedLeft) 
        {
            _weaponPointedLeft = weaponPointedLeft;
        }
        public (Quaternion, bool) ChangeRotation(Vector3 weaponPos, Quaternion weaponRot,float offset)
        {
            bool flipY = false;
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponPos;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            weaponRot = Quaternion.Euler(0f, 0f, rotZ + offset);
            if (rotZ > 90 && rotZ < 180 || rotZ < -90 && rotZ > -180)
            {
                flipY = true;
            }
            if (rotZ > -90 && rotZ < 90)
            {
                flipY = false;
            }
            return (weaponRot, flipY);
        }
    }
}
