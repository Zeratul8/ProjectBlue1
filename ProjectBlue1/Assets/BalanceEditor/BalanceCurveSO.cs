using System.Collections.Generic;
using UnityEngine;

namespace BalanceEditor
{
    [System.Serializable]
    public class FieldData
    {
        public string fieldName = "fieldName";
        public FieldType fieldType = FieldType.Int;
        public string fieldValue = "value";
        public string fieldDecimalPoint = "0";
    }

    public enum FieldType
    {
        Int,
        Float,
        // String,
    }

    [System.Serializable]
    public class BalanceData
    {
        public List<FieldData> balanceDataFields;
    }
    
    [System.Serializable]
    public class BalanceAnimationCurveData
    {
        public string fieldName;
        public AnimationCurve curve;
    }
    
    [System.Serializable]
    public class BalanceMaxValue
    {
        public string fieldName;
        public float maxValue;
        
        public BalanceMaxValue(string _fieldName, float _value)
        {
            fieldName = _fieldName;
            maxValue = _value;
        }
    }

    [CreateAssetMenu(fileName = "BalanceCurveSO", menuName = "Balance Scriptable Object", order = 1)]
    public class BalanceCurveSO : ScriptableObject
    {
        public string autoSaveJsonFileName = "";
        public int maxBalanceLevelValue;
        public int balanceSliderValue;
        public int startKeyValue;
        public bool autoSaveJson;
        public List<FieldData> fields;
        public List<BalanceAnimationCurveData> animationCurves;
        public List<BalanceMaxValue> maxValues;
    }
}