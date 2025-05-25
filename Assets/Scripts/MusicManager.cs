using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicTracks;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    private AudioSource audioSource;
    private int lastTrackIndex = -1;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    private void Start()
    {
        PlayNextTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && musicTracks.Count > 0)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        int nextIndex = GetRandomTrackIndex();
        AudioClip nextClip = musicTracks[nextIndex];
        lastTrackIndex = nextIndex;

        audioSource.clip = nextClip;
        audioSource.Play();

        StartFade(volume);
    }

    private int GetRandomTrackIndex()
    {
        if (musicTracks.Count <= 1) return 0;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, musicTracks.Count);
        } while (newIndex == lastTrackIndex);

        return newIndex;
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource.isPlaying)
        {
            StartFade(volume);
        }
    }

    public void Mute(bool isMuted)
    {
        SetVolume(isMuted ? 0f : volume);
    }

    public void StartFade(float targetVolume)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTo(targetVolume, fadeDuration));
    }

    private IEnumerator FadeTo(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void StopMusicWithFade()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndStop(fadeDuration));
    }

    private IEnumerator FadeAndStop(float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;
    }
}