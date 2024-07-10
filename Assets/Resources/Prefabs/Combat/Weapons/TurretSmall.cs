using UnityEngine;

public class TurretSmall : SingleFireWeaponBase
{
    private Transform target;
    private float rotationSpeed = 5f; // Adjust this value to change rotation speed
    public float MaxTargetRange = 6f;

    protected override void Update()
    {
        base.Update();
        FindTarget();
        AimAtTarget();
    }

    private void FindTarget()
    {
        if (target == null || target.gameObject == null)
        {
            EnemyShip[] enemies = FindObjectsOfType<EnemyShip>();
            if (enemies.Length > 0)
            {
                // Find the closest enemy
                foreach (EnemyShip enemy in enemies)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < MaxTargetRange)
                    {
                        target = enemy.transform;
                        break;
                    }
                }
            }
            else
            {
                target = null;
            }
        }
    }

    private void AimAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Point forward if no target is found
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
    }
}
