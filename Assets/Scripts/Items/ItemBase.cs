using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerEffect(collision.gameObject);
            Destroy(gameObject);
        }
    }

    protected abstract void TriggerEffect(GameObject player);
}
