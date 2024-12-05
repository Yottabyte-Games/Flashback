namespace _Scripts.Audio
{
    using System.Collections.Generic;
    using Generic;
    using FMOD.Studio;
    using FMODUnity;
    using UnityEngine;
    
    public class AudioManager : Singleton<AudioManager>
    {
        
        [SerializeField] FMODEventsSO fmodEventsSo;
        
        public AudioSettingsSO AudioSettingsSo;
        public EventReference AmbienceHubworld => fmodEventsSo.AmbienceHubworld;
        public EventReference PlayerFootsteps => fmodEventsSo.PlayerFootsteps;
        public EventReference Checkpoint => fmodEventsSo.Checkpoint;
        public EventReference Finishline => fmodEventsSo.Finishline;
        public EventReference Dialogue => fmodEventsSo.Dialogue;
        
        
        Bus _masterBus;
        Bus _musicBus;
        Bus _sfxBus;
        Bus _voiceBus;


        List<EventInstance> _eventInstances;
        List<StudioEventEmitter> _eventEmitters;

        EventInstance _ambienceEventInstance;


        public override void Awake()
        {
            base.Awake();
            _eventInstances = new List<EventInstance>();
            _eventEmitters = new List<StudioEventEmitter>();

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
            _voiceBus = RuntimeManager.GetBus("bus:/Voice");
        }

        void Start()
        {
            InitializeAmbience(AmbienceHubworld);
        }
        
        void Update()
        {
            _masterBus.setVolume(AudioSettingsSo.MasterVolume);
            _musicBus.setVolume(AudioSettingsSo.MusicVolume);
            _sfxBus.setVolume(AudioSettingsSo.SfxVolume);
            _voiceBus.setVolume(AudioSettingsSo.VoiceVolume);
            
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

        public void CleanUp()
        {
            _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            
            _masterBus.clearHandle();
            
            // stop and release any created instances
            foreach (EventInstance eventInstance in _eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
                eventInstance.clearHandle();
            }
            // stop all the event emitters, because if we don't they may hang around other scenes
            foreach (StudioEventEmitter emitter in _eventEmitters)
            {
                emitter.Stop();
            }
        }

        void OnDestroy()
        {
            Debug.LogError("AudioManager OnDestroy");
            CleanUp();
        }
    }
}
