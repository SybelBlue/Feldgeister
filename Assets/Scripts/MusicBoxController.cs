using UnityEngine;

public class MusicBoxController : MonoBehaviour
{
    public static MusicBoxController instance { get; private set; }

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

    void OnValidate()
    {
        mainSource = GetComponent<AudioSource>();
    }
}
