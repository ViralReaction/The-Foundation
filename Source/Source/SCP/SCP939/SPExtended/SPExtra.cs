using System;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SCP
{
    [StaticConstructorOnStartup]
    public static class SPExtra
    {
        private static readonly Texture2D FillableBarTexture = SolidColorMaterials.NewSolidColorTexture(0.5f, 0.5f, 0.5f, 0.5f);
        private static readonly Texture2D ClearBarTexture = BaseContent.ClearTex;

        public static void GetList<T>(List<int> offsets, List<T> values, int index, List<T> outList)
        {
            outList.Clear();
            int offset = offsets[index];
            int num = values.Count;
            if (index + 1 < offsets.Count)
                num = offsets[index + 1];
            for (int index1 = offset; index1 < num; ++index1)
                outList.Add(values[index1]);
        }

        public static T PopRandom<T>(ref List<T> list)
        {
            if (list == null || !GenCollection.Any<T>(list))
                return default(T);
            Rand.PushState();
            int index = Rand.Range(0, list.Count);
            T obj = list[index];
            Rand.PopState();
            list.Remove(obj);
            return obj;
        }

        public static KeyValuePair<T1, T2> RandomKVPFromDictionary<T1, T2>(
          this IDictionary<T1, T2> dictionary)
        {
            return dictionary.ElementAt<KeyValuePair<T1, T2>>(Rand.Range(0, dictionary.Count));
        }

        public static void SPShuffle<T>(this IList<T> list)
        {
            System.Random random = new System.Random((int)Time.deltaTime);
            int count = list.Count;
            while (count > 1)
            {
                --count;
                int index = random.Next(count + 1);
                T obj = list[index];
                list[index] = list[count];
                list[count] = obj;
            }
        }

        public static List<T> ConvertToList<T>(this T typeObject) => new List<T>()
    {
      typeObject
    };

        public static bool ContainsAllOfList<T>(
          this IEnumerable<T> sourceList,
          IEnumerable<T> searchingList)
        {
            return sourceList != null && searchingList != null && sourceList.Intersect<T>(searchingList).Any<T>();
        }

        public static List<T> ConvertObjectList<T>(this List<object> objects)
        {
            for (int index = 0; index < objects.Count; ++index)
            {
                object obj = objects[index];
                if (obj.GetType() != typeof(T))
                    objects.Remove(obj);
            }
            return objects.Cast<T>().ToList<T>();
        }

        public static IntVec2 Abs(this IntVec2 c) => new IntVec2(Math.Abs(c.x), Math.Abs(c.z));

        public static IntVec3 Abs(this IntVec3 c) => new IntVec3(Math.Abs(c.x), Math.Abs(c.y), Math.Abs(c.z));

        public static double Distance(IntVec3 c1, IntVec3 c2)
        {
            int val1 = Math.Abs(c1.x - c2.x);
            int val2 = Math.Abs(c1.z - c2.z);
            return Math.Sqrt((double)(val1.Pow(2) + val2.Pow(2)));
        }

        public static IEnumerable<IntVec3> AdjacentCellsCardinal(this IntVec3 c, Map map)
        {
            IntVec3 north = new IntVec3(c.x, c.y, c.z + 1);
            IntVec3 east = new IntVec3(c.x + 1, c.y, c.z);
            IntVec3 south = new IntVec3(c.x, c.y, c.z - 1);
            IntVec3 west = new IntVec3(c.x - 1, c.y, c.z);
            if (north.InBounds(map))
                yield return north;
            if (east.InBounds(map))
                yield return east;
            if (south.InBounds(map))
                yield return south;
            if (west.InBounds(map))
                yield return west;
        }

        public static IEnumerable<IntVec3> AdjacentCellsDiagonal(this IntVec3 c, Map map)
        {
            IntVec3 NE = new IntVec3(c.x + 1, c.y, c.z + 1);
            IntVec3 SE = new IntVec3(c.x + 1, c.y, c.z - 1);
            IntVec3 SW = new IntVec3(c.x - 1, c.y, c.z - 1);
            IntVec3 NW = new IntVec3(c.x - 1, c.y, c.z + 1);
            if (NE.InBounds(map))
                yield return NE;
            if (SE.InBounds(map))
                yield return SE;
            if (SW.InBounds(map))
                yield return SW;
            if (NW.InBounds(map))
                yield return NW;
        }

        public static IEnumerable<IntVec3> AdjacentCells8Way(this IntVec3 c, Map map) => c.AdjacentCellsCardinal(map).Concat<IntVec3>(c.AdjacentCellsDiagonal(map));

        public static int Pow(this int val, int exp) => Enumerable.Repeat<int>(val, exp).Aggregate<int, int>(1, (Func<int, int, int>)((x, y) => x * y));

        public static Rot4 Max4IntToRot(
          int northCellCount,
          int eastCellCount,
          int southCellCount,
          int westCellCount)
        {
            int num1 = northCellCount > eastCellCount ? northCellCount : eastCellCount;
            int num2 = southCellCount > westCellCount ? southCellCount : westCellCount;
            int num3 = num1 > num2 ? num1 : num2;
            if (num3 == northCellCount)
                return Rot4.North;
            if (num3 == eastCellCount)
                return Rot4.East;
            if (num3 == southCellCount)
                return Rot4.South;
            return num3 == westCellCount ? Rot4.West : Rot4.Invalid;
        }

        public static Rot4 RiverDirection(Map map)
        {
            List<Tile.RiverLink> rivers = Find.WorldGrid[map.Tile].Rivers;
            float headingFromTo = Find.WorldGrid.GetHeadingFromTo(map.Tile, rivers.OrderBy<Tile.RiverLink, int>((Func<Tile.RiverLink, int>)(r1 => -r1.river.degradeThreshold)).First<Tile.RiverLink>().neighbor);
            if ((double)headingFromTo < 45.0)
                return Rot4.South;
            if ((double)headingFromTo < 135.0)
                return Rot4.East;
            if ((double)headingFromTo < 225.0)
                return Rot4.North;
            return (double)headingFromTo < 315.0 ? Rot4.West : Rot4.South;
        }

        public static Rect VerticalFillableBar(Rect rect, float fillPercent, bool flip = false) => SPExtra.VerticalFillableBar(rect, fillPercent, SPExtra.FillableBarTexture, flip);

        public static Rect VerticalFillableBar(
          Rect rect,
          float fillPercent,
          Texture2D fillTex,
          bool flip = false)
        {
            bool doBorder = (double)rect.height > 15.0 && (double)rect.width > 20.0;
            return SPExtra.VerticalFillableBar(rect, fillPercent, fillTex, SPExtra.ClearBarTexture, doBorder, flip);
        }

        public static Rect VerticalFillableBar(
          Rect rect,
          float fillPercent,
          Texture2D fillTex,
          Texture2D bgTex,
          bool doBorder = false,
          bool flip = false)
        {
            if (doBorder)
            {
                GUI.DrawTexture(rect, (Texture)bgTex);
                rect = rect.ContractedBy(3f);
            }
            if ((UnityEngine.Object)bgTex != (UnityEngine.Object)null)
                GUI.DrawTexture(rect, (Texture)bgTex);
            if (!flip)
            {
                rect.y += rect.height;
                rect.height *= -1f;
            }
            Rect rect1 = rect;
            rect.height *= fillPercent;
            GUI.DrawTexture(rect, (Texture)fillTex);
            return rect1;
        }

        public static Texture2D ConvertToTexture2D(this RenderTexture rTex)
        {
            Texture2D texture2D = new Texture2D(512, 512, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float)rTex.width, (float)rTex.height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

        public struct Quadrant
        {
            private byte quadInt;

            public Quadrant(byte q) => this.quadInt = q;

            public Quadrant(int q) => this.quadInt = (byte)q.Clamp<int>(1, 4);

            public int AsInt
            {
                get => (int)this.quadInt;
                set
                {
                    value.Clamp<int>(1, 4);
                    this.quadInt = (byte)value;
                }
            }

            public static SPExtra.Quadrant Q1 => new SPExtra.Quadrant(1);

            public static SPExtra.Quadrant Q2 => new SPExtra.Quadrant(2);

            public static SPExtra.Quadrant Q3 => new SPExtra.Quadrant(3);

            public static SPExtra.Quadrant Q4 => new SPExtra.Quadrant(4);

            public static SPExtra.Quadrant Invalid => new SPExtra.Quadrant()
            {
                quadInt = 100
            };

            public static SPExtra.Quadrant QuadrantOfIntVec3(IntVec3 c, Map map)
            {
                if (c.x > map.Size.x / 2 && c.z >= map.Size.z / 2)
                    return SPExtra.Quadrant.Q1;
                if (c.x >= map.Size.x / 2 && c.z < map.Size.z / 2)
                    return SPExtra.Quadrant.Q2;
                if (c.x < map.Size.x / 2 && c.z <= map.Size.z / 2)
                    return SPExtra.Quadrant.Q3;
                if (c.x <= map.Size.x / 2 && c.z > map.Size.z / 2)
                    return SPExtra.Quadrant.Q4;
                return c.x == map.Size.x / 2 && c.z == map.Size.z / 2 ? SPExtra.Quadrant.Q1 : SPExtra.Quadrant.Invalid;
            }

            public static SPExtra.Quadrant QuadrantRelativeToPoint(IntVec3 c, IntVec3 point, Map map)
            {
                if (c.x > point.x && c.z >= point.z)
                    return SPExtra.Quadrant.Q1;
                if (c.x >= point.x && c.z < point.z)
                    return SPExtra.Quadrant.Q2;
                if (c.x < point.x && c.z <= point.z)
                    return SPExtra.Quadrant.Q3;
                if (c.x <= point.x && c.z > point.z)
                    return SPExtra.Quadrant.Q4;
                return c.x == point.x && c.z == point.z ? SPExtra.Quadrant.Q1 : SPExtra.Quadrant.Invalid;
            }

            public static IEnumerable<IntVec3> CellsInQuadrant(SPExtra.Quadrant q, Map map)
            {
                switch (q.AsInt)
                {
                    case 1:
                        return CellRect.WholeMap(map).Cells.Where<IntVec3>((Func<IntVec3, bool>)(c2 => c2.x > map.Size.x / 2 && c2.z >= map.Size.z / 2));
                    case 2:
                        return CellRect.WholeMap(map).Cells.Where<IntVec3>((Func<IntVec3, bool>)(c2 => c2.x <= map.Size.x / 2 && c2.z < map.Size.z / 2));
                    case 3:
                        return CellRect.WholeMap(map).Cells.Where<IntVec3>((Func<IntVec3, bool>)(c2 => c2.x < map.Size.x / 2 && c2.z <= map.Size.z / 2));
                    case 4:
                        return CellRect.WholeMap(map).Cells.Where<IntVec3>((Func<IntVec3, bool>)(c2 => c2.x <= map.Size.x / 2 && c2.z > map.Size.z / 2));
                    default:
                        throw new NotImplementedException("Quadrants");
                }
            }
        }
    }
}