using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemDrop : MonoBehaviour
{
    private EffectData AssignedEffectData;

    [SerializeField]
    private float Speed = 1.0f; // Speed of the downward movement, accessible in the inspector

    private Rigidbody2D rb;

    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      rb.velocity = new Vector2(0, -Speed); // Set the initial downward velocity
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.gameObject.CompareTag("Player"))
      {
        TriggerEffect(collision.gameObject);
        Destroy(gameObject);
      }
    }
    
    public void InitialiseItem(EffectType type, EffectSubType subType)
    {
      AssignedEffectData = GameConfig.EffectDataList.Find(x => x.Type == type && x.SubType == subType);

      if (AssignedEffectData == null)
      {
        Debug.LogError("EffectData not found");
        Destroy(gameObject);
      }
    }

    protected void TriggerEffect(GameObject ship)
    {
      EffectsManager.ActivateEffect(ship, AssignedEffectData);
    }
}
