

namespace Enemy
{
    public class EnemyShoot : EnemyBase
    {
        public GunBase gunBase;

        public override void Update()
        {
            base.Update();
            PlayerKilled();
        }

        protected override void Init()
        {
            base.Init();
            gunBase = GetComponentInChildren<GunBase>();
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

        protected override void PlayerKilled()
        {
            base.PlayerKilled();
            
            if (!Player.Instance.isAlive)
            {
                StopShooting();
            }
        }
    }
}
