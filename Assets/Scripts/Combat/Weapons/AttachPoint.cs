using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public enum RelativeSide
    {
        Left,
        Right,
        Center
    }

    // public GameObject PreAttachedWeapon;
    public WeaponBase AttachedWeapon;
    [HideInInspector] public bool IsEmpty = true;

    private RelativeSide Side;

    private void Awake()
    {
        // Log the local position of the AttachPoint
        Vector3 localPosition = transform.localPosition;

        // Set the relative side based on the localPosition.x value
        if (localPosition.x < 0)
        {
            Side = RelativeSide.Left;
        }
        else if (localPosition.x > 0)
        {
            Side = RelativeSide.Right;
        }
        else
        {
            Side = RelativeSide.Center;
        }
    }

    public bool InitialiseAttachPoint()
    {
        if (AttachedWeapon != null)
        {
            // AttachedWeapon = PreAttachedWeapon.GetComponent<WeaponBase>();
            AttachedWeapon.Side = Side;
            IsEmpty = false;
        }
        return IsEmpty;
    }

    public void AttachWeapon(WeaponBase weaponPrefabComponent, bool force)
    {
        if (!IsEmpty)
        {
            if (force == true)
            {
                DetachWeapon();
            }
            else
            {
                Debug.LogWarning("The weapon slot is already filled.");
                return;
            }
        }

        GameObject weaponObject = Instantiate(weaponPrefabComponent.gameObject, transform.position, transform.rotation, transform);
        WeaponBase weaponComponent = weaponObject.GetComponent<WeaponBase>();

        if (weaponComponent != null)
        {
            // Set the Side property of the weapon based on the AttachPoint
            weaponComponent.Side = Side;

            AttachedWeapon = weaponComponent;
            IsEmpty = false;
        }
        else
        {
            Debug.LogError("The weaponPrefab does not have a WeaponBase component.");
            Destroy(weaponObject);
        }
    }

    public void DetachWeapon()
    {
        if (IsEmpty)
        {
            Debug.LogWarning("No weapon is attached to this slot.");
            return;
        }

        // Destroy the GameObject the WeaponBase script is attached to
        Destroy(AttachedWeapon.gameObject);
        AttachedWeapon = null;
        IsEmpty = true;
    }

    public void FireWeapon(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        if (!IsEmpty)
        {
            AttachedWeapon.AttemptFire(isEnemy, fireRateModifier, damageModifier, bulletSpeedModifier);
        }
    }
}
