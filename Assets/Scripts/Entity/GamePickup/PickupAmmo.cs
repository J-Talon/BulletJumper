

namespace Entity.GamePickup
{
    public class PickupAmmo: Pickup
    {
        
        public void FixedUpdate()
        {
            float y = transform.position.y;
            if (y < base.minimumLoadedY)
                die();
        }
        
        public override void onPickup(Player player)
        {
            player.addBullets(3);
        }
    }
}