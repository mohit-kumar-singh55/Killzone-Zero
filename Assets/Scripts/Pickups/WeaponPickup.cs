using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] WeaponSO weaponSO;

    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        // 銃を変える
        activeWeapon.SwitchWeapon(weaponSO);
    }
}