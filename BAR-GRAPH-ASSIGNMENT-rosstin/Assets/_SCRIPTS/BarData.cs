using System;
using UnityEngine;

namespace BarGraphAssignment
{
    [System.Serializable]
    public class BarData : IEquatable<BarData>, IComparable<BarData>
    {
        [HideInInspector] public int PositionalIndex; // where it currently is // used for positioning, shouldnt be configured
        public int OriginalIndex; // sets initial position, can be configured in inspector
        public int Value; // height, can be configured in inspector

        public BarData(BarData sourceBarData)
        {
            this.PositionalIndex = sourceBarData.PositionalIndex;
            this.OriginalIndex = sourceBarData.OriginalIndex;
            this.Value = sourceBarData.Value;
        }

        public bool Equals(BarData other)
        {
            if (other == null) return false;
            return (this.PositionalIndex.Equals(other.PositionalIndex));
        }

        public int CompareTo(BarData other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return this.PositionalIndex.CompareTo(other.PositionalIndex);
            }
        }

    }
}