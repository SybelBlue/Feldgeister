using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class IntroRunner : MonoBehaviour
{
    public GameObject ewald;
    public GameObject cornKing;
    public GameObject plumWolf;
    public GameObject weatherCat;
    public GameObject rooster;
    
    [YarnCommand("addEwald")]
    public void addEwald()
    {
        ewald.SetActive(true);
    }
    [YarnCommand("addCornKing")]
    public void addCornKing()
    {
        cornKing.SetActive(true);
    }
    [YarnCommand("addPlumWolf")]
    public void addPlumWolf()
    {
        plumWolf.SetActive(true);
    }
    [YarnCommand("addWeatherCat")]
    public void addWeatherCat()
    {
        weatherCat.SetActive(true);
    }
    [YarnCommand("addRooster")]
    public void addRooster()
    {
        rooster.SetActive(true);
    }
    [YarnCommand("MoveToTitle")]
    public void MoveToTitle()
    {
        //from https://forum.unity.com/threads/move-to-another-scene.383653/
        SceneManager.LoadScene("Title Screen");
    }
}
