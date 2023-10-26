using System.Collections.Generic;
using UnityEngine;

namespace BalanceEditor
{
    public interface IBalanceEditor
    {
        BalanceCurveSO InitializeNewBalanceCurveData(string _scriptableObjectPath);
        bool IsKeyFieldValue { get; set; }
        string NewFieldName { get; set; }
        int BalanceSliderValue { get; set; }
        BalanceCurveSO BalanceCurveData { get; set; }
        int MaxBalanceLevelValue { get; set; }
        List<BalanceData> BalanceList { get; set; }
        List<BalanceMaxValue> MaxValues { get; set; }
        Dictionary<string, AnimationCurve> Curves { get; set; }
        Dictionary<string, float> InitialValues { get; set; }
        string[] ValueOptions { get; set; }
        string DataPath { get; set; }
        int StartKeyValue { get; set; }
        bool CurvesLoaded { get; set; }
        bool AutoSaveJson { get; set; }
        int NewFieldCount { get; set; }
        Dictionary<string, string> NewFieldNameMapping { get; set; }
        Dictionary<string, string> FieldNameMapping { get; set; }
        bool AllFieldsFolded { get; set; }
        string ScriptableObjectPath { get; set; }
        string AutoSavePath { get; set; }
        string AutoSaveFileName { get; set; }
        bool IsOpenCustomCurveEditorWindow { get; set; }
        void UpdateBalance(string _curveName = null);
        public bool UnsavedChange { get; set; }
        GUIStyle ButtonStyle { get; set; }
        void SaveCurve();
        void SaveDataToScriptableObject();
        void ApplyAutoSaveJsonFileName(string _fileNameWithoutExtension);
    }
}