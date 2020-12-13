using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroRunner : MonoBehaviour
{
    public GameObject ewald;
    public GameObject cornKing;
    public GameObject plumWolf;
    public GameObject weatherCat;
    public GameObject rooster;
    // Start is called before the first frame update
    void Start()
    {
        ewald.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTitle()
    {
        //from https://forum.unity.com/threads/move-to-another-scene.383653/
        SceneManager.LoadScene("Title Screen");
    }
}
