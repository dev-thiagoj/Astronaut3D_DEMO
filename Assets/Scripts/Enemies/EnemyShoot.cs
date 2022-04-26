using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
    public class EnemyShoot : EnemieBase
    {
        public GunBase gunBase;

        protected override void Init()
        {
            base.Init();

            gunBase = GetComponentInChildren<GunBase>();

            gunBase.StartShoot();
        }
    }

}
