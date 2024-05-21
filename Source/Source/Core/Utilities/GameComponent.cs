using Foundation.SRA;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foundation
{
    [StaticConstructorOnStartup]
    public class StaticCollectionsClass
    {
        public static List<HeadTypeDef> maleHeadTypeList = new List<HeadTypeDef>();
        public static List<HeadTypeDef> femaleHeadTypeList = new List<HeadTypeDef>();
        public static List<BeardDef> beardDefList = new List<BeardDef>();

        static StaticCollectionsClass()
        {

            foreach (HeadTypeDef headType in DefDatabase<HeadTypeDef>.AllDefsListForReading)
            {
                string genderType = headType.gender.ToString();
                if (genderType == "Male")
                {
                    maleHeadTypeList.Add(headType);
                }
                if (genderType == "Female")
                {
                    femaleHeadTypeList.Add(headType);
                }
            }
            foreach (BeardDef beardDef in DefDatabase<BeardDef>.AllDefsListForReading)
            {
                beardDefList.Add(beardDef);
            }
        }
    }
}
