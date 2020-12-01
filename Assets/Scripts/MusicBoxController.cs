using UnityEngine;

public class MusicBoxController : MonoBehaviour
{
    // taken from
    // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
    //
    // Will ensure object is persistent singleton
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music Box");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
