using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoraleDisplay : MonoBehaviour
{
    public TMP_Text morale;
    public AudioSource audioSource;
    public AudioClip moraleUp;
    public AudioClip moraleDown;
    
    public void IncreaseMorale()
    {
        //from https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/#play, 
        //https://answers.unity.com/questions/1327656/how-to-change-the-color-of-the-textmeshpro-gui-com.html,
        morale.color = new Color(50,150,50,255); //green
        morale.text = "+ morale";
        audioSource.PlayOneShot(moraleUp);
        while(audioSource.isPlaying){
            print("still playing");
        }
        morale.text = "";
    }
    public void DecreaseMorale() 
    {
        morale.color = new Color(150,50,50,255); //red
        morale.text = "- morale";
        audioSource.PlayOneShot(moraleDown);
        while(audioSource.isPlaying){
            print("still playing");
        }
        morale.text = "";
    }
}
