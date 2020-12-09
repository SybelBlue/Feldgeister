using UnityEngine;
using System.Collections.Generic;

#pragma warning disable 0649
public class GameController : MonoBehaviour
{
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
    private CharacterJob strategicTargetCharacter, randomTargetCharacter;

#pragma warning disable 0414
    [SerializeField, ReadOnly]
    private bool _mapReady = false;

    public void SetUpForDay()
    {
        strategy = new List<AttackStrategy>(StaticUtils.allStrategies).RandomChoice();
        strategicTarget = StaticUtils.HouseForStrategy(strategy, houses);
        strategicTargetCharacter = strategicTarget.character.characterClass;

        randomTarget = houses.RandomChoice();
        randomTargetCharacter = randomTarget.character.characterClass;
    }

    public void MonsterAttack()
    {

    }

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        houses = new List<House>(mapGenerator.GetComponents<House>());
        _mapReady = true;
        SetUpForDay();
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
            houseOccupantUI.UpdateDisplay(house.occupant);
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

    public void BeginCharacterDialogue(Character character)
    {
        print($"Requested character dialogue: {character.characterClass} {cameraLocked}");
        if (cameraLocked) return;

        var npcConor = character?.GetComponent<NPC_Conor>();
        if (!npcConor.enabled) return;

        npcConor.RunDialogue();
        cameraLocked = true;
    }

    public void BeginOpeningDialogue()
    {
#if UNITY_EDITOR
        if (runOpeningDialogue)
#endif
        {
            GetComponent<NPC_Conor>()?.RunDialogue();
            cameraLocked = true;
        }
#if UNITY_EDITOR
        else
        {
            cameraLocked = false;
        }
#endif
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