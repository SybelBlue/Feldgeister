using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TopMenuController : MonoBehaviour
{
    public int food = 3;
    public TMP_Text foodRemaining;
    public UnityEvent hideResources;
    public UnityEvent hideDefenses;
    public UnityEvent changeToDusk;
    public GameController gameController;

    void OnStart()
    {
        gameController.foodRemaining = 3;
    }
   
   //reduces food by one, should be called after speaking with a character
    public void ReduceFood()
    {
        foodRemaining.text = "Food Remaining: " + --food;
        if(food < 1)
        {
            changeToDusk.Invoke();
            print("***changing to dusk***");
        }
    }
//resets food remaining text (needs to eventually update food variable as well)
    public void ResetFood()
    {
        //Todo: update food variable here
        foodRemaining.text = "Food Remaining: " + food;
    }
    //hides resource and defense lists but keeps top menu
    public void HideLists()
    {
        hideResources.Invoke();
        hideDefenses.Invoke();
    }
    //hides both the lists and the top menu itself
    public void HideAll()
    {
        this.gameObject.SetActive(false);
    }
    //shows just the top menu and not the lists
    public void ShowTop()
    {
        this.gameObject.SetActive(true);
        hideResources.Invoke();
        hideDefenses.Invoke();
    }
}
