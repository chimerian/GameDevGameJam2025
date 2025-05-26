using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> sounds;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        if (sounds == null || sounds.Count == 0)
        {
            return;
        }

        int index = Random.Range(0, sounds.Count);
        AudioClip selectedClip = sounds[index];

        audioSource.PlayOneShot(selectedClip);
    }
}
