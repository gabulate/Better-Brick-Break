using System.Collections;
using UnityEngine;


public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    public SoundClip clip;
    [SerializeField]
    private bool repeating = false;

    /// <summary>
    /// Plays a sound to be heard everywhere.
    /// </summary>
    /// <param name="soundClip">The soundClip to be played.</param>
    public void PlayFX(SoundClip soundClip)
    {
        source.spatialBlend = 0.0f;

        clip = soundClip;
        PlayFX();
    }

    /// <summary>
    /// Plays a sound with 3D spatial blend.
    /// </summary>
    /// <param name="soundClip">The soundClip to be played.</param>
    /// <param name="position">The position from where the sound will be heard.</param>
    public void PlayFX(SoundClip soundClip, Vector3 position)
    {
        transform.position = position;
        source.spatialBlend = 0.8f;

        clip = soundClip;
        PlayFX();
    }

    private void PlayFX()
    {
        source.clip = clip.clip;
        source.volume = clip.volume * SaveSystem.csd.volume;

        VaryPitch();
        source.Play();
        StartCoroutine(DisableObject());
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    /// <summary>
    /// Stops the sound or music being played.
    /// </summary>
    public void Stop()
    {
        source.Stop();
        repeating = false;
    }

    public IEnumerator PlayRepeating(SoundClip soundClip, float timeBetween)
    {
        source.spatialBlend = 0.0f;

        clip = soundClip;

        WaitForSeconds waitSeconds = new WaitForSeconds(timeBetween);

        while (repeating)
        {
            PlayFX();
            yield return waitSeconds;
        }
    }

    public IEnumerator PlayRepeating(SoundClip soundClip, float timeBetween, Transform transform)
    {
        source.spatialBlend = 0.8f;

        clip = soundClip;
        repeating = true;

        WaitForSeconds waitSeconds = new WaitForSeconds(timeBetween);

        while (repeating)
        {
            transform.position = transform.position;
            PlayFX();
            yield return waitSeconds;
        }
    }

    private IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(source.clip.length + 1);
        gameObject.SetActive(false);
    }

    private void VaryPitch()
    {
        if (clip.varyPitch)
        {
            source.pitch = Random.Range(clip.minPitch, clip.maxPitch);
        }
        else
        {
            source.pitch = clip.pitch;
        }
    }

    private void OnPause(bool paused)
    {
        if (paused) Pause();
        else UnPause();

        source.volume = clip.volume * SaveSystem.csd.volume;
        
    }

    private void OnEnable()
    {

        if (GameManager.Instance)
            GameEvents.e_gamePaused.AddListener(OnPause);
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameEvents.e_gamePaused.RemoveListener(OnPause);
    }
}
