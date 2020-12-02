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

    public bool mapReady { get => mapGenerator != null; }
    public HouseOccupant houseOccupantUI;

    [Range(0, 1)]
    public float UIVolume;

    public bool runOpeningDialogue;

#if UNITY_EDITOR
    // used only to display progress in inspector!
#pragma warning disable 0414
    [SerializeField, ReadOnly]
    private bool _mapReady = false;
#endif

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

        if (!cameraController.lockPosition && Feldgeister.Input.lastFocused is House)
        {
            var house = Feldgeister.Input.lastFocused as House;
            houseOccupantUI.UpdateDisplay(house.occupant);
        }
        else
        {
            houseOccupantUI.UpdateDisplay(null);
        }
    }

    public void BeginCharacterDialogue(Character character)
    {
        print($"Requested character dialogue: {character.characterClass} {cameraController.lockPosition}");
        if (cameraController.lockPosition) return;

        var npcConor = character?.GetComponent<NPC_Conor>();
        if (!npcConor.enabled) return;
        
        npcConor.RunDialogue();
        cameraController.lockPosition = true;
    }

    public void BeginOpeningDialogue()
    {
#if UNITY_EDITOR
        if (runOpeningDialogue)
#endif
        {
            GetComponent<NPC_Conor>()?.RunDialogue();
            cameraController.lockPosition = true;
        }
#if UNITY_EDITOR
        else
        {
            cameraController.lockPosition = false;
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