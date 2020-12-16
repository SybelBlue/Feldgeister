using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DuskTopMenuController : MonoBehaviour
{
    public TMP_Text foodRemaining;
    public UnityEvent hideResources;
    public UnityEvent hideDefenses;
    public UnityEvent changeToDusk;
   
   //reduces food by one, should be called after speaking with a character
    public void ReduceFood()
    {
        
    }
//resets food remaining text (needs to eventually update food variable as well)
    public void ResetFood()
    {
        
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
