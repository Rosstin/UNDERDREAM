using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BarGraphAssignment
{

    public static class GraphCalculationUtility
    {
        /// <summary>
        /// Get positional index from the list index, inverse of GetListIndexFromPositionalIndex
        /// </summary>
        /// <param name="positionalIndex"></param>
        /// <returns></returns>
        public static int GetPositionalIndexFromListIndex(int listIndex)
        {
            return (listIndex + 1) * 10;
        }

        /// <summary>
        /// inverse of GetPositionalIndexFromListIndex
        /// </summary>
        /// <param name="listIndex"></param>
        /// <returns></returns>
        public static int GetListIndexFromPositionalIndex(int posIndex)
        {
            return (posIndex / 10) - 1;
        }

        /// <summary>
        /// Given the index value, calculate relative X pos.
        /// Must be the logical inverse of GetIndexValueFromXLocalPos or strange behavior will result [todo: unit test this]
        /// NOTE: GetIndexValueFromXLocalPos converts to an int, so it is not a strict inverse.
        /// </summary>
        /// <param name="pos"></param>
        public static float GetXLocalPosFromIndexValue(float indexVal, float minX, float maxX, float MaxIndexValue)
        {
            float relPos = indexVal / MaxIndexValue;

            return Mathf.Lerp(a: minX, b: maxX, t: relPos);
        }

        /// <summary>
        /// Given the relative X pos, calculate index val
        /// Must be logical inverse of GetXLocalPosFromIndexValue or strange behavior will result [todo: unit test this]
        /// NOTE: this method converts to an int, so it is not a strict inverse.
        /// </summary>
        /// <param name="localPositionX"></param>
        /// <returns></returns>
        public static int GetIndexValueFromXLocalPos(float localPositionX, float minX, float maxX, float MaxIndexValue)
        {
            float relPos = Mathf.InverseLerp(minX, maxX, localPositionX);

            // calculate it and floor it to int value
            int indexVal = (int)(relPos * MaxIndexValue);

            return indexVal;
        }

    }
}