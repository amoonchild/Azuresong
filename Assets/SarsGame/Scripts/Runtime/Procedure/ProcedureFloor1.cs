//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 16:20:51
//  Description：ProcedureFirstFloor
//------------------------------------------------------------


namespace SarsGame.Runtime
{
    public class ProcedureFloor1 : ProcedureBase
    {
        public override string SceneName
        {
            get
            {
                return "Floor_1";
            }
        }

        public override bool NeedPlayer
        {
            get
            {
                return true;
            }
        }
    }
}