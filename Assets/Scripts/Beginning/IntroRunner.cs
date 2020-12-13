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
    
    public void addEwald()
    {
        ewald.SetActive(true);
    }
    public void addCornKing()
    {
        cornKing.SetActive(true);
    }
    public void addPlumWolf()
    {
        plumWolf.SetActive(true);
    }
    public void addWeatherCat()
    {
        weatherCat.SetActive(true);
    }
    public void addRooster()
    {
        rooster.SetActive(true);
    }

    public void MoveToTitle()
    {
        //from https://forum.unity.com/threads/move-to-another-scene.383653/
        SceneManager.LoadScene("Title Screen");
    }
}
