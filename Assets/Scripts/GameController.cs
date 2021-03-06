﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Yarn.Unity;



[Serializable]
public enum GamePhase
{
    Dawn,
    Day,
    Dusk,
    Night,
}

#pragma warning disable 0649
public class GameController : MonoBehaviour
{
    [ReadOnly]
    public int day_number;
   //CONOR FUTZING AGAIN
    //VariableStorage varStore = DialogueRunner.GetComponent<VariableStorage>();
    //CONOR FUTZING AGAIN
    
    private static int phaseCount = Enum.GetValues(typeof(GamePhase)).Length;

    public GamePhase phase = GamePhase.Night;

    public GameObject endDialogue;
    public GameObject endFoodDonation;
    public GameObject endDefensePlacement;
    public GameObject foodDonationScreen;
    public GameObject lambPlacementScreen;
    public GameObject defensePlacementScreen;
    public GameObject dayTopMenu;
    public GameObject hungerList;
    public ResourceList resourceList;
    public GameObject duskTopMenu;
    public GameObject dawnTwoWindow;
    
    private ISelectionMode _selectionMode;
    private ISelectionMode selectionMode
    {
        get => _selectionMode;
        set
        {
            if (_selectionMode != null)
            {
                _selectionMode.OnEndSelectionMode();
            }
            print("Changing to selection mode: " + value?.name);
            _selectionMode = value;
            _selectionMode.OnBeginSelectionMode();
        }
    }

    [SerializeField]
    private AudioClip buttonHoverClip, buttonSelectClip;
    private MapGenerator mapGenerator;
    private RegionManager<Building> buildingMap;

    private CameraController _cameraController;
    public CameraController cameraController
        => _cameraController ?? (_cameraController = Camera.main.GetComponent<CameraController>());

    public GameObject characterContainer;

    private Phase_Test _phaseTest;
    private Phase_Test phaseTest
        => _phaseTest ?? (_phaseTest = GetComponent<Phase_Test>());

    public bool cameraLocked
    {
        get => cameraController.lockPosition;
        set => cameraController.lockPosition = value;
    }

    public DialogueRunner dialogueRunner;
    
    public TopMenuController topMenuUI;
    public DuskTopMenuController duskMenuUI;

    public HouseOccupant houseOccupantUI;

    public CharacterDisplayController leftCharacterDisplay, rightCharacterDisplay;
    public Character mayorCharacter;
    public Character blacksmith;

    public NPC_Conor watcherNPC;

    [Range(0, 1)]
    public float UIVolume;

    public bool runOpeningDialogue;

    public bool runningDialogue
    {
        get => cameraLocked;
        set => cameraLocked = value;
    }

    private List<House> houses;

    public House strategicTarget, randomTarget;

    public AttackStrategy strategy;

    [SerializeField, ReadOnly, Tooltip("For inspector debugging use only.")]
    private CharacterJob strategicTargetJob, randomTargetJob;

    [SerializeField, ReadOnly]
    
    private int _foodRemaining;
    public int foodRemaining
    {
        get => _foodRemaining;
        set
        {
            topMenuUI.food = value;
            _foodRemaining = value;
            topMenuUI.ResetFood();
        }
    }

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        houses = new List<House>(mapGenerator.GetComponents<House>());
    }
//Need to implement a counter that tells us how many phases we have been through in order to start yarn in right places
    public void RunPhase(GamePhase phase)
    {
        this.phase = phase;

        switch (phase)
        {   
            //Note from Conor: defining the strategies at dawn, prior to placing defenses, 
            case GamePhase.Dawn: 
                day_number ++;
                print("DAWN NUMBER " + day_number);
                print(day_number);
                foodRemaining = 3;
                topMenuUI.ResetFood();
                print("UI menu reset");
                //topMenuUI.UpdateAll();
                dialogueRunner.variableStorage.SetValue("$day_number", new Yarn.Value(day_number));
                strategy = new List<AttackStrategy>(StaticUtils.allStrategies).RandomChoice();
                strategicTarget = StaticUtils.HouseForStrategy(strategy, houses);
                strategicTargetJob = strategicTarget.character.job;
                randomTarget = houses.RandomChoice();
                randomTargetJob = randomTarget.character.job;
                // watcherNPC.strategicAttackTargetChar = Enum.GetName(typeof(CharacterJob),strategicTargetJob);
                watcherNPC.strategicAttackTarget = Enum.GetName(typeof(AttackStrategy),strategy);
                watcherNPC.randomAttackTarget = Enum.GetName(typeof(CharacterJob),randomTargetJob);
                dialogueRunner.variableStorage.SetValue("$strategic_attack", new Yarn.Value(Enum.GetName(typeof(AttackStrategy),strategy)));
                dialogueRunner.variableStorage.SetValue("$random_attack", new Yarn.Value(Enum.GetName(typeof(CharacterJob),randomTargetJob)));
                AutoAdvancePhaseOnDialogueComplete();
                print("got to the end of this long guy");
                runningDialogue = true;
                //adding to seperate out the two dawn dialogues as it seems upset when we roll over into a new day
                if (day_number == 1)
                {
                    phaseTest.RunDawnDialogue();
                    print("running dialogue 1");

                }
                if (day_number == 2)
                {
                    phaseTest.RunDawnDialogueTwo();
                    dawnTwoWindow.SetActive(true);
                    print("running dialogue 2");
                }
                print("yikes why are we here");

                foreach (var character in characterContainer.GetComponentsInChildren<Character>())
                {
                    dialogueRunner.variableStorage.SetValue($"${character.job}_defense".ToLower(), new Yarn.Value(character.house?.defenseLevel ?? 0));
                }
                foreach (var npc_conor in characterContainer.GetComponentsInChildren<NPC_Conor>())
                {
                    npc_conor.SetDayDialogue(day_number);
                }
                break;
            case GamePhase.Day:
                // 
                print("TODO: update character food and morale stats"); // katia
                print("TODO: display defense and resource dropdowns"); // katia
                selectionMode = new DialogueSelectionMode(this);
                print("TODO: show button to change selection mode to allow food donation"); // katia
                // make button call GameController.FinishDialogueSelection(),
                // will advance to food placement
                break;
            case GamePhase.Dusk:
                duskMenuUI.UpdateAll();
                runningDialogue = true;
                phaseTest.RunDuskDialogue();
                AddWeapons();
                selectionMode = new LambSelectionMode(this);
                // defense placement mode will start after lamb is placed
                // make button call GameController.FinishDefenseSelection(),
                // will advance to night
                break;
            case GamePhase.Night:
                //moved here from dawn in order to ensure that it registers who is actually the strongest character available, though setting strategy
                //earlier on may end up not actually reflecting here -- Conor
                dialogueRunner.variableStorage.SetValue("$strategic_attack_character", new Yarn.Value(Enum.GetName(typeof(CharacterJob),strategicTargetJob)));
                AutoAdvancePhaseOnDialogueComplete();
                runningDialogue = true;
                phaseTest.RunNightDialogue();
                MonsterAttack();
                print("TODO: show feldgeister on screen");
                break;
        }
    }

    public void AddWeapons()
    {
        resourceList.AddWeapons(2);
        if(blacksmith.moraleValue > 1)
        {
            resourceList.AddWeapons(1);
        }
    }
    public void FinishLambSelection()
        => selectionMode = new DefenseSelectionMode(this);

    public void FinishDefenseSelection()
        => AdvancePhase();

    public void FinishDialogueSelection()
    {
        if (foodRemaining > 0)
        {
            print("there's food here");
            selectionMode = new FoodSelectionMode(this);
        }   
        else
        {
            print("food: " + foodRemaining);
            FinishFoodSelection();
        }
    }

    public void FinishFoodSelection(){
        print("finishing food");
        AdvancePhase();
    }

    public void AdvancePhase()
    {
        print("ADVANCING PHASE");
        var nextPhase = (GamePhase)(((int)phase + 1) % phaseCount);
        RunPhase(nextPhase);
    }

    void Update()
    {
        // required so that the feldgeister input system is up to date each frame
        // do not change __
        Feldgeister.Input.Update(buildingMap.Get);
        // do not change ^^

        Character hovered = 
            Feldgeister.Input.lastFocused is House ? 
                (Feldgeister.Input.lastFocused as House).occupant :
                null;
        selectionMode?.OnHover(hovered);

// #if UNITY_EDITOR
        // if in the editor, can advance selection modes with ']'
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            switch (selectionMode)
            {
                case DialogueSelectionMode _:
                    FinishDialogueSelection();
                    break;
                case DefenseSelectionMode _:
                    FinishDefenseSelection();
                    break;
                default:
                    AdvancePhase();
                    break;
            }
        }
// #endif
    }

    public void MonsterAttack()
    {
        strategicTarget.defenseLevel--;
        randomTarget.defenseLevel--;
    }

    public void OnCharacterDied(Character c)
    {
        if (c.house.hasLamb)
        {
            Debug.LogWarning("You lost!");
        }
    }

    public void CharacterSelected(Character character)
        => selectionMode.OnSelected(character);

    public void BeginOpeningDialogue()
    {
// #if UNITY_EDITOR
//         if (runOpeningDialogue)
// #endif
//         {
//             GetComponent<NPC_Conor>()?.RunDialogue();
//             print("delaying game start till after opening dialogue");
//             AutoAdvancePhaseOnDialogueComplete();
//             cameraLocked = true;
//         }
// #if UNITY_EDITOR
//         else
//         {
//             cameraLocked = false;
//             AdvancePhase();
//         }
// #endif
        AdvancePhase();
    }

    public void AutoAdvancePhaseOnDialogueComplete()
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

    public void ClearHouseAndCharacterDisplays()
    {
        houseOccupantUI.UpdateDisplay(null);
        leftCharacterDisplay.UpdateDisplay(null);
        rightCharacterDisplay.UpdateDisplay(null);
    }
}