using System.Collections.Generic;
using _Scripts.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace _Scripts.Audio
{

    public class AudioManager : Singleton<AudioManager>
    {

        [Header("Volume")]
        [Range(0f, 1f)]
        public float MasterVolume = 1f;
    
        [Range(0f, 1f)]
        public float MusicVolume = 1f;
    
        [Range(0f, 1f)]
        public float SfxVolume = 1f;
    
        [Range(0f, 1f)]
        public float VoiceVolume = 1f;
    
        [Range(0f, 1f)] 
    
        Bus _masterBus;

        Bus _musicBus;

        Bus _sfxBus;

        Bus _voiceBus;


        List<EventInstance> _eventInstances;
        List<StudioEventEmitter> _eventEmitters;

        EventInstance _ambienceEventInstance;


        void Awake()
        {
            if (Instance is not null)
            {
                Debug.LogError("More than one AudioManager in the scene");
            }

            _eventInstances = new List<EventInstance>();
            _eventEmitters = new List<StudioEventEmitter>();

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
            _voiceBus = RuntimeManager.GetBus("bus:/Voice");
        }

        void Start()
        {
            InitializeAmbience(FMODEvents.Instance.AmbienceHubworld);
        }

        void Update()
        {
            _masterBus.setVolume(MasterVolume);
            _musicBus.setVolume(MusicVolume);
            _sfxBus.setVolume(SfxVolume);
            _voiceBus.setVolume(VoiceVolume);
        }


        void InitializeAmbience(EventReference ambienceEventReference)
        {
            _ambienceEventInstance = CreateEventInstance(ambienceEventReference);
            _ambienceEventInstance.start();
        }
    
        public void SetAmbienceArea(AmbienceArea area)
        {
            _ambienceEventInstance.setParameterByName("area", (float)area);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
            return eventInstance;
        }

        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference,GameObject emitterGameObject)
        {
            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            _eventEmitters.Add(emitter);
            return emitter;
        }

        void CleanUp()
        {
            // stop and release any created instances
            foreach (EventInstance eventInstance in _eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
            // stop all of the event emitters, because if we dont they may hang around other scenes
            foreach (StudioEventEmitter emitter in _eventEmitters)
            {
                emitter.Stop();
            }
        }

        void OnDestroy()
        {
            CleanUp();
        }
    }

}
