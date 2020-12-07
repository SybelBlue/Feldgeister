using UnityEngine;
using System.Collections;

public class MusicBoxController : MonoBehaviour
{
    public static MusicBoxController instance { get; private set; }

    [SerializeField]
#pragma warning disable 0649
    private AudioClip dayAudioClip, nightAudioClip;

    [SerializeField, ReadOnly]
    private AudioSource mainSource;

    [SerializeField, ReadOnly]
    private bool inTransition;

    [Range(0, 1)]
    public float maxVolume;

    // Will ensure object is persistent singleton
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        mainSource.clip = dayAudioClip;
        mainSource.volume = maxVolume;
        StartCoroutine(FadeIn(mainSource, 3));
    }

    void Update()
    {
        if (inTransition) return;

        mainSource.volume = maxVolume;
    }

    void OnValidate()
    {
        mainSource = GetComponent<AudioSource>();
    }

    // taken from
    // https://forum.unity.com/threads/fade-out-audio-source.335031/
    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        inTransition = true;
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return true;
        }
 
        audioSource.Stop();
        audioSource.volume = startVolume;
        yield return false;
        inTransition = false;
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float? targetVolume = null) {
        inTransition = true;
        float finalVolume = targetVolume ?? audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
 
        while (audioSource.volume < Mathf.Min(1, finalVolume))
        {
            audioSource.volume += finalVolume * Time.deltaTime / FadeTime;
 
            yield return true;
        }

        audioSource.volume = finalVolume;
        yield return false;
        inTransition = false;
    }
}
