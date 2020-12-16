using UnityEngine;
using Yarn.Unity;
using UnityEngine.Events;


/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.

public class NPC_Conor : MonoBehaviour {

    public GameController gameController = GetComponent<GameController>();
    
    public string strategicAttackTarget;

    public string randomAttackTarget; 
    
    public string characterName = "";

    public string day_1_talkToNode = "";

    public string day_2_talkToNode = "";

    public UnityEvent moraleUp;

    public UnityEvent moraleDown;

    // public AttackStrategy strategy
    // {
    //     get => 
    // }

    [Header("Optional")]
    public YarnProgram day_1;
    public YarnProgram day_2;

    [SerializeField, ReadOnly]
    private Character _linkedCharacter;
    public Character linkedCharacter
        => _linkedCharacter ?? (_linkedCharacter = GetComponent<Character>());

    [YarnCommand("decrease_morale")]
    public void Decrease_Morale(){
        linkedCharacter.DecreaseMorale();
        print(linkedCharacter.moraleValue);
        moraleDown.Invoke();
    }

    [YarnCommand("increase_morale")]
    public void Increase_Morale(){
        linkedCharacter.IncreaseMorale();
        print(linkedCharacter.moraleValue);
        moraleUp.Invoke(); 
    }
    public int houseDefenseLevel 
        => linkedCharacter.house.defenseLevel;

    public void RunDialogue()
    {
        FindObjectOfType<DialogueRunner>().StartDialogue(day_1_talkToNode);
    }

    void Start ()
    {
        if (day_2 != null && gameController.day_number == 2)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(day_2);
        }  
        else if (day_1 != null && gameController.day_number == 1)
        {       
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(day_1);
        }
    }
}

//Help























/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/