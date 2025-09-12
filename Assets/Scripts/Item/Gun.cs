namespace Item
{
    public interface Gun
    {
        //return whether the gun fired successfully
        public abstract bool fire();
        
        //return whether the gun can fire
        public abstract bool canFire();
    }
}