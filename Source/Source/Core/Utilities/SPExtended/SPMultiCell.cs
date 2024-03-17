using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foundation.Utilities
{
    public static class SPMultiCell
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
                return min;
            return val.CompareTo(max) > 0 ? max : val;
        }

        //public static void ClampToMap(Pawn pawn, ref IntVec3 exitPoint, Map map, int extraOffset = 0)
        //{
        //    int x = pawn.def.size.x;
        //    int z = pawn.def.size.z;
        //    int num = x > z ? x + extraOffset : z + extraOffset;
        //    if (exitPoint.x < num)
        //        exitPoint.x = num / 2;
        //    else if (exitPoint.x >= map.Size.x - num / 2)
        //        exitPoint.x = map.Size.x - num / 2;
        //    if (exitPoint.z < num)
        //    {
        //        exitPoint.z = num / 2;
        //    }
        //    else
        //    {
        //        if (exitPoint.z <= map.Size.z - num / 2)
        //            return;
        //        exitPoint.z = map.Size.z - num / 2;
        //    }
        //}

        //public static IntVec3 ClampToMap(this Pawn pawn, IntVec3 spawnPoint, Map map, int extraOffset = 0)
        //{
        //    int x = pawn.def.size.x;
        //    int z = pawn.def.size.z;
        //    int num = x > z ? x + extraOffset : z + extraOffset;
        //    if (spawnPoint.x < num)
        //        spawnPoint.x = num / 2;
        //    else if (spawnPoint.x >= map.Size.x - num / 2)
        //        spawnPoint.x = map.Size.x - num / 2;
        //    if (spawnPoint.z < num)
        //        spawnPoint.z = num / 2;
        //    else if (spawnPoint.z > map.Size.z - num / 2)
        //        spawnPoint.z = map.Size.z - num / 2;
        //    return spawnPoint;
        //}

        //public static bool ClampHitboxToMap(Pawn p, IntVec3 nextCell, Map map)
        //{
        //    int num1 = p.def.size.x % 2 == 0 ? p.def.size.x / 2 : (p.def.size.x + 1) / 2;
        //    int num2 = p.def.size.z % 2 == 0 ? p.def.size.z / 2 : (p.def.size.z + 1) / 2;
        //    int num3 = num1 > num2 ? num1 : num2;
        //    return nextCell.x + num3 >= map.Size.x || nextCell.z + num3 >= map.Size.z || nextCell.x - num3 <= 0 || nextCell.z - num3 <= 0;
        //}

        //public static List<IntVec3> PawnOccupiedCells(
        //  this Pawn pawn,
        //  IntVec3 centerPoint,
        //  Rot4 direction)
        //{
        //    int width;
        //    int height;
        //    switch (direction.AsInt)
        //    {
        //        case 0:
        //            width = pawn.def.size.x;
        //            height = pawn.def.size.z;
        //            break;
        //        case 1:
        //            width = pawn.def.size.z;
        //            height = pawn.def.size.x;
        //            break;
        //        case 2:
        //            width = pawn.def.size.x;
        //            height = pawn.def.size.z;
        //            break;
        //        case 3:
        //            width = pawn.def.size.z;
        //            height = pawn.def.size.x;
        //            break;
        //        default:
        //            throw new NotImplementedException("MoreThan4Rotations");
        //    }
        //    return CellRect.CenteredOn(centerPoint, width, height).Cells.ToList<IntVec3>();
        //}

        //public static Rot4 ClosestEdge(Pawn pawn, Map map)
        //{
        //    IntVec2 intVec2_1 = new IntVec2(map.Size.x, map.Size.z);
        //    IntVec2 intVec2_2 = new IntVec2(pawn.Position.x, pawn.Position.z);
        //    SPTuples.SPTuple2<Rot4, int> spTuple2_1 = Math.Abs(intVec2_2.x) < Math.Abs(intVec2_2.x - intVec2_1.x) ? new SPTuples.SPTuple2<Rot4, int>(Rot4.West, intVec2_2.x) : new SPTuples.SPTuple2<Rot4, int>(Rot4.East, Math.Abs(intVec2_2.x - intVec2_1.x));
        //    SPTuples.SPTuple2<Rot4, int> spTuple2_2 = Math.Abs(intVec2_2.z) < Math.Abs(intVec2_2.z - intVec2_1.z) ? new SPTuples.SPTuple2<Rot4, int>(Rot4.South, intVec2_2.z) : new SPTuples.SPTuple2<Rot4, int>(Rot4.North, Math.Abs(intVec2_2.z - intVec2_1.z));
        //    return spTuple2_1.Second <= spTuple2_2.Second ? spTuple2_1.First : spTuple2_2.First;
        //}

        //public static bool WithinDistanceToEdge(this IntVec3 position, int distance, Map map) => position.x < distance || position.z < distance || map.Size.x - position.x < distance || map.Size.z - position.z < distance;

        //public static void CalculateSelectionBracketPositionsWorldForMultiCellPawns<T>(
        //  Vector3[] bracketLocs,
        //  T obj,
        //  Vector3 worldPos,
        //  Vector2 worldSize,
        //  Dictionary<T, float> dict,
        //  Vector2 textureSize,
        //  float pawnAngle = 0.0f,
        //  float jumpDistanceFactor = 1f)
        //{
        //    float num1;
        //    float num2 = (dict.TryGetValue(obj, out num1) ? Mathf.Max(0.0f, (float)(1.0 - ((double)Time.realtimeSinceStartup - (double)num1) / 0.070000000298023224)) : 1f) * 0.2f * jumpDistanceFactor;
        //    float num3 = (float)(0.5 * ((double)worldSize.x - (double)textureSize.x)) + num2;
        //    float num4 = (float)(0.5 * ((double)worldSize.y - (double)textureSize.y)) + num2;
        //    float y = AltitudeLayer.VisEffects.AltitudeFor();
        //    bracketLocs[0] = new Vector3(worldPos.x - num3, y, worldPos.z - num4);
        //    bracketLocs[1] = new Vector3(worldPos.x + num3, y, worldPos.z - num4);
        //    bracketLocs[2] = new Vector3(worldPos.x + num3, y, worldPos.z + num4);
        //    bracketLocs[3] = new Vector3(worldPos.x - num3, y, worldPos.z + num4);
        //    float num5 = pawnAngle;
        //    if ((double)num5 != 45.0)
        //    {
        //        if ((double)num5 != -45.0)
        //            return;
        //        for (int index = 0; index < 4; ++index)
        //        {
        //            SPTuples.SPTuple2<float, float> spTuple2 = SPTrig.RotatePointCounterClockwise(bracketLocs[index].x - worldPos.x, bracketLocs[index].z - worldPos.z, 45f);
        //            bracketLocs[index].x = spTuple2.First + worldPos.x;
        //            bracketLocs[index].z = spTuple2.Second + worldPos.z;
        //        }
        //    }
        //    else
        //    {
        //        for (int index = 0; index < 4; ++index)
        //        {
        //            SPTuples.SPTuple2<float, float> spTuple2 = SPTrig.RotatePointClockwise(bracketLocs[index].x - worldPos.x, bracketLocs[index].z - worldPos.z, 45f);
        //            bracketLocs[index].x = spTuple2.First + worldPos.x;
        //            bracketLocs[index].z = spTuple2.Second + worldPos.z;
        //        }
        //    }
        //}

        //public static Vector3 DrawPosTransformed(this Pawn pawn, float x, float z, float angle = 0.0f)
        //{
        //    Vector3 drawPos = pawn.DrawPos;
        //    switch (pawn.Rotation.AsInt)
        //    {
        //        case 0:
        //            drawPos.x += x;
        //            drawPos.z += z;
        //            break;
        //        case 1:
        //            if ((double)angle == -45.0)
        //            {
        //                drawPos.x += (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                drawPos.z += (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                break;
        //            }
        //            if ((double)angle == 45.0)
        //            {
        //                drawPos.x += (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                drawPos.z -= (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                break;
        //            }
        //            drawPos.x += z;
        //            drawPos.z += x;
        //            break;
        //        case 2:
        //            drawPos.x -= x;
        //            drawPos.z -= z;
        //            break;
        //        case 3:
        //            if ((double)angle == -45.0)
        //            {
        //                drawPos.x -= (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                drawPos.z -= (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                break;
        //            }
        //            if ((double)angle == 45.0)
        //            {
        //                drawPos.x -= (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                drawPos.z += (double)x == 0.0 ? z / (float)Math.Sqrt(2.0) : x / (float)Math.Sqrt(2.0);
        //                break;
        //            }
        //            drawPos.x -= z;
        //            drawPos.z -= x;
        //            break;
        //        default:
        //            throw new NotImplementedException("Pawn Rotation outside Rot4");
        //    }
        //    return drawPos;
        //}
    }
}