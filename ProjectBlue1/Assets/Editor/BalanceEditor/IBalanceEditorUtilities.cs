namespace BalanceEditor
{
    public interface IBalanceEditorUtilities
    {
        void ReLoadDataFromScriptableObject();
        void UpdateBalance(string _curveName = null);
        FieldData CreateNewField();
        void InitializeBalanceCurves();
        void InitializeInitialValues();
        void InitializeBalanceFieldsList();
        void UpdateValueOptions();
        void SaveDataToScriptableObject();
        void SaveJsonData(bool _newFile = false);
        void SaveCsvData(bool _newFile = false);
        void OnMaxValueChanged(int _maxBalanceLevelValue);
        void ResetToInitialValues();
        void LoadJsonData();
        void OnMaxLevelSliderValueChanged(int _balanceSliderValue);
        void SetEditorInstance(IBalanceEditor _balanceCurveEditor);
        void LoadDataFromScriptableObject();
        string SaveNewDataToScriptableObject(bool _isNew);
        void Initialize();
        string OpenJsonFolderPath();
        void LoadCsvData();
    }
}