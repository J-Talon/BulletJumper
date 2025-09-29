

namespace Entity.GamePickup
{
    public class PickupAmmo: Pickup
    {
        
        public override void onPickup(Player player)
        {
            player.addBullets(3);
        }
    }
}