using UnityEngine;
using Yarn.Unity;

public class Phase_Test : MonoBehaviour
{
    public GameController _gameController;
    public GameController gameController
        => _gameController ?? (_gameController = GetComponent<GameController>());

    public DialogueRunner _dialogueRunner;
    public DialogueRunner dialogueRunner
        => _dialogueRunner ?? (_dialogueRunner = FindObjectOfType<DialogueRunner>());

    public YarnProgram Dusk;
    public YarnProgram Night;
    public YarnProgram Dawn;
    public string phase_talkToNode = "";

    private void StartDialogue()
        => dialogueRunner.StartDialogue(phase_talkToNode);

    public void RunDawnDialogue()
    {
        phase_talkToNode = "dawn_start";
        dialogueRunner.Add(Dawn);
        StartDialogue();
    }

    public void RunDuskDialogue()
    {
        phase_talkToNode = "dusk_start";
        dialogueRunner.Add(Dusk);
        StartDialogue();
    }

    public void RunNightDialogue()
    {
        phase_talkToNode = "night_start";
        dialogueRunner.Add(Night);
        StartDialogue();
    }

}
