using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseList : MonoBehaviour
{
    public void toggleDefenses()
    {
        DefenseList.setActive(!activeSelf);
    }
}
