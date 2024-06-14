using UnityEngine;

public abstract class ItemDrop : MonoBehaviour
{
    private EffectData AssignedEffectData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerEffect(collision.gameObject);
            Destroy(gameObject);
        }
    }
    
    public void InitialiseItem(Effect effectName)
    {
      if (GameConfig.EffectDataDictionary.TryGetValue(effectName, out var effectData))
      {
        AssignedEffectData = effectData;
      }
    }

    protected void TriggerEffect(GameObject ship)
    {
      EffectsManager.Inst.ActivateEffect(ship, AssignedEffectData);
    }
}
