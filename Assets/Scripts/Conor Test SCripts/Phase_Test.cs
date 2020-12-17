using UnityEngine;
using Yarn.Unity;

public class Phase_Test : MonoBehaviour
{
    public int run_number;
    public GameController _gameController;
    public GameController gameController
        => _gameController ?? (_gameController = GetComponent<GameController>());

    public DialogueRunner _dialogueRunner;
    public DialogueRunner dialogueRunner
        => _dialogueRunner ?? (_dialogueRunner = FindObjectOfType<DialogueRunner>());

    public YarnProgram Dusk;
    public YarnProgram Night;
    public YarnProgram Dawn;
    public YarnProgram Dawn_2;
    public string phase_talkToNode = "";

    private void StartDialogue()
        => dialogueRunner.StartDialogue(phase_talkToNode);

    public void RunDawnDialogue()
    {
        //run_number ++;
        //print("run_number" + run_number);
        if (phase_talkToNode != "dawn_start")
        {
            phase_talkToNode = "dawn_start";
        }
        dialogueRunner.Add(Dawn);
        if(Dawn.baseLocalisationStringTable == null)
        {
		dialogueRunner.AddStringTable(Dawn);
        }
        StartDialogue();
    }

    public void RunDawnDialogueTwo()
    {
        print("run_number" + run_number);
        if (phase_talkToNode != "dawn_start_2")
        {
            phase_talkToNode = "dawn_start_2";
        }
        dialogueRunner.Add(Dawn_2);
	    if(Dawn_2.baseLocalisationStringTable == null)
        {
		dialogueRunner.AddStringTable(Dawn_2);
        }
        StartDialogue();
    }
    public void RunDuskDialogue()
    {
        if (phase_talkToNode != "dusk_start")
        {
        phase_talkToNode = "dusk_start";
        }
        dialogueRunner.Add(Dusk);
        if(Dusk.baseLocalisationStringTable == null)
        {
		dialogueRunner.AddStringTable(Dusk);
        }
        StartDialogue();
    }

    public void RunNightDialogue()
    {
        if (phase_talkToNode != "night_start")
        {
        phase_talkToNode = "night_start";
        }
        dialogueRunner.Add(Night);
        if(Night.baseLocalisationStringTable == null)
        {
		dialogueRunner.AddStringTable(Night);
        }
        StartDialogue();
    }
}
