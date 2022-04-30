using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COISAS PARA FAZER:

//corrigir erro qdo o player morre dentro do trigger "EnemyTrigger linha 68"

namespace Enemy
{
    public class EnemyShoot : EnemyBase
    {
        public GunBase gunBase;

        //public GameObject trigger;
        //[SerializeField] private Player player;

        private void OnValidate()
        {
            player = FindObjectOfType<Player>();
        }

        public override void Update()
        {
            base.Update();
            PlayerKilled();
        }

        protected override void Init()
        {
            base.Init();

            gunBase = GetComponentInChildren<GunBase>();

            //gunBase.StartShoot();
        }

        protected override void Kill()
        {
            base.Kill();
            
            gunBase.StopShoot();
        }

        public void StartShooting()
        {
            gunBase.StartShoot();
        }

        public void StopShooting()
        {
            gunBase.StopShoot();
        }

        /*private void OnTriggerStay(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                while (player.isAlive) gunBase.StartShoot();
            }
            else gunBase.StopShoot();
        }*/

        protected override void PlayerKilled()
        {
            if (!player.isAlive)
            {
                gunBase.StopShoot();
            }
            else return;
        }
    }

}
