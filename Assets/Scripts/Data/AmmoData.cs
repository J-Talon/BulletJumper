using UnityEngine;

[CreateAssetMenu(fileName = "AmmoData", menuName = "Ammo Data")]
public class AmmoData : ScriptableObject
{
    [SerializeField] private int currentAmmo = 0;
    [SerializeField] private int highestAmmo = 0;

    public int CurrentAmmo
    {
        get { return currentAmmo; }
        set { currentAmmo = value; }
    }

    public int HighestAmmo
    {
        get { return highestAmmo; }
        set { highestAmmo = value; }
    }

    public void AddAmmo(int points)
    {
        currentAmmo = points;
        if (currentAmmo > highestAmmo)
        {
            highestAmmo = currentAmmo;
        }
    }

    public void ResetAmmo()
    {
        currentAmmo = 0;
    }

    public void ResetAll()
    {
        currentAmmo = 0;
        highestAmmo = 0;
    }
}
