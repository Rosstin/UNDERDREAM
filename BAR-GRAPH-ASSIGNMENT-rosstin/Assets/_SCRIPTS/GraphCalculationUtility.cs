using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
        public static int GetListIndexFromPositionalIndex(float posIndex)
        {
            return (int) (posIndex / 10) - 1;
        }

        /// <summary>
        /// Given the index value, calculate relative X pos.
        /// Must be the logical inverse of GetIndexValueFromXLocalPos or strange behavior will result [todo: unit test this]
        /// NOTE: GetIndexValueFromXLocalPos converts to an int, so it is not a strict inverse.
        /// </summary>
        /// <param name="pos"></param>
        public static float GetXLocalPosFromPositionalIndexValue(float indexVal, float minX, float maxX, float MaxIndexValue)
        {
            float relPos = indexVal / MaxIndexValue;

            return Mathf.Lerp(a: minX, b: maxX, t: relPos);
        }

        /// <summary>
        /// Given the relative X pos, calculate index val
        /// Must be logical inverse of GetXLocalPosFromIndexValue or strange behavior will result [todo: unit test this]
        /// </summary>
        /// <param name="localPositionX"></param>
        /// <returns></returns>
        public static float GetPositionalIndexValueFromXLocalPos(float localPositionX, float minX, float maxX, float MaxIndexValue)
        {
            float relPos = Mathf.InverseLerp(minX, maxX, localPositionX);

            // calculate it it to int value
            float indexVal = (relPos * MaxIndexValue);

            return indexVal;
        }

        /// <summary>
        /// Returns mean, median, and mode as elements in a vector
        /// 
        /// todo calc median and mode
        /// </summary>
        /// <returns></returns>
        public static Vector3 CalculateMeanMedianMode(List<BarData> bars)
        {
            float mean = 0f;
            float median = 0f;
            float mode = 0f;

            float sum = 0f;
            for(int i = 0; i < bars.Count; i++)
            {
                sum += bars[i].Value; 
            }

            mean = sum / bars.Count;

            return new Vector3(mean, median, mode);
        }


    }
}