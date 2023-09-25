using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace BalanceEditor
{
    public class BalanceEditor : EditorWindow, IBalanceEditor
    {
        #region Variable

        private IBalanceEditorUtilities editorUtilities = new BalanceEditorUtilities();
        private CustomCurveEditorWindow customCurveEditorWindow;
        public BalanceCurveSO BalanceCurveData { get; set; }
        
        public List<BalanceData> BalanceList { get; set; } = new();
        public List<BalanceMaxValue> MaxValues { get; set; } = new();

        public Dictionary<string, AnimationCurve> Curves { get; set; } = new();
        public Dictionary<string, string> NewFieldNameMapping { get; set; } = new();
        public Dictionary<string, string> FieldNameMapping { get; set; } = new();
        public Dictionary<string, float> InitialValues { get; set; } = new(); 

        private Vector2 scrollPosition;

        public string[] ValueOptions { get; set; }
        public string NewFieldName { get; set; }

        public int BalanceSliderValue { get; set; } = 30;
        public int MaxBalanceLevelValue { get; set; } = 100;
        public int NewFieldCount { get; set; }
        public int StartKeyValue { get; set; } = 1;
        private int selectedValueIndex;

        private int selectedSaveType;
        private readonly string[] saveTypeOptions = { "JSON", "CSV" }; 
        private int selectedLoadType;
        private readonly string[] loadTypeOptions = { "JSON", "CSV" }; 

        public bool CurvesLoaded { get; set; }
        public bool AllFieldsFolded { get; set; } = true;
        public bool IsKeyFieldValue { get; set; } = true;
        public bool AutoSaveJson { get; set; }
        public string DataPath { get; set; } = "Assets/BalanceEditor/Data/";
        private string DefaultScriptableObjectPath { get; set; } = "Assets/BalanceEditor/Data/BalanceCurveData.asset";
        public string ScriptableObjectPath { get; set; } = "";
        public string AutoSavePath { get; set; } = "Assets/BalanceEditor/Data/";
        public string AutoSaveFileName { get; set; } = "";
        private string previousAutoSaveJsonFileName = "";
        
        public bool IsOpenCustomCurveEditorWindow { get; set; }
        public bool UnsavedChange { get; set; }
        private bool changeAutoSaveJsonFileName;

        private const float LABEL_WIDTH = 80f;
        public GUIStyle ButtonStyle { get; set; }
        private Texture2D normalBackground;
        private Texture2D hoverBackground;

        #endregion

        #region ShowWindow
        
        [MenuItem("Custom Editor/Balance Curve Editor")]
        public static void ShowWindow()
        {
            GetWindow<BalanceEditor>("Balance Curve Editor");
        }
        #endregion

        #region OnEnable
        private void OnEnable()
        {
            Initialize();
        }
        
        private void OnDisable()
        {
            if (UnsavedChange)
            {
                DisplayDialogUnsavedChanges();
            }
        }
        #endregion
        
        #region MakeTexture
        private Texture2D MakeTexture(Color _color)
        {
            var pixelArray = new Color[1];
            pixelArray[0] = _color;

            var texture = new Texture2D(1, 1);
            texture.SetPixels(pixelArray);
            texture.Apply();

            return texture;
        }
        #endregion
        
        #region UpdateBalance
        // Updates the balance for a given curve name and repaints the window.
        public void UpdateBalance(string _curveName = null)
        {
            editorUtilities.UpdateBalance(_curveName);
            Repaint();
        }
        #endregion
        
        #region CustomCurveEditorInit
        // Initializes the custom curve editor window with current curves and field name mappings.
        private void CustomCurveEditorInit()
        {
            customCurveEditorWindow.Initialize(Curves, FieldNameMapping, this, this);
        }
        #endregion
        
        #region SaveNewDataToScriptableObject
        // Saves new data to a scriptable object and returns its path.
        public void SaveDataToScriptableObject()
        {
            editorUtilities.SaveDataToScriptableObject();
        }
        #endregion
        
        #region SaveCurve
        public void SaveCurve()
        {
            editorUtilities.SaveDataToScriptableObject();
            editorUtilities.ReLoadDataFromScriptableObject();
        }
        #endregion
        
        #region Initialize
        // Sets up initial state of variables for the balance editor.
        private void Initialize()
        {
            editorUtilities.SetEditorInstance(this);
            ValueOptions = new[] { "Any Field" };

            normalBackground = MakeTexture(Color.gray);
            hoverBackground = MakeTexture(Color.black);
            
            Curves = new Dictionary<string, AnimationCurve>();

            string path = "Assets/BalanceEditor/Data/";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets/BalanceEditor", "Data");
            }
            
            if (!DefaultScriptableObjectPath.Equals(ScriptableObjectPath) && string.IsNullOrEmpty(ScriptableObjectPath))
            {
                ScriptableObjectPath = DefaultScriptableObjectPath;
            }
            else
            {
                if (string.IsNullOrEmpty(ScriptableObjectPath))
                {
                    ScriptableObjectPath = DefaultScriptableObjectPath;
                }
            }
            BalanceCurveData = AssetDatabase.LoadAssetAtPath<BalanceCurveSO>(ScriptableObjectPath);

            LoadBalanceCurveData();  
        }
        
        /* Loads data from a ScriptableObject. If there is existing data at the specified path,
             it initializes with that. Otherwise, it creates new data. */
        private void LoadBalanceCurveData()
        {
            if (File.Exists(ScriptableObjectPath)) 
            {
                InitializeExistingData();
            }
            else
            {
                InitializeNewData();
            }
        }

        private void InitializeExistingData()
        {
            MaxBalanceLevelValue = BalanceCurveData.maxBalanceLevelValue;
            BalanceSliderValue = BalanceCurveData.balanceSliderValue;
            StartKeyValue = BalanceCurveData.startKeyValue;
            CurvesLoaded = true; 
            IsKeyFieldValue = false; 

            editorUtilities.ReLoadDataFromScriptableObject();
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ScriptableObjectPath);
            AutoSaveFileName = fileNameWithoutExtension;
            if (!string.IsNullOrEmpty(BalanceCurveData.autoSaveJsonFileName))
            {
                AutoSaveFileName = BalanceCurveData.autoSaveJsonFileName;
                previousAutoSaveJsonFileName = BalanceCurveData.autoSaveJsonFileName;
            }
            else
            {
                BalanceCurveData.autoSaveJsonFileName = AutoSaveFileName;
                previousAutoSaveJsonFileName = BalanceCurveData.autoSaveJsonFileName;
            }

            foreach (var field in BalanceCurveData.fields) 
            {
                ProcessField(field);
            }
            editorUtilities.UpdateBalance();
            if (IsOpenCustomCurveEditorWindow)
            {
                CustomCurveEditorInit();
            }
        }

        private void InitializeNewData()
        {
            BalanceCurveData = CreateInstance<BalanceCurveSO>();
            BalanceCurveData.fields = new List<FieldData>();
            BalanceCurveData.animationCurves = new List<BalanceAnimationCurveData>();
            BalanceCurveData.maxValues = new List<BalanceMaxValue>();

            AutoSaveFileName = "BalanceCurveData";

            BalanceCurveData.autoSaveJsonFileName = AutoSaveFileName;
            previousAutoSaveJsonFileName = BalanceCurveData.autoSaveJsonFileName;
            BalanceCurveData.maxBalanceLevelValue = MaxBalanceLevelValue;
            BalanceCurveData.balanceSliderValue = BalanceSliderValue;

            if (BalanceCurveData.fields.Count == 0)
            {
                var newFieldData = editorUtilities.CreateNewField();
                BalanceCurveData.fields.Add(newFieldData);
                IsKeyFieldValue = false;
                newFieldData = editorUtilities.CreateNewField();
                BalanceCurveData.fields.Add(newFieldData);
                CurvesLoaded = false;
                editorUtilities.Initialize();
                AssetDatabase.CreateAsset(BalanceCurveData, ScriptableObjectPath);
                AssetDatabase.SaveAssets();
            }
            else
            {
                editorUtilities.Initialize();
                editorUtilities.UpdateBalance();
                IsKeyFieldValue = false;
                AssetDatabase.CreateAsset(BalanceCurveData, ScriptableObjectPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void ProcessField(FieldData _field)
        {
            var fieldName = _field.fieldName;
            var maxValue = BalanceCurveData.maxValues.FirstOrDefault(_mv => _mv.fieldName == fieldName);
            
            if (_field.fieldType == FieldType.Int)
            {
                InitialValues[fieldName] = int.Parse(_field.fieldValue);
            }
            else if (_field.fieldType == FieldType.Float)
            {
                InitialValues[fieldName] = float.Parse(_field.fieldValue);
            } 
            if (maxValue == null)
            {
                var fieldValue = float.Parse(_field.fieldValue);
                MaxValues.Add(new BalanceMaxValue(fieldName, fieldValue));
            }
            else
            {
                MaxValues.Add(maxValue);
            }
        }
        #endregion
        
        #region InitializeNewBalanceCurveData
        public BalanceCurveSO InitializeNewBalanceCurveData(string _scriptableObjectPath)
        {
            var newBalanceCurveData = CreateInstance<BalanceCurveSO>();
            newBalanceCurveData.fields = new List<FieldData>();
            newBalanceCurveData.animationCurves = new List<BalanceAnimationCurveData>();
            newBalanceCurveData.maxValues = new List<BalanceMaxValue>();
            AssetDatabase.CreateAsset(newBalanceCurveData, _scriptableObjectPath);
            AssetDatabase.SaveAssets();

            newBalanceCurveData.fields.Clear();
            newBalanceCurveData.animationCurves.Clear();
            Curves.Clear();
            BalanceList.Clear();
            MaxValues.Clear();
            newBalanceCurveData.maxBalanceLevelValue = BalanceCurveData.maxBalanceLevelValue;
            newBalanceCurveData.balanceSliderValue = BalanceCurveData.balanceSliderValue;
            BalanceCurveData = newBalanceCurveData;
            
            return newBalanceCurveData;
        }
        #endregion
        
        #region DisplayMaxLevelSlider
        /* Display a slider to control maximum level value in Unity's Inspector Window */
        private void DisplayMaxLevelSlider()
        {
            EditorGUILayout.BeginHorizontal();
    
            GUILayout.Label("Max Level", GUILayout.Width(80));
            var oldMaxLevel = BalanceSliderValue;
            BalanceSliderValue = EditorGUILayout.IntSlider(BalanceSliderValue, 1, MaxBalanceLevelValue, GUILayout.MinWidth(100));
            if (oldMaxLevel != BalanceSliderValue)
            {
                editorUtilities.OnMaxLevelSliderValueChanged(BalanceSliderValue);
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region DisplayAutoSaveJson
        /* Display auto save JSON toggle button and related options in Unity's Inspector Window */ 
        private void DisplayAutoSaveJson()
        {
            GUIStyle blueBoldLabel = new GUIStyle(EditorStyles.boldLabel)
            {
                normal =
                {
                    textColor = Color.green
                }
            };

            GUIStyle redBoldLabel = new GUIStyle(EditorStyles.boldLabel)
            {
                normal =
                {
                    textColor = Color.red
                }
            };

            EditorGUILayout.BeginHorizontal();

            AutoSaveJson = EditorGUILayout.Toggle(AutoSaveJson, GUILayout.Width(20));

            GUIStyle currentLabelStyle = AutoSaveJson ? redBoldLabel : blueBoldLabel;
            GUIContent labelContent = new GUIContent("Auto Save JSON");

            if (GUILayout.Button(labelContent, currentLabelStyle, GUILayout.Width(100)))
            {
                AutoSaveJson = !AutoSaveJson;
                if (AutoSaveJson)
                {
                    editorUtilities.InitializeBalanceFieldsList();
                    UpdateBalance();
                }
            }
            BalanceCurveData.autoSaveJson = AutoSaveJson;
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField(" | Path", GUILayout.Width(45)); 
            AutoSavePath = EditorGUILayout.TextField(AutoSavePath, GUILayout.Width(200));
            if (GUILayout.Button("Browse", ButtonStyle, GUILayout.Width(75)))
            {
                AutoSavePath = editorUtilities.OpenJsonFolderPath();
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("JSON File Name", GUILayout.Width(100));
            previousAutoSaveJsonFileName = EditorGUILayout.TextField(previousAutoSaveJsonFileName, GUILayout.Width(120));
            
            DisplayApplyButton();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region DisplayCustomCurves
        private void DisplayApplyButton()
        {
            GUI.enabled = changeAutoSaveJsonFileName;
            if (GUILayout.Button("Apply", ButtonStyle, GUILayout.Width(75)))
            {
                ApplyAutoSaveJsonFileName(previousAutoSaveJsonFileName);
            }
            GUI.enabled = true;
        }
        #endregion
        
        #region DisplayCustomCurves
        public void ApplyAutoSaveJsonFileName(string _name)
        {
            editorUtilities.UpdateBalance();
            
            AutoSaveFileName = _name;
            BalanceCurveData.autoSaveJsonFileName = _name;
            previousAutoSaveJsonFileName = _name;
    
            changeAutoSaveJsonFileName= false;
            UnsavedChange = false;
        }
        #endregion
        
        #region DisplayCustomCurves
        /* Displays Custom Curves section header in Unity's Inspector Window */
        private void DisplayCustomCurves()
        {
            EditorGUILayout.LabelField("Custom Curves", EditorStyles.boldLabel, GUILayout.Width(120));
            if (GUILayout.Button("Edit", ButtonStyle, GUILayout.Width(75)))
            {
                if (UnsavedChange)
                {
                    DisplayDialogUnsavedChanges();
                }
                var window = GetWindow<CustomCurveEditorWindow>("Curve Editor");
                window.ShowWindow(Curves, FieldNameMapping, this, this);
                window.Repaint();
                window.SetEditorInstance(this);
                customCurveEditorWindow = window;
                IsOpenCustomCurveEditorWindow = true;
            }
        }
        #endregion

        private void DisplayDialogUnsavedChanges()
        {
            if (EditorUtility.DisplayDialog("Unsaved Changes",
                    "You have unsaved changes. Would you like to save them before closing the Balance Editor?",
                    "Save", "Cancel"))
            {
                editorUtilities.SaveDataToScriptableObject();
                UnsavedChange = false;
                if (IsOpenCustomCurveEditorWindow) 
                {
                    customCurveEditorWindow.unsavedChange = false;
                    customCurveEditorWindow.Close();
                }
            }
        }
        
        #region DisplaySelectedValueDropdown
        private void DisplaySelectedValueDropdown()
        {
            var oldValueIndex = selectedValueIndex;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter", EditorStyles.boldLabel, GUILayout.Width(150));
            selectedValueIndex = EditorGUILayout.Popup(selectedValueIndex, ValueOptions); 
            EditorGUILayout.EndHorizontal();

            if (oldValueIndex != selectedValueIndex)
            {
                editorUtilities.UpdateBalance();
            }
        }
        #endregion
        
        #region UpdateDecimalPoint
        private void UpdateDecimalPoint(FieldData _changedTypeFieldData)
        {
            string[] decimalPointOptions = { "0", "1", "2", "3", "4" }; 
            if (!int.TryParse(_changedTypeFieldData.fieldDecimalPoint, out var selectedDecimalPoint)) 
            {
                selectedDecimalPoint = 2;
            }

            var oldSelectedDecimalPoint = selectedDecimalPoint;
            selectedDecimalPoint = EditorGUILayout.Popup("Decimal", selectedDecimalPoint, decimalPointOptions);

            _changedTypeFieldData.fieldDecimalPoint = selectedDecimalPoint.ToString();
            if (oldSelectedDecimalPoint != selectedDecimalPoint)
            {
                editorUtilities.InitializeBalanceFieldsList();
                editorUtilities.UpdateBalance();
            }
        }
        #endregion
        
        #region DisplayCustomFields
        private void DisplayCustomFields()
        {
            NewFieldNameMapping = FieldNameMapping;
            NewFieldCount = NewFieldNameMapping.Count;
            NewFieldName = "NewField" + NewFieldCount;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Custom Fields", EditorStyles.boldLabel, GUILayout.Width(145));
            string buttonText = AllFieldsFolded ? "Fold All Fields" : "UnFold All Fields";
            if (GUILayout.Button(buttonText, ButtonStyle, GUILayout.Width(120)))
            {
                AllFieldsFolded = !AllFieldsFolded;
            }

            GUILayout.Space(50);
            DisplayCustomCurves();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal(); 
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            if (AllFieldsFolded)
            {
                var fieldData = BalanceCurveData.fields[0];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Key Field Value", EditorStyles.boldLabel, GUILayout.Width(145));
                fieldData.fieldName =  EditorGUILayout.TextField(BalanceCurveData.fields[0].fieldName);
                fieldData.fieldType = FieldType.Int;
                EditorGUILayout.LabelField("Start Value", GUILayout.Width(70));
                StartKeyValue = EditorGUILayout.IntField(StartKeyValue);
                fieldData.fieldValue = StartKeyValue.ToString();
                BalanceCurveData.startKeyValue = StartKeyValue;
                EditorGUILayout.EndHorizontal();

                bool isFloat = false;
                for (var i = 1; i < BalanceCurveData.fields.Count; i++)
                {
                    fieldData = BalanceCurveData.fields[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(fieldData.fieldName, GUILayout.Width(145));

                    if (GUILayout.Button("Delete", ButtonStyle, GUILayout.Width(60)))
                    {
                        DeleteField(fieldData);
                        editorUtilities.UpdateValueOptions();
                        EditorGUILayout.EndVertical();
                    }
                    
                    var oldFieldType = fieldData.fieldType;
                    fieldData.fieldType = (FieldType)EditorGUILayout.EnumPopup(fieldData.fieldType, GUILayout.Width(50));
                    
                    if (oldFieldType != fieldData.fieldType)
                    {
                        if (fieldData.fieldType == FieldType.Int)
                        {
                            fieldData.fieldDecimalPoint = "0";
                        }
                        editorUtilities.InitializeBalanceFieldsList();
                    }
                    
                    var oldFieldName = fieldData.fieldName;
                    fieldData.fieldName = EditorGUILayout.TextField(fieldData.fieldName, GUILayout.Width(120));

                    EditorGUILayout.Space(1);
                    EditorGUILayout.LabelField("Value", GUILayout.Width(40));
                    
                    if (float.TryParse(fieldData.fieldValue, out var parsedFloat))
                    {
                        if (fieldData.fieldType == FieldType.Int)
                        {
                            var parsedInt = Mathf.RoundToInt(parsedFloat);
                            fieldData.fieldValue = EditorGUILayout.IntField(parsedInt).ToString();
                            isFloat = false;
                        }
                        else
                        {
                            fieldData.fieldValue = EditorGUILayout.FloatField(parsedFloat).ToString("F"+fieldData.fieldDecimalPoint);
                            isFloat = true;
                        }
                    }

                    EditorGUILayout.Space(1);
                    EditorGUILayout.LabelField("MaxValue", GUILayout.Width(60));
                    EditorGUILayout.Space(1);

                    if (oldFieldName != fieldData.fieldName)
                    {
                        NewFieldNameMapping[oldFieldName] = fieldData.fieldName;

                        if (Curves.ContainsKey(oldFieldName))
                        {
                            var curve = Curves[oldFieldName];
                            Curves.Remove(oldFieldName);
                            Curves[fieldData.fieldName] = curve;

                            var curveData =
                                BalanceCurveData.animationCurves.FirstOrDefault(_ac => _ac.fieldName == oldFieldName);
                            if (curveData != null)
                            {
                                curveData.fieldName = fieldData.fieldName;
                            }
                        }

                        for (var index = 0; index < MaxValues.Count; index++)
                        {
                            if (MaxValues[index].fieldName == oldFieldName)
                            {
                                var updatedMaxValue =
                                    new BalanceMaxValue(fieldData.fieldName, MaxValues[index].maxValue);
                                MaxValues[index] = updatedMaxValue;

                                var maxValueIndex =
                                    BalanceCurveData.maxValues.FindIndex(_mv => _mv.fieldName == oldFieldName);
                                if (maxValueIndex >= 0)
                                {
                                    BalanceCurveData.maxValues[maxValueIndex] = updatedMaxValue;
                                }

                                break;
                            }
                        }
                        editorUtilities.InitializeBalanceFieldsList();
                        editorUtilities.UpdateValueOptions();

                        if (IsOpenCustomCurveEditorWindow)
                        {
                            customCurveEditorWindow.CurveEditorUpdate();
                            customCurveEditorWindow.Repaint();
                        }
                    }

                    float oldMaxValue = 0;
                    foreach (var maxValue in MaxValues)
                    {
                        if (maxValue.fieldName == fieldData.fieldName)
                        {
                            oldMaxValue = maxValue.maxValue;
                            break;
                        }
                    }

                    float newMaxValue = 0;
                    if (fieldData.fieldType == FieldType.Int)
                    {
                        newMaxValue = EditorGUILayout.IntField((int)oldMaxValue, GUILayout.Width(80));
                    }
                    else if (fieldData.fieldType == FieldType.Float)
                    {
                        newMaxValue = EditorGUILayout.FloatField(oldMaxValue, GUILayout.Width(80));
                    }

                    if (!Mathf.Approximately(oldMaxValue, newMaxValue))
                    {
                        var updatedBalanceVal = new BalanceMaxValue(fieldData.fieldName, newMaxValue);
    
                        int indexInMv = MaxValues.FindIndex(_mv => _mv.fieldName == fieldData.fieldName);
    
                        // Update or add the value in MaxValues
                        if(indexInMv >= 0) 
                            MaxValues[indexInMv] = updatedBalanceVal; 
                        else 
                            MaxValues.Add(updatedBalanceVal);

    
                        int indexInBcd= BalanceCurveData.maxValues.FindIndex(_mv => _mv.fieldName == fieldData.fieldName);

                        // Update or add the value in BalanceCurveData.maxValues
                        if(indexInBcd >= 0) 
                            BalanceCurveData.maxValues[indexInBcd] = updatedBalanceVal;  
                        else 
                            BalanceCurveData.maxValues.Add(updatedBalanceVal);       
                    }

                    NewFieldNameMapping[oldFieldName] = fieldData.fieldName;

                    if (isFloat)
                    {
                        UpdateDecimalPoint(fieldData);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add Field", ButtonStyle))
            {
                AddField();
                editorUtilities.UpdateValueOptions();
            }

            FieldNameMapping = NewFieldNameMapping;

            EditorGUILayout.EndVertical();
        }
        #endregion
        
        #region AddField
        /* Adds a new field to BalanceCurveData.fields List and updates all relevant properties */
        private void AddField()
        {
            if (BalanceCurveData.fields == null)
            {
                BalanceCurveData.fields = new List<FieldData>();
            }

            while (NewFieldNameMapping.ContainsKey(NewFieldName))
            {
                NewFieldCount++;
                NewFieldName = "NewField" + NewFieldCount;
            }

            var newField = editorUtilities.CreateNewField();
            BalanceCurveData.fields.Add(newField);

            if (BalanceList == null)
            {
                BalanceList = new List<BalanceData>();
            }

            foreach (var balanceData in BalanceList)
            {
                if (balanceData.balanceDataFields == null)
                {
                    balanceData.balanceDataFields = new List<FieldData>();
                }
        
                if (balanceData.balanceDataFields.Count < BalanceCurveData.fields.Count)
                {
                    var newBalanceField = new FieldData
                    {
                        fieldName = newField.fieldName,
                        fieldType = newField.fieldType,
                        fieldValue = newField.fieldValue
                    };
                    balanceData.balanceDataFields.Add(newBalanceField);
                }
            }
            InitialValues[newField.fieldName] = float.Parse(newField.fieldValue);
            
            var newMaxValue = float.Parse(newField.fieldValue);
            
            if (BalanceCurveData.maxValues.All(_mv => _mv.fieldName != newField.fieldName))
            {
                BalanceCurveData.maxValues.Add(new BalanceMaxValue(newField.fieldName, newMaxValue*10));
                MaxValues.Add(new BalanceMaxValue(newField.fieldName, newMaxValue*10));
            }

            CurvesLoaded = false;
            editorUtilities.InitializeBalanceFieldsList();
            if (IsOpenCustomCurveEditorWindow)
            {
                CustomCurveEditorInit();
            }
        }
        #endregion
        
        #region DeleteField
        
        private void DeleteField(FieldData _fieldData)
        {
            BalanceCurveData.fields.Remove(_fieldData);

            if (Curves.ContainsKey(_fieldData.fieldName))
            {
                Curves.Remove(_fieldData.fieldName);
            }

            foreach (var balance in BalanceList)
            {
                var fieldToRemove = balance.balanceDataFields.FirstOrDefault(_f => _f.fieldName == _fieldData.fieldName);
                if (fieldToRemove != null)
                {
                    balance.balanceDataFields.Remove(fieldToRemove);
                }
            }
            
            var curveToRemove = BalanceCurveData.animationCurves.FirstOrDefault(_ac => _ac.fieldName == _fieldData.fieldName);
            if (curveToRemove != null)
            {
                BalanceCurveData.animationCurves.Remove(curveToRemove);
            }

            var maxValueToRemove = BalanceCurveData.maxValues.FirstOrDefault(_mv => _mv.fieldName == _fieldData.fieldName);
            if (maxValueToRemove != null)
            {
                MaxValues.Remove(maxValueToRemove);
                BalanceCurveData.maxValues.Remove(maxValueToRemove);
            }

            if (FieldNameMapping.ContainsKey(_fieldData.fieldName))
            {
                FieldNameMapping.Remove(_fieldData.fieldName);
            }
            NewFieldNameMapping.Remove(NewFieldName);
            NewFieldCount--;
            if (IsOpenCustomCurveEditorWindow)
            {
                CustomCurveEditorInit();
            }
            editorUtilities.InitializeBalanceFieldsList();
            editorUtilities.UpdateBalance();
        }
        #endregion
        
        #region DisplayLevelEditor
        private void DisplayLevelEditor(int _i)
        {
            var keyName = BalanceCurveData.fields[0].fieldName;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField($"{keyName} {StartKeyValue + _i}",
                    EditorStyles.boldLabel, GUILayout.Width(145));
            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            var valueChanged = false;

            for (var j = 1; j < BalanceCurveData.fields.Count; j++)
            {
                var fieldData = BalanceCurveData.fields[j];
                if (selectedValueIndex > 0 && !fieldData.fieldName.Equals(ValueOptions[selectedValueIndex]))
                {
                    continue;
                }
                EditorGUILayout.BeginVertical();

                if (_i == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(fieldData.fieldName, GUILayout.Width(140));

                    var oldValue = fieldData.fieldValue;
                    if (fieldData.fieldType == FieldType.Int || fieldData.fieldType == FieldType.Float)
                    {
                        if (float.TryParse(fieldData.fieldValue, out var parsedFloat))
                        {
                            if (fieldData.fieldType == FieldType.Int)
                            {
                                var parsedInt = Mathf.RoundToInt(parsedFloat);
                                fieldData.fieldValue = EditorGUILayout.IntField(parsedInt).ToString();
                            }
                            else
                            {
                                fieldData.fieldValue = EditorGUILayout.FloatField(parsedFloat).ToString("F"+fieldData.fieldDecimalPoint);
                            }
                        }
                        else
                        {
                            fieldData.fieldValue = "0";
                        }
                    }

                    if (oldValue != fieldData.fieldValue)
                    {
                        InitialValues[fieldData.fieldName] = float.Parse(fieldData.fieldValue);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(fieldData.fieldName, GUILayout.Width(140));

                    var currentFieldData = BalanceList[_i].balanceDataFields[j];

                    var oldFieldValue = currentFieldData.fieldValue;

                    if (float.TryParse(currentFieldData.fieldValue, out var parsedFloat))
                    {
                        if (currentFieldData.fieldType == FieldType.Int)
                        {
                            var parsedInt = Mathf.RoundToInt(parsedFloat);
                            currentFieldData.fieldValue = EditorGUILayout.IntField(parsedInt).ToString();
                        }
                        else if (currentFieldData.fieldType == FieldType.Float)
                        {
                            currentFieldData.fieldValue = EditorGUILayout.FloatField(parsedFloat).ToString("F"+fieldData.fieldDecimalPoint);
                        }
                    }
                    else
                    {
                        currentFieldData.fieldValue = "0";
                    }

                    if (oldFieldValue != currentFieldData.fieldValue)
                    {
                        valueChanged = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (valueChanged)
            {
                editorUtilities.UpdateBalance();
            }
        }
        #endregion
        
        #region DisplayBalanceCurveDataField
        private void DisplayBalanceCurveDataField()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Balance Curve Data", EditorStyles.boldLabel, GUILayout.Width(150));
            var oldBalanceCurveData = BalanceCurveData;
            BalanceCurveData = (BalanceCurveSO)EditorGUILayout.ObjectField(BalanceCurveData, typeof(BalanceCurveSO), false);
            EditorGUILayout.EndHorizontal();

            if (BalanceCurveData != null && oldBalanceCurveData != BalanceCurveData)
            {
                string scriptableObjectPath;
                string fileNameWithoutExtension;
                if (BalanceCurveData.fields.Count == 0)
                {
                    editorUtilities.ResetToInitialValues();
                    scriptableObjectPath = AssetDatabase.GetAssetPath(BalanceCurveData);
                    ScriptableObjectPath = scriptableObjectPath;
                    NewFieldCount = BalanceCurveData.fields.Count;
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ScriptableObjectPath);
                    AutoSaveFileName = fileNameWithoutExtension;
                    if (IsOpenCustomCurveEditorWindow)
                    {
                        CustomCurveEditorInit();
                    }
                    return;
                }
                
                CurvesLoaded = false;
                editorUtilities.ReLoadDataFromScriptableObject();
                editorUtilities.InitializeBalanceFieldsList(); 
                editorUtilities.UpdateValueOptions();
                editorUtilities.UpdateBalance();
                
                scriptableObjectPath = AssetDatabase.GetAssetPath(BalanceCurveData);
                ScriptableObjectPath = scriptableObjectPath;
                NewFieldCount = BalanceCurveData.fields.Count;
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ScriptableObjectPath);
                AutoSaveFileName = fileNameWithoutExtension;
                if (IsOpenCustomCurveEditorWindow)
                {
                    CustomCurveEditorInit();
                }
            }
        }
        #endregion
        
        #region OnGUI
        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = LABEL_WIDTH;
            if (ButtonStyle == null)
            {
                ButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    border = new RectOffset(10, 10, 10, 10),
                    normal =
                    {
                        textColor = Color.white,
                        background = normalBackground
                    },
                    active =
                    {
                        // buttonStyle.hover.textColor = Color.red;
                        // buttonStyle.hover.background = hoverBackground;
                        textColor = Color.black,
                        background = hoverBackground
                    }
                };
            }

            EditorGUILayout.BeginHorizontal();
            LoadTypeEnumPopup();

            if (GUILayout.Button("Load JSON or CSV File", ButtonStyle))
            {
                string selectedOption = loadTypeOptions[selectedLoadType];

                if (selectedOption == "JSON")
                {
                    editorUtilities.LoadJsonData();
                }
                else if (selectedOption == "CSV")
                {
                    editorUtilities.LoadCsvData();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Load Data From ScriptableObject", ButtonStyle))
            {
                CurvesLoaded = false;
                editorUtilities.LoadDataFromScriptableObject();
                editorUtilities.ReLoadDataFromScriptableObject();
                editorUtilities.UpdateBalance();
            }
            
            if (GUILayout.Button("ReSet", ButtonStyle))
            {
                editorUtilities.ResetToInitialValues();
            }

            DisplayBalanceCurveDataField();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Max Level Value",GUILayout.Width(148));
            var oldMaxSliderValue = MaxBalanceLevelValue;
            MaxBalanceLevelValue = EditorGUILayout.IntField(MaxBalanceLevelValue);
            if (oldMaxSliderValue != MaxBalanceLevelValue)
            {
                editorUtilities.OnMaxValueChanged(MaxBalanceLevelValue);
            }

            if (BalanceCurveData != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space(2);
                DisplayMaxLevelSlider();
                EditorGUILayout.EndHorizontal();

                if (ShouldUpdateValues())
                {
                    editorUtilities.UpdateBalance();
                    UnsavedChange = true;
                }
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                DisplayAutoSaveJson();
                EditorGUILayout.EndHorizontal();
                
                if (ShouldUpdateValues())
                {
                    AutoSaveFileName = previousAutoSaveJsonFileName;
                    changeAutoSaveJsonFileName = BalanceCurveData.autoSaveJsonFileName != previousAutoSaveJsonFileName;
                    UnsavedChange = true;
                }
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginVertical(GUI.skin.box);
                DisplayCustomFields();
                DisplaySelectedValueDropdown();
                EditorGUILayout.EndVertical();

                if (ShouldUpdateValues())
                {
                    editorUtilities.UpdateBalance();
                    UnsavedChange = true;
                }
                
                EditorGUILayout.BeginVertical(GUI.skin.box);
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                
                EditorGUI.BeginChangeCheck();

                for (var i = 0; i < BalanceSliderValue; i++)
                {
                    DisplayLevelEditor(i);
                }

                EditorGUILayout.EndVertical();

                if (ShouldUpdateValues())
                {
                    editorUtilities.UpdateBalance();
                    UnsavedChange = true;
                }
                
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal();
                SaveTypeEnumPopup();

                if (GUILayout.Button("Save Data To JSON or CSV File", ButtonStyle))
                {
                    editorUtilities.InitializeBalanceFieldsList();
                    UpdateBalance();
                    
                    string selectedOption = saveTypeOptions[selectedSaveType];

                    if (selectedOption == "JSON")
                    {
                        editorUtilities.SaveJsonData(true);
                    }
                    else if (selectedOption == "CSV")
                    {
                        editorUtilities.SaveCsvData(true);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Save Curves To ScriptableObject", ButtonStyle))
                {
                    editorUtilities.SaveDataToScriptableObject();
                    UnsavedChange = false;
                    if (customCurveEditorWindow != null)
                    {
                        customCurveEditorWindow.unsavedChange = false;
                    }
                }

                if (GUILayout.Button("Save Curves To New ScriptableObject", ButtonStyle))
                {
                    editorUtilities.SaveNewDataToScriptableObject(true);
                    UnsavedChange = false;
                    if (customCurveEditorWindow != null)
                    {
                        customCurveEditorWindow.unsavedChange = false;
                    }
                }
            }
            else
            {
                GUILayout.Label("No JSON file loaded");
            }
        }
        #endregion
        
        #region LoadTypeEnumPopup
        private void LoadTypeEnumPopup()
        {
            selectedLoadType = EditorGUILayout.Popup("LoadType", selectedLoadType, loadTypeOptions);
        }
        #endregion
        
        #region SaveTypeEnumPopup
        private void SaveTypeEnumPopup()
        {
            selectedSaveType = EditorGUILayout.Popup("SaveType", selectedSaveType, saveTypeOptions);
        }
        #endregion
        
        #region ShouldUpdateValues
        private bool ShouldUpdateValues()
        {
            if (EditorGUI.EndChangeCheck())
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}