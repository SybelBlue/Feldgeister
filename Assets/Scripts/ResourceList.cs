using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList : MonoBehaviour
{
    public void toggleResources()
    {
        this.setActive(!activeSelf);
    }
}
