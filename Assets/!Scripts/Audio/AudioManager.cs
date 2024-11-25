using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    [Header("Volume")]
    [Range(0f, 1f)]

    public float masterVolume = 1f;
    [Range(0f, 1f)]

    public float musicVolume = 1f;
    [Range(0f, 1f)]

    public float sfxVolume = 1f;
    [Range(0f, 1f)]

    public float voiceVolume = 1f;
    [Range(0f, 1f)]

    private Bus masterBus;

    private Bus musicBus;

    private Bus sfxBus;

    private Bus voiceBus;


    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;

    public static AudioManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene");
        }
        Instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        voiceBus = RuntimeManager.GetBus("bus:/Voice");
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.Instance.AmbienceHubworld);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        voiceBus.setVolume(voiceVolume);
    }


    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    
    public void SetAmbienceArea(AmbienceArea area)
    {
        ambienceEventInstance.setParameterByName("area", (float)area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference,GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we dont they may hang around other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
