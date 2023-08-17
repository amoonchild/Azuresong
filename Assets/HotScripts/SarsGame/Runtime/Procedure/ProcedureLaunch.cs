//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:05:00
//  Description：ProcedureLaunch
//------------------------------------------------------------
using SarsFramework.Runtime;
using GameFramework.Resource;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace SarsGame.Runtime
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            InitSoundSettings();
            InitUIGroups();
            InitEntityGroups();
        }

        protected override void OnUpdateSelf(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (SarsEntry.Base.EditorResourceMode)
            {
                SarsLog.Info("Editor resource mode detected.");
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if (SarsEntry.Resource.ResourceMode == ResourceMode.Package)
            {
                SarsLog.Info("Package resource mode detected.");
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
            else
            {
                SarsLog.Info("Updatable resource mode detected.");
            }
        }

        private void InitSoundSettings()
        {
            SarsEntry.Sound.AddSoundGroup(SarsFramework.Runtime.Constant.SoundGroup.MusicGroupName, false, SarsEntry.Setting.IsMusicMuted(), 1f, 1);
            SarsEntry.Sound.AddSoundGroup(SarsFramework.Runtime.Constant.SoundGroup.SoundGroupName, true, SarsEntry.Setting.IsSoundMuted(), 1f, 50);
            SarsEntry.Sound.AddSoundGroup(SarsFramework.Runtime.Constant.SoundGroup.UISoundGroupName, true, SarsEntry.Setting.IsUISoundMuted(), 1f, 10);

            SarsEntry.Sound.SetMusicVolume(SarsEntry.Setting.GetMusicVolume());
            SarsEntry.Sound.SetSoundVolume(SarsEntry.Setting.GetSoundVolume());
            SarsEntry.Sound.SetUISoundVolume(SarsEntry.Setting.GetUISoundVolume());

            SarsLog.Info("Init sound settings complete.");
        }

        private void InitEntityGroups()
        {
            SarsEntry.Entity.AddEntityGroup(Constant.EntityGroup.Player, 30f, 30, 30f, 100);
            //SarsEntry.Entity.AddEntityGroup(Constant.EntityGroup.ChooseRole, 30f, 100, 30f, 100);
            //SarsEntry.Entity.AddEntityGroup(Constant.EntityGroup.Npc,30f,100,30f,100);

            SarsLog.Info("Init entity groups complete.");
        }

        private void InitUIGroups()
        {
            SarsEntry.UI.AddUIGroup(Constant.UIGroup.Normal, 0);
            SarsEntry.UI.AddUIGroup(Constant.UIGroup.Popup, 10000);
            SarsEntry.UI.AddUIGroup(Constant.UIGroup.Top, 20000);

            SarsLog.Info("Init ui groups complete.");
        }
    }
}