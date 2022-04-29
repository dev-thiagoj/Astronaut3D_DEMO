using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COISAS PARA FAZER:

//implementar para que comece a atirar somente qdo o player chegar a certa distância dele (trigger? raycast? outro?)

namespace Enemy
{
    public class EnemyShoot : EnemieBase
    {
        public GunBase gunBase;
        [SerializeField] private Player player;

        private void OnValidate()
        {
            player = GameObject.FindObjectOfType<Player>();
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

            gunBase.StartShoot();
        }

        protected override void Kill()
        {
            base.Kill();

            
            gunBase.StopShoot();
        }

        public void PlayerKilled()
        {
            if(player.healthBase._currLife <= 0)
            {
                gunBase.StopShoot();
            }
        }
    }

}
