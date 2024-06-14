using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public enum RelativeSide
    {
        Left,
        Right,
        Center
    }

    public GameObject PreAttachedWeapon;
    public WeaponBase AttachedWeapon;

    private RelativeSide Side;
    private bool isSlotFilled = false;

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

        if (PreAttachedWeapon != null)
        {
            AttachedWeapon = PreAttachedWeapon.GetComponent<WeaponBase>();
            AttachedWeapon.Side = Side;
            isSlotFilled = true;
        }
    }

    public void AttachWeapon(GameObject weaponPrefab, bool forceAttach)
    {
        if (isSlotFilled)
        {
            if (forceAttach == true)
            {
                DetachWeapon();
            }
            else
            {
                Debug.LogWarning("The weapon slot is already filled.");
                return;
            }
        }

        GameObject weaponObject = Instantiate(weaponPrefab, transform.position, transform.rotation, transform);
        WeaponBase weapon = weaponObject.GetComponent<WeaponBase>();

        if (weapon != null)
        {
            // Set the Side property of the weapon based on the AttachPoint
            weapon.Side = Side;

            AttachedWeapon = weapon;
            isSlotFilled = true;
        }
        else
        {
            Debug.LogError("The weaponPrefab does not have a WeaponBase component.");
            Destroy(weaponObject);
        }
    }

    public void DetachWeapon()
    {
        if (!isSlotFilled)
        {
            Debug.LogWarning("No weapon is attached to this slot.");
            return;
        }

        // Destroy the GameObject the WeaponBase script is attached to
        Destroy(AttachedWeapon.gameObject);
        AttachedWeapon = null;
        isSlotFilled = false;
    }
}
