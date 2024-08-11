using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Inst { get; private set; }
    private List<AudioSource> audioSources = new List<AudioSource>();

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("MusicManager already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;

        audioSources.AddRange(GetComponents<AudioSource>());
    }

  public void PlayAudioFile(string clipName, float pitch = 1.0f)
  {
      // Find an available audio source
      AudioSource availableSource = null;
      foreach (AudioSource source in audioSources)
      {
          if (!source.isPlaying)
          {
              availableSource = source;
              break;
          }
      }
  
      if (availableSource == null)
      {
          Debug.LogError("No available audio sources to play the clip.");
          return;
      }
  
      // Get the audio clip from the AssetManager
      if (AssetManager.AudioClips.TryGetValue(clipName, out AudioClip clip))
      {
          availableSource.clip = clip;
          availableSource.pitch = pitch; // Set the pitch of the audio source
          availableSource.Play();
      }
      else
      {
          Debug.LogError($"Audio clip '{clipName}' not found in the AssetManager.");
      }
  }

}
