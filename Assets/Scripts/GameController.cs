using UnityEngine;
using System.Collections.Generic;
using System;

#pragma warning disable 0649
public class GameController : MonoBehaviour
{
    [Serializable]
    public enum GamePhase
    {
        Dawn,
        Day,
        Dusk,
        Night,
    }

    private static int phaseCount = Enum.GetValues(typeof(GamePhase)).Length;

    public GamePhase phase = GamePhase.Night;

    [SerializeField]
    private AudioClip buttonHoverClip, buttonSelectClip;
    private MapGenerator mapGenerator;
    private RegionManager<Building> buildingMap;

    public CameraController cameraController
        => Camera.main.GetComponent<CameraController>();

    public bool cameraLocked
    {
        get => cameraController.lockPosition;
        set => cameraController.lockPosition = value;
    }

    public bool mapReady { get => mapGenerator != null; }

    public Yarn.Unity.DialogueRunner dialogueRunner;
    
    public HouseOccupant houseOccupantUI;

    public CharacterDisplayController leftCharacterDisplay, rightCharacterDisplay;
    public Character mayorCharacter;

    [Range(0, 1)]
    public float UIVolume;

    public bool runOpeningDialogue;

    private List<House> houses;

    public House strategicTarget, randomTarget;

    public AttackStrategy strategy;

    [SerializeField, ReadOnly, Tooltip("For inspector debugging use only.")]
    private CharacterJob strategicTargetJob, randomTargetJob;

    private static int jobCount = System.Enum.GetValues(typeof(CharacterJob)).Length;
    private bool[] charactersTalkedToday = new bool[jobCount];

#pragma warning disable 0414
    [SerializeField, ReadOnly]
    private bool _mapReady = false;

    public void RunPhase(GamePhase phase)
    {
        this.phase = phase;

        switch (phase)
        {
            case GamePhase.Dawn:
                charactersTalkedToday = new bool[jobCount];

                strategy = new List<AttackStrategy>(StaticUtils.allStrategies).RandomChoice();
                strategicTarget = StaticUtils.HouseForStrategy(strategy, houses);
                strategicTargetJob = strategicTarget.character.job;

                randomTarget = houses.RandomChoice();
                randomTargetJob = randomTarget.character.job;
                print("TODO: watcher says attack strategy");
                // use this to transition to day after watcher dialogue finishes
                // AutoPhaseAdvanceOnDialogueComplete();
                break;
            case GamePhase.Day:
                print("TODO: update character food and morale stats");
                print("TODO: display defense and resource dropdowns");
                print("TODO: change selection mode to allow hose calls and food donation");
                break;
            case GamePhase.Dusk:
                print("TODO: get defenses from blacksmith");
                print("TODO: change selection mode to place defenses");
                print("TODO: change selection mode to place lamb");
                break;
            case GamePhase.Night:
                MonsterAttack();
                print("TODO: show feldgeister on screen");
                print("TODO: display attack dialogue");
                break;
        }
    }

    public void AdvancePhase()
    {
        var nextPhase = (GamePhase)(((int)phase + 1) % phaseCount);
        RunPhase(nextPhase);
    }

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        houses = new List<House>(mapGenerator.GetComponents<House>());
        _mapReady = true;
    }

    void Update()
    {
        // required so that the feldgeister input system is up to date each frame
        // do not change __
        Feldgeister.Input.Update(buildingMap.Get);
        // do not change ^^

        rightCharacterDisplay.DisplayCharacter(cameraLocked ? mayorCharacter : null);

        if (!cameraLocked && Feldgeister.Input.lastFocused is House)
        {
            var house = Feldgeister.Input.lastFocused as House;
            houseOccupantUI.UpdateDisplay(house.occupant, !HasTalkedToday(house.occupant));
            leftCharacterDisplay.DisplayCharacter(house.occupant);
        }
        else
        {
            houseOccupantUI.UpdateDisplay(null);
            if (!cameraLocked)
            {
                leftCharacterDisplay.DisplayCharacter(null);
            }
        }
    }

    public void MonsterAttack()
    {
        strategicTarget.defenseLevel--;
        randomTarget.defenseLevel--;
    }

    public void OnCharacterDied(Character c)
    {
        print($"he ded: {c}");
    }

    public void BeginCharacterDialogue(Character character)
    {
        if (!character)
        {
            print("Rejected dialogue start, null character.");
            return;
        }

        if (cameraLocked) 
        {
            print("Rejected dialogue start, dialogue already running.");
            return;
        }

        if (HasTalkedToday(character))
        {
            print("Rejected dialogue start, already talked today.");
            return;
        }

        var npcConor = character?.GetComponent<NPC_Conor>();
        if (!npcConor || !npcConor.enabled)
        {
            print("Rejected dialogue start, no active NPC_Conor found.");
            return;
        }

        npcConor.RunDialogue();
        cameraLocked = true;

        MarkTalkedToday(character);
    }

    private void MarkTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job] = true;
    
    private bool HasTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job];

    public void BeginOpeningDialogue()
    {
#if UNITY_EDITOR
        if (runOpeningDialogue)
#endif
        {
            GetComponent<NPC_Conor>()?.RunDialogue();
            print("delaying game start till after opening dialogue");
            AutoPhaseAdvanceOnDialogueComplete();
            cameraLocked = true;
        }
#if UNITY_EDITOR
        else
        {
            cameraLocked = false;
            AdvancePhase();
        }
#endif
    }

    public void AutoPhaseAdvanceOnDialogueComplete()
    {
        dialogueRunner.onDialogueComplete.AddListener(_AdvanceOnDialogueComplete);
    }

    public void _AdvanceOnDialogueComplete()
    {
        AdvancePhase();
        dialogueRunner.onDialogueComplete.RemoveListener(_AdvanceOnDialogueComplete);
    }

    public void PlayButttonHover()
    {
        var source = GetComponent<AudioSource>();
        if (!source) return;
        source.clip = buttonHoverClip;
        source.volume = UIVolume;
        source.Play();
    }

    public void PlayButttonSelect()
    {
        var source = GetComponent<AudioSource>();
        if (!source) return;
        source.clip = buttonSelectClip;
        source.volume = UIVolume;
        source.Play();
    }

    public void SetCameraLock(bool locked)
        => Camera.main.GetComponent<CameraController>().lockPosition = locked;
}