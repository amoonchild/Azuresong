//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 14:19:02
//  Description：ASSoundExtension
//------------------------------------------------------------
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public static class ASSoundExtension
    {
        private const float _fadeVolumeDuration = 1f;
        private static int? _musicSerialId = null;


        public static int? PlayMusic(this SoundComponent soundComponent, string musicAssetName, object userData = null)
        {
            soundComponent.StopMusic();

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.FadeInSeconds = _fadeVolumeDuration;
            playSoundParams.SpatialBlend = 0f;

            _musicSerialId = soundComponent.PlaySound(musicAssetName,
                Constant.SoundGroup.MusicGroupName, Constant.AssetPriority.MusicAsset, playSoundParams, null, userData);

            return _musicSerialId;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!_musicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(_musicSerialId.Value, _fadeVolumeDuration);
            _musicSerialId = null;
        }

        public static int? PlaySound(this SoundComponent soundComponent, string soundAssetName, 
            bool loop, AzuresongEntityLogic bindingEntity = null, object userData = null)
        {
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = loop;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.SpatialBlend = 0f;

            return soundComponent.PlaySound(soundAssetName,
                Constant.SoundGroup.SoundGroupName, Constant.AssetPriority.SoundAsset, 
                playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }

        public static int? PlayUISound(this SoundComponent soundComponent, string soundAssetName, object userData = null)
        {
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.SpatialBlend = 0f;

            return soundComponent.PlaySound(soundAssetName,
                Constant.SoundGroup.UISoundGroupName, Constant.AssetPriority.UISoundAsset, playSoundParams, userData);
        }

        public static bool IsMusicMuted(this SoundComponent soundComponent)
        {
            return soundComponent.IsMuted(Constant.SoundGroup.MusicGroupName);
        }

        public static bool IsSoundMuted(this SoundComponent soundComponent)
        {
            return soundComponent.IsMuted(Constant.SoundGroup.SoundGroupName);
        }

        public static bool IsUISoundMuted(this SoundComponent soundComponent)
        {
            return soundComponent.IsMuted(Constant.SoundGroup.UISoundGroupName);
        }

        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ASLog.Warning("Sound group is invalid.");
                return true;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                ASLog.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }

            return soundGroup.Mute;
        }

        public static void MuteMusic(this SoundComponent soundComponent, bool mute)
        {
            soundComponent.Mute(Constant.SoundGroup.MusicGroupName, mute);
        }

        public static void MuteSound(this SoundComponent soundComponent, bool mute)
        {
            soundComponent.Mute(Constant.SoundGroup.SoundGroupName, mute);
        }

        public static void MuteUISound(this SoundComponent soundComponent, bool mute)
        {
            soundComponent.Mute(Constant.SoundGroup.UISoundGroupName, mute);
        }

        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ASLog.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                ASLog.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;

            AzuresongEntry.Setting.SetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            AzuresongEntry.Setting.Save();
        }

        public static float GetMusicVolume(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume(Constant.SoundGroup.MusicGroupName);
        }

        public static float GetSoundVolume(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume(Constant.SoundGroup.SoundGroupName);
        }

        public static float GetUISoundVolume(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume(Constant.SoundGroup.UISoundGroupName);
        }

        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ASLog.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                ASLog.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetMusicVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume(Constant.SoundGroup.MusicGroupName, volume);
        }

        public static void SetSoundVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume(Constant.SoundGroup.SoundGroupName, volume);
        }

        public static void SetUISoundVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume(Constant.SoundGroup.UISoundGroupName, volume);
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ASLog.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                ASLog.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            AzuresongEntry.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            AzuresongEntry.Setting.Save();
        }
    }
}
