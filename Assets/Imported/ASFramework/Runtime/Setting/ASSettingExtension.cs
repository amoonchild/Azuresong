//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 15:23:13
//  Description：ASSettingExtension
//------------------------------------------------------------
using GameFramework;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public static class ASSettingExtension
    {
        public static bool IsMusicMuted(this SettingComponent settingComponent)
        {
            return settingComponent.GetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, Constant.SoundGroup.MusicGroupName), false);
        }

        public static bool IsSoundMuted(this SettingComponent settingComponent)
        {
            return settingComponent.GetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, Constant.SoundGroup.SoundGroupName), false);
        }

        public static bool IsUISoundMuted(this SettingComponent settingComponent)
        {
            return settingComponent.GetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, Constant.SoundGroup.UISoundGroupName), false);
        }

        public static float GetMusicVolume(this SettingComponent settingComponent)
        {
            return AzuresongEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.MusicGroupName), 1f);
        }

        public static float GetSoundVolume(this SettingComponent settingComponent)
        {
            return AzuresongEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.SoundGroupName), 1f);
        }

        public static float GetUISoundVolume(this SettingComponent settingComponent)
        {
            return AzuresongEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.UISoundGroupName), 1f);
        }

        public static void SetMusicVolume(this SettingComponent settingComponent, float volume)
        {
            AzuresongEntry.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.MusicGroupName), volume);
        }

        public static void SetSoundVolume(this SettingComponent settingComponent, float volume)
        {
            AzuresongEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.SoundGroupName), volume);
        }

        public static void SetUISoundVolume(this SettingComponent settingComponent, float volume)
        {
            AzuresongEntry.Setting.GetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, Constant.SoundGroup.UISoundGroupName), volume);
        }
    }
}