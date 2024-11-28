using UnityEngine;

namespace _Scripts.Audio
{
    [CreateAssetMenu(fileName = "AudioSettingsSO", menuName = "Audio ScriptableObjects/AudioSettingsSO", order = 1)]
    public class AudioSettingsSO : ScriptableObject
    {
        [Range(0f, 1f)]
        public float MasterVolume = 1f;

        [Range(0f, 1f)]
        public float MusicVolume = 1f;

        [Range(0f, 1f)]
        public float SfxVolume = 1f;

        [Range(0f, 1f)]
        public float VoiceVolume = 1f;

        public void SetMasterVolume(float volume)
        {
            MasterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = volume;
        }

        public void SetVoiceVolume(float volume)
        {
            VoiceVolume = volume;
        }
    }
}
