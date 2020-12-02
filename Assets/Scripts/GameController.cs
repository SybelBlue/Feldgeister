using UnityEngine;

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
    public CharacterDisplayController characterDisplay;

    [Range(0, 1)]
    public float UIVolume;

    public bool runOpeningDialogue;

#pragma warning disable 0414
    [SerializeField, ReadOnly]
    private bool _mapReady = false;

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        _mapReady = true;
    }

    void Update()
    {
        if (Feldgeister.Input.mouseHasMoved && mapReady) 
        {
            var flattened = Feldgeister.Input.currentWorldMousePosition.To2DInt();
            Feldgeister.Input.lastFocused = buildingMap[flattened];
        }

        if (Input.GetMouseButtonDown(0))
        {
            Feldgeister.Input.SendClick();
        }

        if (!cameraLocked && Feldgeister.Input.lastFocused is House)
        {
            var house = Feldgeister.Input.lastFocused as House;
            houseOccupantUI.UpdateDisplay(house.occupant);
            characterDisplay.DisplayCharacter(house.occupant);
        }
        else
        {
            houseOccupantUI.UpdateDisplay(null);
            if (!cameraLocked)
            {
                characterDisplay.DisplayCharacter(null);
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