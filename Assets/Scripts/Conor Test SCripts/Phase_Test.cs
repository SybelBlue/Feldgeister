using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Phase_Test : MonoBehaviour
{
    public GamePhase phase => GetComponent<GameController>().phase;
    public YarnProgram Dusk;
    public YarnProgram Night;
    public YarnProgram Dawn;
    public string phase_talkToNode = "";

    public void RunPhaseDialogue()
    {
        if (phase == GamePhase.Dusk) {
            phase_talkToNode = "dusk_start";
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(Dusk);
            FindObjectOfType<DialogueRunner>().StartDialogue(phase_talkToNode);
        }

        if (phase == GamePhase.Night) {
            phase_talkToNode = "night_start";
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(Night);
            FindObjectOfType<DialogueRunner>().StartDialogue(phase_talkToNode);
        }
        
        if (phase == GamePhase.Dawn) {
            phase_talkToNode = "dawn_start";
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(Dawn);
            FindObjectOfType<DialogueRunner>().StartDialogue(phase_talkToNode);
        }
    }

}
