namespace Entity.GamePickup
{
    public class PickupHealth: Pickup
    {
        public override void onPickup(Player player)
        {
            player.heal();
        }
    }
}