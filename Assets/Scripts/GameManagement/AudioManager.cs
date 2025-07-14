using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioLibrary mainAudioLibrary;
    [SerializeField] private AudioLibrary characterAudioLibrary;
    [SerializeField] private float fadeDuration = 0.7f;

    private Coroutine fadeCoroutine;


    private void Start()
    {
        GameManager.Instance.RegisterAudioManager(this);
    }

    public void PlayCharacterFx(string soundID)
    {
        playerSource.PlayOneShot(characterAudioLibrary.GetAudioClip(soundID));
    }  

    public void PlayLevelMusic(string levelID)
    {
        if (mainSource.isPlaying)
        {
            Crossfade(mainAudioLibrary.GetAudioClip(levelID));
        }
        else
        {
            mainSource.clip = mainAudioLibrary.GetAudioClip(levelID);
            mainSource.Play();
        } 
    }

    private void Crossfade(AudioClip newClip, float targetVolume = 1f)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        {
            fadeCoroutine = StartCoroutine(FadeToNewClip(newClip, targetVolume));
        }  
    }

    private IEnumerator FadeToNewClip(AudioClip newClip, float targetVolume)
    {
        float startVolume = mainSource.volume;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            mainSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        mainSource.Stop();
        mainSource.clip = newClip;
        mainSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            mainSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }

        mainSource.volume = targetVolume;
    }
}
