using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public enum RelativeSide
    {
        Left,
        Right,
        Center
    }

    public RelativeSide Side;
    public WeaponBase AttachedWeapon;

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
    public void AttachWeapon(GameObject weaponPrefab, int quantity)
  {
      int slotsFilled = 0;

      foreach (var slot in WeaponSlots)
      {
          if (slotsFilled >= quantity)
              break;

          // Check if the slot is empty
          if (slot.childCount == 0)
          {
              GameObject weaponObject = Instantiate(weaponPrefab, slot.position, slot.rotation, slot);
              WeaponBase weapon = weaponObject.GetComponent<WeaponBase>();

              if (weapon != null)
              {
                  // Get the AttachPoint component from the slot
                  AttachPoint attachPoint = slot.GetComponent<AttachPoint>();

                  if (attachPoint != null)
                  {
                      // Set the Side property of the weapon based on the AttachPoint
                      weapon.Side = attachPoint.Side;
                  }

                  AttachedWeapons.Add(weapon);
                  slotsFilled++;
              }
              else
              {
                  Debug.LogError("The weaponPrefab does not have a WeaponBase component.");
                  Destroy(weaponObject);
              }
          }
      }

      if (slotsFilled < quantity)
      {
          Debug.LogWarning("Not enough empty weapon slots to attach all weapons.");
      }
  }
}
