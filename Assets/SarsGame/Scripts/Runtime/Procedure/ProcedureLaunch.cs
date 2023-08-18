//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:05:00
//  Description：ProcedureLaunch
//------------------------------------------------------------
using Azuresong.Runtime;
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
            if (AzuresongEntry.Base.EditorResourceMode)
            {
                ASLog.Info("Editor resource mode detected.");
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if (AzuresongEntry.Resource.ResourceMode == ResourceMode.Package)
            {
                ASLog.Info("Package resource mode detected.");
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
            else
            {
                ASLog.Info("Updatable resource mode detected.");
            }
        }

        private void InitSoundSettings()
        {
            AzuresongEntry.Sound.AddSoundGroup(Azuresong.Runtime.Constant.SoundGroup.MusicGroupName, false, AzuresongEntry.Setting.IsMusicMuted(), 1f, 1);
            AzuresongEntry.Sound.AddSoundGroup(Azuresong.Runtime.Constant.SoundGroup.SoundGroupName, true, AzuresongEntry.Setting.IsSoundMuted(), 1f, 50);
            AzuresongEntry.Sound.AddSoundGroup(Azuresong.Runtime.Constant.SoundGroup.UISoundGroupName, true, AzuresongEntry.Setting.IsUISoundMuted(), 1f, 10);

            AzuresongEntry.Sound.SetMusicVolume(AzuresongEntry.Setting.GetMusicVolume());
            AzuresongEntry.Sound.SetSoundVolume(AzuresongEntry.Setting.GetSoundVolume());
            AzuresongEntry.Sound.SetUISoundVolume(AzuresongEntry.Setting.GetUISoundVolume());

            ASLog.Info("Init sound settings complete.");
        }

        private void InitEntityGroups()
        {
            AzuresongEntry.Entity.AddEntityGroup(Constant.EntityGroup.Player, 30f, 30, 30f, 100);
            //SarsEntry.Entity.AddEntityGroup(Constant.EntityGroup.ChooseRole, 30f, 100, 30f, 100);
            //SarsEntry.Entity.AddEntityGroup(Constant.EntityGroup.Npc,30f,100,30f,100);

            ASLog.Info("Init entity groups complete.");
        }

        private void InitUIGroups()
        {
            AzuresongEntry.UI.AddUIGroup(Constant.UIGroup.Normal, 0);
            AzuresongEntry.UI.AddUIGroup(Constant.UIGroup.Popup, 10000);
            AzuresongEntry.UI.AddUIGroup(Constant.UIGroup.Top, 20000);

            ASLog.Info("Init ui groups complete.");
        }
    }
}