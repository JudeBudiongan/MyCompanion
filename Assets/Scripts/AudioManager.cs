using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------Audio Source-----------")]
    [SerializeField] AudioSource musicSource;

    [Header("--------Audio Clips-----------")]
    public AudioClip background;

  private static AudioManager instance;

    private void Awake() {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this as the instance and make it persistent
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy the duplicate
            Destroy(gameObject);
            return;
        }

        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;  // Set loop to true to make the audio clip repeat infinitely
            musicSource.Play();
        }
    }
}
