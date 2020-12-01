using UnityEngine;
using System.Collections;

public class MusicBoxController : MonoBehaviour
{
    public static MusicBoxController instance { get; private set; }

    [SerializeField]
    private AudioClip dayAudioClip, nightAudioClip;

    [SerializeField, ReadOnly]
    private AudioSource mainSource;

    // taken from
    // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    //
    // Will ensure object is persistent singleton
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
    }

    void Start()
    {
        mainSource.clip = dayAudioClip;
        StartCoroutine(FadeIn(mainSource, 3));
    }

    void OnValidate()
    {
        mainSource = GetComponent<AudioSource>();
    }

    // taken from
    // https://forum.unity.com/threads/fade-out-audio-source.335031/
    private static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return true;
        }
 
        audioSource.Stop();
        audioSource.volume = startVolume;
        yield return false;
    }

    private static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float? targetVolume = null) {
        float finalVolume = targetVolume ?? audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
 
        while (audioSource.volume < Mathf.Min(1, finalVolume)) {
            audioSource.volume += finalVolume * Time.deltaTime / FadeTime;
 
            yield return true;
        }

        audioSource.volume = finalVolume;
        yield return false;
    }
}
