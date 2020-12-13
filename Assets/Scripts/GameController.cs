﻿using UnityEngine;
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

    public CameraController cameraController
        => Camera.main.GetComponent<CameraController>();

    public bool cameraLocked
    {
        get => cameraController.lockPosition;
        set => cameraController.lockPosition = value;
    }

    public Yarn.Unity.DialogueRunner dialogueRunner;
    
    public HouseOccupant houseOccupantUI;

    public CharacterDisplayController leftCharacterDisplay, rightCharacterDisplay;
    public Character mayorCharacter;

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

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        houses = new List<House>(mapGenerator.GetComponents<House>());
    }

    public void RunPhase(GamePhase phase)
    {
        this.phase = phase;

        switch (phase)
        {
            case GamePhase.Dawn:
                strategy = new List<AttackStrategy>(StaticUtils.allStrategies).RandomChoice();
                strategicTarget = StaticUtils.HouseForStrategy(strategy, houses);
                strategicTargetJob = strategicTarget.character.job;

                randomTarget = houses.RandomChoice();
                randomTargetJob = randomTarget.character.job;
                print("TODO: watcher says attack strategy"); // conor
                // use this to transition to day after watcher dialogue finishes
                // AutoAdvancePhaseOnDialogueComplete();
                AdvancePhase(); // remove this line when above todo implemeted
                break;
            case GamePhase.Day:
                print("TODO: update character food and morale stats"); // katia
                print("TODO: display defense and resource dropdowns"); // katia
                selectionMode = new DialogueSelectionMode(this);
                print("TODO: show button to change selection mode to allow food donation"); // katia
                break;
            case GamePhase.Dusk:
                print("TODO: get defenses from blacksmith"); // conor
                print("TODO: change selection mode to place lamb");
                print("TODO: await defense finish to change selection mode to place defenses");
                break;
            case GamePhase.Night:
                MonsterAttack();
                print("TODO: show feldgeister on screen");
                print("TODO: display attack dialogue"); // conor
                break;
        }
    }

    public void AdvancePhase()
    {
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

    public void CharacterSelected(Character character)
        => selectionMode.OnSelected(character);

    public void BeginOpeningDialogue()
    {
#if UNITY_EDITOR
        if (runOpeningDialogue)
#endif
        {
            GetComponent<NPC_Conor>()?.RunDialogue();
            print("delaying game start till after opening dialogue");
            AutoAdvancePhaseOnDialogueComplete();
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
}