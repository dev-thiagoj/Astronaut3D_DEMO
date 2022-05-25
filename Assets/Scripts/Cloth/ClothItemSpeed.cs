

namespace Cloth
{
    public class ClothItemSpeed : ClothItemBase
    {
        public float targetSpeed = 2;

        public override void Collect()
        {
            base.Collect();

            Player.Instance.ChangeSpeed(targetSpeed, duration);
        }
    }
}
