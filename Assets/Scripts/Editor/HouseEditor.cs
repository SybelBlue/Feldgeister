
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

#pragma warning disable 0618
[CustomEditor(typeof(Character))]
public class HouseEditor : Editor
{
    private House house => target as House;
    public override void OnInspectorGUI()
    {
        if (!house) 
        {
            Debug.LogError("House null!");
            return;
        }

        base.OnInspectorGUI();

        var oldLevel = house.defenseLevel;
        var newLevel = EditorGUILayout.IntField("Defense Level", oldLevel);
        if (newLevel < 0)
        {
            Debug.LogWarning("Cannot decrease defense level past 0.");
            house.defenseLevel = 0;
        }
        else if (oldLevel > newLevel)
        {
            house.defenseLevel = newLevel;
        }
        else if (oldLevel < newLevel)
        {
            Debug.LogWarning("Cannot increase defense level directly. \nAdd a defense object to the defenses list instead.");
        }

        using (new DisabledGroup())
        {
            if (!house.occupant)
            {
                house.ResetCharactersArray();
            }
            EditorGUILayout.ObjectField("Occupant", house.occupant, typeof(Character));
        }
    }
}
#endif