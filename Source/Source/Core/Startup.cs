using RimWorld;
using Foundation;
using Foundation.Containment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Diagnostics;
using Verse;

namespace Foundation
{
    [StaticConstructorOnStartup]
    public class SCP_Startup

    {
        public static RoomRoleDef containmentRoom;
        public static string version = "v0.0.1";

        static SCP_Startup()
        {
            List<RoomRoleDef> allDefList = DefDatabase<RoomRoleDef>.AllDefsListForReading;
            for (int index = 0; index < allDefList.Count; index++)
            {
                RoomRoleDef allDef = allDefList[index];
                if (allDef.defName == "SCP_ContainmentRoom")
                {
                    SCP_Startup.containmentRoom = allDef;
                    break;
                }
            }
            //foreach (RoomRoleDef allDef in DefDatabase<RoomRoleDef>.AllDefs)
            //{
            //    if (allDef.defName == "SCP_ContainmentRoom")
            //    {
            //        SCP_Startup.containmentRoom = allDef;
            //        break;
            //    }
            //}
            Log.Message("Level 5 security credentials accepted. Welcome back to the Foundation, [REDACTED]\nSecure Contain Protect [" + SCP_Startup.version + "] loaded.");
        }
    }
}