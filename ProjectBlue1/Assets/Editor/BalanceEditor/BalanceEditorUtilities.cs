using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace BalanceEditor
{
    public class BalanceEditorUtilities : IBalanceEditorUtilities
    {
        private IBalanceEditor editorInstance;
        
        #region SetEditorInstance
        // This method is used to set the instance of the balance editor.
        public void SetEditorInstance(IBalanceEditor _balanceEditor)
        {
            if (editorInstance != null) return;
            editorInstance = _balanceEditor;
        }
        #endregion

        #region Initialize
        // This method initializes various components of the balance editor.
        /* Calls methods to initialize balance curves, initial values, field list,
              update value options and balances, and save data as JSON. */
        public void Initialize()
        {
            InitializeBalanceCurves();
            InitializeInitialValues();
            InitializeBalanceFieldsList();
            UpdateValueOptions();
            UpdateBalance();
            SaveJsonData();
        }
        #endregion
        
        #region CreateNewField
        /* This method creates a new field with default parameters and adds it to existing fields list and curve dictionary.
         * If "IsKeyFieldValue" is true, creates a special key field with predefined name and value. */
        public FieldData CreateNewField()
        {
            FieldData newFieldData;
            if (editorInstance.IsKeyFieldValue)
            {
                editorInstance.NewFieldName = "Key Field Value";
 
                newFieldData = new FieldData
                {
                    fieldName = editorInstance.NewFieldName,
                    fieldType = FieldType.Int,
                    fieldValue = "1",
                    fieldDecimalPoint = "0"
                };
                return newFieldData;
            }
            editorInstance.NewFieldName = "NewField" + editorInstance.NewFieldCount;
            while (editorInstance.NewFieldNameMapping.ContainsKey(editorInstance.NewFieldName))
            {
                editorInstance.NewFieldCount++;
                editorInstance.NewFieldName = "NewField" + editorInstance.NewFieldCount;
            }
 
            newFieldData = new FieldData
            {
                fieldName = editorInstance.NewFieldName,
                fieldType = FieldType.Int,
                fieldValue = "100",
                fieldDecimalPoint = "0"
            };
            
            var newCurveData = new BalanceAnimationCurveData
            {
                fieldName = editorInstance.NewFieldName,
                curve = new AnimationCurve(
                    CreateKeyframe(0, 0),
                    CreateKeyframe(1, 1))
            };
            
            editorInstance.BalanceCurveData.animationCurves.Add(newCurveData);
            editorInstance.Curves[editorInstance.NewFieldName] = newCurveData.curve;

            return newFieldData;
        }
        #endregion

        #region CreateKeyframe
        private Keyframe CreateKeyframe(float _time, float _value)
        {
            return new Keyframe(_time, _value);
        }
        #endregion
        
        #region InitializeInitialValues
        public void InitializeInitialValues()
        {
            editorInstance.InitialValues.Clear();
            editorInstance.MaxValues.Clear();
            editorInstance.BalanceCurveData.maxValues.Clear();

            foreach (var field in editorInstance.BalanceCurveData.fields)
            {
                switch (field.fieldType)
                {
                    case FieldType.Int:
                        editorInstance.InitialValues[field.fieldName] = int.Parse(field.fieldValue);
                        editorInstance.MaxValues.Add(new BalanceMaxValue(field.fieldName, int.Parse(field.fieldValue)*10));
                        break;
                    case FieldType.Float:
                        editorInstance.InitialValues[field.fieldName] = float.Parse(field.fieldValue);
                        editorInstance.MaxValues.Add(new BalanceMaxValue(field.fieldName, float.Parse(field.fieldValue)*10));
                        break;
                }

                editorInstance.BalanceCurveData.maxValues.Add(new BalanceMaxValue(field.fieldName, float.Parse(field.fieldValue)*10));
            }
        }
        #endregion

        #region InitializeBalanceFieldsList
        public void InitializeBalanceFieldsList()
        {
            if (editorInstance.BalanceCurveData != null)
            {
                var fields = editorInstance.BalanceCurveData.fields;

                if (fields.Count == 0)
                {
                    Debug.LogError("InitializeBalanceFieldsList fields.count : " + fields.Count);
                    return;
                }
                if (!editorInstance.CurvesLoaded)
                {
                    InitializeBalanceCurves();
                    editorInstance.CurvesLoaded = true;
                }

                var newBalanceList = new List<BalanceData>();

                for (var i = 0; i < editorInstance.BalanceSliderValue; i++)
                {
                    var newBalanceData = new BalanceData();
                    newBalanceList.Add(newBalanceData);
                    newBalanceData.balanceDataFields = new List<FieldData>();

                    var fieldIndex = 0;

                    foreach (var fieldData in editorInstance.BalanceCurveData.fields)
                    {
                        var fieldName = fieldData.fieldName;
                        var type = fieldData.fieldType;
                        var value = fieldData.fieldValue;
                        var decimalPoint = fieldData.fieldDecimalPoint;

                        var newFieldData = new FieldData
                        {
                            fieldName = fieldName,
                            fieldType = type,
                            fieldValue = value,
                            fieldDecimalPoint = decimalPoint
                        };

                        float fieldVal;
                        if (i == 0)
                        {
                            fieldVal = float.Parse(newFieldData.fieldValue);
                        }
                        else
                        {
                            var time = (float)(i) / (editorInstance.BalanceSliderValue - 1);

                            if (editorInstance.Curves.ContainsKey(fieldData.fieldName) && editorInstance.InitialValues.TryGetValue(fieldData.fieldName, out var initialValue))
                            {
                                var level1FieldValue = initialValue;
                                fieldVal = level1FieldValue * editorInstance.Curves[fieldData.fieldName].Evaluate(time);
                            }
                            else
                            {
                                fieldVal = float.Parse(newFieldData.fieldValue);
                            }
                        }

                        if (fieldData.fieldType == FieldType.Int)
                        {
                            newFieldData.fieldValue = Mathf.RoundToInt(fieldVal).ToString();
                        }
                        else if (fieldData.fieldType == FieldType.Float)
                        {
                            newFieldData.fieldDecimalPoint = fieldData.fieldDecimalPoint;
                            newFieldData.fieldValue = fieldVal.ToString("F" + fieldData.fieldDecimalPoint);
                        }

                        if (fieldIndex == 0)
                        {
                            newFieldData.fieldType = FieldType.Int;
                            newFieldData.fieldValue = (editorInstance.StartKeyValue + i).ToString();
                        }

                        newBalanceData.balanceDataFields.Add(newFieldData);

                        fieldIndex++;
                    }
                }

                editorInstance.BalanceList = newBalanceList;
            }
        }
        #endregion
        
        #region InitializeBalanceCurves
        public void InitializeBalanceCurves()
        {
            editorInstance.Curves = new Dictionary<string, AnimationCurve>();
            if (editorInstance.BalanceCurveData != null)
            {
                var fields = editorInstance.BalanceCurveData.fields;

                if (fields != null && fields.Count > 0)
                {
                    string fieldName;
                    for (var i = 1; i < editorInstance.BalanceCurveData.animationCurves.Count; i++)
                    {
                        var animationCurveData = editorInstance.BalanceCurveData.animationCurves[i];
                        fieldName = animationCurveData.fieldName;
                        editorInstance.Curves[fieldName] = new AnimationCurve(animationCurveData.curve.keys);
                    }

                    for (var i = 1; i < editorInstance.BalanceCurveData.fields.Count; i++)
                    {
                        var field = editorInstance.BalanceCurveData.fields[i];
                        fieldName = field.fieldName;
                        if (editorInstance.Curves.ContainsKey(fieldName)) continue;
                        editorInstance.Curves[fieldName] =
                            new AnimationCurve(CreateKeyframe(0f, 0f), CreateKeyframe(1, 1));
                    }
                }
            }
        }
        #endregion

        #region ReLoadDataFromScriptableObject
        public void ReLoadDataFromScriptableObject()
        {
            editorInstance.Curves.Clear();
            editorInstance.MaxValues.Clear();

            foreach (var curveData in editorInstance.BalanceCurveData.animationCurves)
            {
                var storedCurve = new AnimationCurve(curveData.curve.keys)
                {
                    preWrapMode = curveData.curve.preWrapMode,
                    postWrapMode = curveData.curve.postWrapMode
                };
                editorInstance.Curves[curveData.fieldName] = storedCurve;
            }

            foreach (var maxValue in editorInstance.BalanceCurveData.maxValues)
            {
                editorInstance.MaxValues.Add(maxValue);
            }

            editorInstance.AutoSaveJson = editorInstance.BalanceCurveData.autoSaveJson;

            InitializeBalanceFieldsList();
            UpdateValueOptions();
            Debug.Log("Curves loaded from ScriptableObject"); 
        }
        #endregion
        
        #region LoadDataFromScriptableObject
        public void LoadDataFromScriptableObject()
        {
            editorInstance.ScriptableObjectPath = OpenScriptableObjectPath();
            editorInstance.BalanceCurveData = AssetDatabase.LoadAssetAtPath<BalanceCurveSO>(editorInstance.ScriptableObjectPath);
            Debug.Log("Data loaded from ScriptableObject");
        }
        #endregion

        #region SaveDataToScriptableObject
        public void SaveDataToScriptableObject()
        {
            SaveNewDataToScriptableObject(false);
        }
        #endregion
        
        #region SaveDataToScriptableObject
        public string SaveNewDataToScriptableObject(bool _isNew)
        {
            string path;
            if (_isNew)
            {
                path = SaveScriptableObjectPath();

                if (string.IsNullOrEmpty(path))
                {
                    return path;
                }
            }
            else
            {
                path = editorInstance.ScriptableObjectPath;
            }
            BalanceCurveSO newBalanceCurveData = ScriptableObject.CreateInstance<BalanceCurveSO>();

            var curveDataList = new List<BalanceAnimationCurveData>();
            var fieldsList = new List<FieldData>();
            var maxValuesList = new List<BalanceMaxValue>();

            foreach (var fieldData in editorInstance.BalanceCurveData.fields)
            {
                fieldsList.Add(new FieldData
                {
                    fieldName = fieldData.fieldName,
                    fieldType = fieldData.fieldType,
                    fieldValue = fieldData.fieldValue,
                    fieldDecimalPoint = fieldData.fieldDecimalPoint
                });

                if (editorInstance.Curves.TryGetValue(fieldData.fieldName, out var animationCurve))
                {
                    curveDataList.Add(new BalanceAnimationCurveData
                    {
                        fieldName = fieldData.fieldName,
                        curve = animationCurve
                    });
                }
            }

            foreach (var maxValue in editorInstance.BalanceCurveData.maxValues)
            {
                maxValuesList.Add(new BalanceMaxValue(
                    _fieldName: maxValue.fieldName,
                    _value: maxValue.maxValue
                ));
            }

            newBalanceCurveData.animationCurves = curveDataList;
            newBalanceCurveData.fields = fieldsList;
            newBalanceCurveData.maxValues = maxValuesList;
            newBalanceCurveData.maxBalanceLevelValue = editorInstance.BalanceCurveData.maxBalanceLevelValue;
            newBalanceCurveData.balanceSliderValue = editorInstance.BalanceCurveData.balanceSliderValue;
            newBalanceCurveData.startKeyValue = editorInstance.BalanceCurveData.startKeyValue;
            newBalanceCurveData.autoSaveJson = editorInstance.BalanceCurveData.autoSaveJson;
            newBalanceCurveData.autoSaveJsonFileName = editorInstance.BalanceCurveData.autoSaveJsonFileName;
            editorInstance.BalanceCurveData = newBalanceCurveData;
            EditorUtility.SetDirty(editorInstance.BalanceCurveData);

            if (!_isNew)
            {
                AssetDatabase.CreateAsset(newBalanceCurveData, editorInstance.ScriptableObjectPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Curves and fields saved to ScriptableObject at path: {path}");
                return editorInstance.ScriptableObjectPath;
            }
            if (!string.IsNullOrEmpty(path))
            {
                editorInstance.ScriptableObjectPath = path;
                
                int assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                path = path.Substring(assetsIndex);

                AssetDatabase.CreateAsset(newBalanceCurveData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Curves and fields saved to ScriptableObject at path: {path}");
            }
            return path;
        }
        #endregion
        
        #region LoadDataJson
        public void LoadJsonData()
        {
            editorInstance.DataPath = GetJsonFilePath();
            if (string.IsNullOrEmpty(editorInstance.DataPath)) return;

            var jsonString = File.ReadAllText(editorInstance.DataPath);
            var jsonObjects = DeserializeJson(jsonString);

            if (jsonObjects.Count == 0) return;

            Dictionary<string, string> maxDecimalPoints = new Dictionary<string, string>();
            SetMaxDecimalPoints(jsonObjects, maxDecimalPoints);

            string scriptableObjectPath = SaveNewDataToScriptableObject(true);
            if (string.IsNullOrEmpty(scriptableObjectPath)) return;

            editorInstance.ScriptableObjectPath = scriptableObjectPath;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptableObjectPath);
            editorInstance.ApplyAutoSaveJsonFileName(fileNameWithoutExtension);

            var newBalanceCurveData = editorInstance.InitializeNewBalanceCurveData(scriptableObjectPath);
            ParseFirstJsonObjFields(jsonObjects[0], newBalanceCurveData, maxDecimalPoints);
            ParseJsonObjects(jsonObjects, newBalanceCurveData);
            editorInstance.BalanceCurveData = newBalanceCurveData;

            InitializeBalanceFieldsList();
            UpdateValueOptions();
            UpdateBalance();
        }

        private void SetMaxDecimalPoints(List<Dictionary<string, object>> _jsonObjects, Dictionary<string, string> _maxDecimalPoints)
        {
            foreach (var jsonObj in _jsonObjects)
            {
                foreach (var kv in jsonObj)
                {
                    if (float.TryParse(kv.Value.ToString(), out var floatValue))
                    {
                        var decimalValue = Convert.ToDecimal(floatValue);
                        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
                        string decimalPlacesStr = decimalPlaces.ToString();

                        if (!_maxDecimalPoints.ContainsKey(kv.Key) || int.Parse(decimalPlacesStr) > int.Parse(_maxDecimalPoints[kv.Key]))
                        {
                            _maxDecimalPoints[kv.Key] = decimalPlacesStr;
                        }
                    }
                }
            }
        }

        private string GetJsonFilePath()
        {
            return EditorUtility.OpenFilePanel("Select JSON File", editorInstance.DataPath, "json");
        }

        public string OpenJsonFolderPath()
        {
            var path = EditorUtility.OpenFolderPanel("Select JSON File", editorInstance.AutoSavePath, "");
            var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
            return path.Substring(assetsIndex);
        }

        private List<Dictionary<string, object>> DeserializeJson(string _jsonString)
        {
            return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_jsonString);
        }

        private string OpenScriptableObjectPath()
        {
            var path = EditorUtility.OpenFilePanel("Open ScriptableObject", editorInstance.ScriptableObjectPath, "asset");
            var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
            if (assetsIndex >= 0)
            {
                return path.Substring(assetsIndex);
            }
            Debug.LogError("Invalid path. Please select a valid ScriptableObject within the Assets folder.");
            return editorInstance.ScriptableObjectPath;
        }

        private string SaveScriptableObjectPath()
        {
            var path = EditorUtility.SaveFilePanel("Save ScriptableObject as", editorInstance.ScriptableObjectPath, "NewBalanceSO", "asset");
            var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            return path.Substring(assetsIndex);
        }

        private void ParseFirstJsonObjFields(Dictionary<string, object> _jsonObj, BalanceCurveSO _newBalanceCurveData, Dictionary<string, string> _maxDecimalPoints)
        {
            var obj = JObject.FromObject(_jsonObj);
            bool isFirstField = true;

            foreach (var entry in obj)
            {
                string fieldName = entry.Key;

                if (entry.Value?.Type == JTokenType.Float || entry.Value?.Type == JTokenType.Integer)
                {
                    var fieldType = GetFieldType(entry.Value, out string stringValue, out string decimalPoint);

                    FieldData newFieldData = new FieldData
                    {
                        fieldName = fieldName,
                        fieldType = fieldType,
                        fieldValue = stringValue,
                        fieldDecimalPoint = _maxDecimalPoints[fieldName]
                    };

                    _newBalanceCurveData.fields.Add(newFieldData);

                    if (!isFirstField)
                    {
                        AnimationCurve curve = new AnimationCurve(CreateKeyframe(0f, 0f), CreateKeyframe(1f, 1f));
                        editorInstance.Curves[fieldName] = curve;
                        _newBalanceCurveData.animationCurves.Add(new BalanceAnimationCurveData
                        {
                            fieldName = fieldName,
                            curve = curve
                        });
                    }

                    isFirstField = false;
                }
            }
        }

        private FieldType GetFieldType(JToken _tokenValue, out string _stringValue, out string _decimalPoint)
        {
            FieldType fieldType;
            _decimalPoint = "0";

            if (_tokenValue.Type == JTokenType.Integer)
            {
                fieldType = FieldType.Int;
                var intValue = _tokenValue.Value<int>();
                _stringValue = intValue.ToString();
            }
            else
            {
                fieldType = FieldType.Float;
                var floatValue = _tokenValue.Value<float>();
                var decimalValue = Convert.ToDecimal(floatValue);
                var decimals = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
                var decimalPointStr = decimals.ToString();

                if (int.Parse(decimalPointStr) > int.Parse(_decimalPoint))
                {
                    _decimalPoint = decimalPointStr.Substring(0, Math.Min(decimalPointStr.Length, 4));
                }

                _stringValue = floatValue.ToString(CultureInfo.InvariantCulture);
            }

            return fieldType;
        }

        private void ParseJsonObjects(List<Dictionary<string, object>> _jsonObjects, BalanceCurveSO _newBalanceCurveData)
        {
            foreach (var jsonObject in _jsonObjects)
            {
                var balanceData = ParseJsonObj(jsonObject, _newBalanceCurveData);
                editorInstance.BalanceList.Add(balanceData);
            }

            var lastLevelData = _jsonObjects[^1];
            ParseLastLevelMaxValues(lastLevelData, _newBalanceCurveData);
        }

        private string GetFirstKeyFieldName(Dictionary<string, object> _jsonObj)
        {
            var obj = JObject.FromObject(_jsonObj);
            return obj.Properties().FirstOrDefault()?.Name;
        }

        private BalanceData ParseJsonObj(Dictionary<string, object> _jsonObj, BalanceCurveSO _balanceCurveData)
        {
            var balanceData = new BalanceData { balanceDataFields = new List<FieldData>() };
            var firstKeyFieldName = GetFirstKeyFieldName(_jsonObj);

            if (!string.IsNullOrEmpty(firstKeyFieldName))
            {
                foreach (var entry in JObject.FromObject(_jsonObj))
                {
                    ProcessEntry(entry, _balanceCurveData, balanceData);
                }
            }

            return balanceData;
        }

        private void ProcessEntry(KeyValuePair<string, JToken> _entry, BalanceCurveSO _balanceCurveData, BalanceData _balanceData)
        {
            var fieldData = _balanceCurveData.fields.FirstOrDefault(_f => _f.fieldName == _entry.Key);

            if (fieldData != null)
            {
                var fieldType = fieldData.fieldType;
                GetFieldType(_entry.Value, out var stringValue, out var decimalPoint);

                var newFieldData = new FieldData
                {
                    fieldName = _entry.Key,
                    fieldType = fieldType,
                    fieldValue = stringValue,
                    fieldDecimalPoint = decimalPoint
                };

                _balanceData.balanceDataFields.Add(newFieldData);
            }
        }

        private void ParseLastLevelMaxValues(Dictionary<string, object> _lastLevelData, BalanceCurveSO _newBalanceCurveData)
        {
            var firstKeyFieldName = GetFirstKeyFieldName(_lastLevelData);

            if (!string.IsNullOrEmpty(firstKeyFieldName))
            {
                foreach (var entry in _lastLevelData)
                {
                    if (!entry.Key.Equals(firstKeyFieldName))
                    {
                        var fieldName = entry.Key;
                        if (float.TryParse(entry.Value.ToString(), out var maxValue))
                        {
                            editorInstance.MaxValues.Add(new BalanceMaxValue(fieldName, maxValue));
                            _newBalanceCurveData.maxValues.Add(new BalanceMaxValue(fieldName, maxValue));
                        }
                    }
                }
            }
        }
        #endregion

        #region SaveJsonData
        /* Saves current state as JSON file. If _newFile parameter is true,
              prompts user to select location for saving a new file.*/
        public void SaveJsonData(bool _newFile = false)
        {
            try
            {
                var outputList = editorInstance.BalanceList.Select((_balanceData, _index) =>
                {
                    float.TryParse(_balanceData.balanceDataFields[0].fieldValue, out var firstFieldValue);
                    var firstFieldIntValue = Mathf.RoundToInt(firstFieldValue);

                    var levelDictionary = new Dictionary<string, object>
                    {
                        { editorInstance.BalanceCurveData.fields[0].fieldName, firstFieldIntValue }
                    };

                    var fieldDictionary = _balanceData.balanceDataFields.Skip(1).ToDictionary<FieldData, string, object>(_field => _field.fieldName, _field =>
                    {
                        if (float.TryParse(_field.fieldValue, out var fieldValue))
                        {
                            int decimalPoint = string.IsNullOrEmpty(_field.fieldDecimalPoint) ? 0 : int.Parse(_field.fieldDecimalPoint);
                            
                            if (_field.fieldType == FieldType.Int)
                            {
                                return (int)fieldValue;
                            }

                            if (_field.fieldType == FieldType.Float)
                            {
                                return Math.Round(fieldValue, decimalPoint);
                            }
                        }
                        Debug.LogError($"Failed to parse field value '{(_field.fieldValue)}' for '{(_field.fieldName)}'.");
                        return 0f;
                    });

                    foreach (var field in fieldDictionary) 
                    {
                        levelDictionary.Add(field.Key, field.Value);
                    }

                    return levelDictionary;
                }).ToList();

                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        IgnoreSerializableAttribute = false
                    },
                    Formatting = Formatting.Indented,
                    FloatParseHandling = FloatParseHandling.Decimal
                };
                var jsonString = JsonConvert.SerializeObject(outputList, jsonSettings);
                string fileNameWithoutExtension;
                string path;
                if (_newFile)
                {
                    path = EditorUtility.SaveFilePanel("Save Stats JSON File", editorInstance.DataPath, "BalanceData", "json");
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(editorInstance.AutoSaveFileName);
                    path = Path.Combine(editorInstance.AutoSavePath, fileNameWithoutExtension + ".json");
                    var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                    path = path.Substring(assetsIndex);
                }

                if (!string.IsNullOrEmpty(path))
                {
                    File.WriteAllText(path, jsonString);
                    editorInstance.ApplyAutoSaveJsonFileName(fileNameWithoutExtension);
                    Debug.Log("The save has been completed. Path : "+path);
                }
                
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred while saving data to JSON file: " + e.Message);
            }
        }
        #endregion
        
        #region LoadDataCsv
        public void LoadCsvData()
        {
            editorInstance.DataPath = GetCsvFilePath();
            if (string.IsNullOrEmpty(editorInstance.DataPath)) return;

            var csvLines = File.ReadAllLines(editorInstance.DataPath);

            if (csvLines.Length == 0) return;

            Dictionary<string, string> maxDecimalPoints = new Dictionary<string, string>();

            // You would need to implement a similar method for CSV data.
            // SetMaxDecimalPoints(csvLines, maxDecimalPoints); 

            string scriptableObjectPath = SaveNewDataToScriptableObject(true);

            if (string.IsNullOrEmpty(scriptableObjectPath)) return;

            editorInstance.ScriptableObjectPath = scriptableObjectPath;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(scriptableObjectPath);

            editorInstance.ApplyAutoSaveJsonFileName(fileNameWithoutExtension);

	        var headers = csvLines[0].Split(',');

	        var csvObjects= new List<Dictionary<string, object>>();

            for(int i=1; i<csvLines.Length; i++)
            {
                var values = csvLines[i].Split(',');
    
                var fieldDictionary = new Dictionary<string, object>();
    
                for(int j=0; j<headers.Length && j<values.Length; j++)
                {
                    if(float.TryParse(values[j], out float value))
                        fieldDictionary[headers[j]] = value;
                    else
                        Debug.LogError($"Failed to parse field value '{values[j]}' for '{headers[j]}'.");
                }
    
                csvObjects.Add(fieldDictionary);
            }
            
            SetCsvMaxDecimalPoints(csvObjects, maxDecimalPoints);

            var newBalanceCurveData = editorInstance.InitializeNewBalanceCurveData(scriptableObjectPath);
            ParseFirstCsvObjFields(csvObjects[0], newBalanceCurveData, maxDecimalPoints);
            ParseCsvObjects(csvObjects.Select(_x => _x.ToDictionary(_y => _y.Key.ToString(), _y => _y.Value)).ToList(), newBalanceCurveData);
   
            editorInstance.BalanceCurveData = newBalanceCurveData;

            InitializeBalanceFieldsList();
            UpdateValueOptions();
            UpdateBalance();

            AssetDatabase.Refresh(); 
        }
        
        private void SetCsvMaxDecimalPoints(List<Dictionary<string, object>> csvObjects, Dictionary<string, string> maxDecimalPoints)
        {
            foreach (var csvObj in csvObjects)
            {
                foreach (var kv in csvObj)
                {
                    if (kv.Value is float floatValue)
                    {
                        var decimalValue = Convert.ToDecimal(floatValue);
                        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
                        string decimalPlacesStr = decimalPlaces.ToString();

                        if (!maxDecimalPoints.ContainsKey(kv.Key) || int.Parse(decimalPlacesStr) > int.Parse(maxDecimalPoints[kv.Key]))
                        {
                            maxDecimalPoints[kv.Key] = decimalPlacesStr;
                        }
                    }
                }
            }
        }

        private void ParseFirstCsvObjFields(Dictionary<string, object> csvObj, BalanceCurveSO balanceCurveSO, Dictionary<string,string> maxDecimalPoint)
        {
            bool isFirstField = true;
            foreach(var kv in csvObj)
            {
                FieldType fieldType;
                string stringValue;
                string decimalPointString = maxDecimalPoint[kv.Key];

                if (kv.Value is float intValue && decimalPointString == "0")
                {
                    fieldType = FieldType.Int;
                    stringValue = Mathf.RoundToInt(intValue).ToString();
                    decimalPointString="0";
                }
                else if (kv.Value is float floatValue)
                {
                    fieldType = FieldType.Float;
                    stringValue=floatValue.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    Debug.LogError($"Unsupported type '{kv.Value.GetType()}' for field '{kv.Key}'.");
                    continue;
                }

                FieldData newFieldData=new FieldData
                {
                    fieldName = kv.Key,
                    fieldType = fieldType,
                    fieldValue = stringValue,
                    fieldDecimalPoint = decimalPointString
                };

                balanceCurveSO.fields.Add(newFieldData);

                if (!isFirstField)
                {
                    AnimationCurve curve = new AnimationCurve(CreateKeyframe(0f, 0f), CreateKeyframe(1f, 1f));
                    editorInstance.Curves[kv.Key] = curve;
                    
                    balanceCurveSO.animationCurves.Add(new BalanceAnimationCurveData
                    {
                        fieldName = kv.Key,
                        curve = curve
                    });
                }

                isFirstField = false;
            }
        }

        private void ParseCsvObjects(List<Dictionary<string, object>> csvObjects, BalanceCurveSO balanceCurveSO)
        {
            foreach (var csvObj in csvObjects)
            {
                var balanceData = ParseCsvObj(csvObj, balanceCurveSO);
                
                editorInstance.BalanceList.Add(balanceData);
            }

            var lastLevelData = csvObjects[^1];
            
            ParseLastLevelMaxValues(lastLevelData, balanceCurveSO);
        }

        private BalanceData ParseCsvObj(Dictionary<string, object> csvObj, BalanceCurveSO balanceCurveSO)
        {
            var balanceData = new BalanceData {balanceDataFields = new List<FieldData>()};

	        var firstKeyFieldName = csvObj.Keys.First();

	        foreach(var kv in csvObj)
	        {
		        var fieldInBalance=balanceCurveSO.fields.FirstOrDefault(_x=>_x.fieldName==kv.Key);

		        if(fieldInBalance != null)
		        {
			        FieldType fieldType;
			        string stringValue;

			        if(kv.Value is int intValue)
			        {
				        fieldType = FieldType.Int;
				        stringValue = intValue.ToString();
			        }
			        else if(kv.Value is float floatValue)
			        {
				        fieldType = FieldType.Float;
				        stringValue = floatValue.ToString(CultureInfo.InvariantCulture);
			        }
			        else continue;

		            FieldData newField = new FieldData
		            {
		                fieldName = kv.Key,
		                fieldType = fieldType,
		                fieldValue = stringValue,
		                fieldDecimalPoint = "0" // This should be replaced with the actual decimal point value.
		            };

		            balanceData.balanceDataFields.Add(newField);
	            }
            }

            return balanceData;
        }

        private string GetCsvFilePath()
        {
	        return EditorUtility.OpenFilePanel("Select CSV File", editorInstance.DataPath, "csv");
        }
        #endregion

        #region SaveCsvData
        public void SaveCsvData(bool _newFile = false)
        {
            try
            {
                var outputList = editorInstance.BalanceList.Select((_balanceData, _index) =>
                {
                    float.TryParse(_balanceData.balanceDataFields[0].fieldValue, out var firstFieldValue);
                    var firstFieldIntValue = Mathf.RoundToInt(firstFieldValue);

                    var levelDictionary = new Dictionary<string, object>
                    {
                        { editorInstance.BalanceCurveData.fields[0].fieldName, firstFieldIntValue }
                    };

                    var fieldDictionary = _balanceData.balanceDataFields.Skip(1).ToDictionary<FieldData, string, object>(_field => _field.fieldName, _field =>
                    {
                        if (_field.fieldType == FieldType.Int && int.TryParse(_field.fieldValue, out int intValue))
                        {
                            return intValue;
                        }

                        if (_field.fieldType == FieldType.Float && float.TryParse(_field.fieldValue, out float floatValue))
                        {
                            return Math.Round(floatValue,
                                string.IsNullOrEmpty(_field.fieldDecimalPoint) ? 0 : int.Parse(_field.fieldDecimalPoint));
                        }

                        Debug.LogError($"Failed to parse field value '{_field.fieldValue}' for '{_field.fieldName}'.");
                        
                        return 0f;
                        
                     });

                     foreach (var field in fieldDictionary)
                     {
                         levelDictionary.Add(field.Key.ToString(), field.Value);
                     }

                     return levelDictionary;

                 }).ToList();

                 // Prepare CSV data
                 StringBuilder csvContentBuilder = new StringBuilder();
                 
                 // Write header row
                 List<string> headers = outputList[0].Keys.ToList();
                 csvContentBuilder.AppendLine(string.Join(",", headers));
                 
                 // Write data rows
                 foreach(var item in outputList)
                 { 
                      List<string> rowValues = item.Values.Select(v => v.ToString()).ToList();
                      csvContentBuilder.AppendLine(string.Join(",", rowValues));
                 }

                string fileNameWithoutExtension;
                string path;

                if (_newFile)
                {
                   path= EditorUtility.SaveFilePanel("Save Stats CSV File", editorInstance.DataPath,"BalanceData", "csv");
                   fileNameWithoutExtension= Path.GetFileNameWithoutExtension(path);
                }
                else
               { 
                   fileNameWithoutExtension= Path.GetFileNameWithoutExtension(editorInstance.AutoSaveFileName);
                   path= Path.Combine(editorInstance.AutoSavePath , fileNameWithoutExtension + ".csv");
                   var assetsIndex= path.IndexOf("Assets", StringComparison.Ordinal); 
                   path= path.Substring(assetsIndex); 
               } 

               if (!string.IsNullOrEmpty(path))
               { 
                  File.WriteAllText(path,csvContentBuilder.ToString());
                  editorInstance.ApplyAutoSaveJsonFileName(fileNameWithoutExtension); 
                  Debug.Log("The save has been completed. Path : "+path); 
              } 

              AssetDatabase.Refresh(); 

            } catch(Exception e){ Debug.LogError("Error occurred while saving data to CSV file: " + e.Message);} 

        }
        #endregion
        
        #region UpdateValueOptions
        public void UpdateValueOptions()
        {
            if (editorInstance.BalanceCurveData != null)
            {
                var fields = editorInstance.BalanceCurveData.fields;

                editorInstance.ValueOptions = new string[fields.Count + 1];
                editorInstance.ValueOptions[0] = "Any Fields";

                for (var i = 1; i < fields.Count; i++)
                {
                    editorInstance.ValueOptions[i] = fields[i].fieldName;
                }
            }
        }
        #endregion
        
        #region UpdateBalance
        public void UpdateBalance(string _curveName = null)
        {
            if (editorInstance.BalanceCurveData == null || editorInstance.BalanceList == null ||
                editorInstance.BalanceList.Count == 0 || editorInstance.Curves == null)
            {
                Debug.LogError("UpdateBalance return");
                return;
            }

            EnsureBalanceHasEnoughLevels();

            editorInstance.InitialValues.Clear();

            int fieldIndex = 0;

            foreach (var field in editorInstance.BalanceList[0].balanceDataFields)
            {
                if (fieldIndex == 0)
                {
                    fieldIndex++;
                    continue;
                }

                if (field.fieldType == FieldType.Int)
                {
                    editorInstance.InitialValues[field.fieldName] = int.Parse(field.fieldValue);
                }
                else if (field.fieldType == FieldType.Float)
                {
                    editorInstance.InitialValues[field.fieldName] = float.Parse(field.fieldValue);
                }
            }

            for (var i = 1; i < editorInstance.BalanceList.Count; i++)
            {
                fieldIndex = 0;

                foreach (var field in editorInstance.BalanceCurveData.fields)
                {
                    if (fieldIndex == 0)
                    {
                        fieldIndex++;
                        continue;
                    }

                    bool shouldUpdate = _curveName == null || field.fieldName == _curveName;
                    if (shouldUpdate)
                    {
                        var currentData = editorInstance.BalanceList[i];
                        var currentFieldData = currentData.balanceDataFields.FirstOrDefault(_f => _f.fieldName == field.fieldName);

                        if (currentFieldData != null && editorInstance.Curves.TryGetValue(field.fieldName, out var curve))
                        {
                            var fieldVal = float.Parse(field.fieldValue);
                            var maxValue = fieldVal;
                            foreach (var kvp in editorInstance.MaxValues)
                            {
                                if (kvp.fieldName == field.fieldName)
                                {
                                    maxValue = kvp.maxValue;
                                    break;
                                }
                            }
                            var adjustedValue = Mathf.Lerp(fieldVal, maxValue, curve.Evaluate(i / (Mathf.Max(1f, editorInstance.BalanceList.Count - 1))));
                            if (field.fieldType == FieldType.Int)
                            {
                                currentFieldData.fieldValue = Mathf.RoundToInt(adjustedValue).ToString();
                            }
                            else if (field.fieldType == FieldType.Float)
                            {
                                currentFieldData.fieldValue = adjustedValue.ToString("F"+field.fieldDecimalPoint);
                            }
                        }
                    }

                    fieldIndex++;
                }
            }


            if (editorInstance.AutoSaveJson)
            {
                SaveJsonData();
            }
        }
        #endregion

        #region EnsureBalanceHasEnoughLevels
        
        private void EnsureBalanceHasEnoughLevels()
        {
            if (editorInstance.BalanceList.Count > editorInstance.BalanceSliderValue)
            {
                editorInstance.BalanceList.RemoveRange(editorInstance.BalanceSliderValue, editorInstance.BalanceList.Count - editorInstance.BalanceSliderValue);
            }
            else if (editorInstance.BalanceList.Count < editorInstance.BalanceSliderValue)
            {
                for (var i = editorInstance.BalanceList.Count; i < editorInstance.BalanceSliderValue; i++)
                {
                    var newBalance = new BalanceData
                    {
                        balanceDataFields = new List<FieldData>()
                    };

                    foreach (var fieldData in editorInstance.BalanceCurveData.fields)
                    {
                        var newFieldData = new FieldData
                        {
                            fieldName = fieldData.fieldName,
                            fieldType = fieldData.fieldType,
                            fieldValue = fieldData.fieldValue,
                            fieldDecimalPoint = fieldData.fieldDecimalPoint
                        };
                        newBalance.balanceDataFields.Add(newFieldData);
                    }

                    editorInstance.BalanceList.Add(newBalance);
                }
            }
        }
        #endregion
        
        #region OnMaxValueChanged

        public void OnMaxValueChanged(int _value)
        {
            editorInstance.MaxBalanceLevelValue = _value;

            editorInstance.BalanceCurveData.maxBalanceLevelValue = editorInstance.MaxBalanceLevelValue;
        }
        #endregion
        
        #region OnMaxLevelSliderValueChanged
        
        public void OnMaxLevelSliderValueChanged(int _value)
        {
            editorInstance.BalanceSliderValue = _value;

            editorInstance.BalanceCurveData.balanceSliderValue = editorInstance.BalanceSliderValue;
            UpdateBalance();
        }
        #endregion
        
        
        
        #region ResetToInitialValues
        /* Resets all settings back to their initial states - clears lists, resets flags,
                recreates default key field and another one non-key field.*/
        public void ResetToInitialValues()
        {
            editorInstance.BalanceList.Clear();
            editorInstance.MaxValues.Clear();
            editorInstance.Curves.Clear();
            editorInstance.NewFieldNameMapping.Clear();
            editorInstance.FieldNameMapping.Clear();
            editorInstance.InitialValues.Clear();
            editorInstance.BalanceCurveData.fields.Clear();
            editorInstance.BalanceCurveData.animationCurves.Clear();
            editorInstance.BalanceCurveData.maxValues.Clear();
            editorInstance.NewFieldName = string.Empty;
            editorInstance.NewFieldCount = 0;
            editorInstance.AllFieldsFolded = true;
            editorInstance.IsKeyFieldValue = true;
            editorInstance.AutoSaveJson = false;
            editorInstance.StartKeyValue = 1;
            editorInstance.BalanceCurveData.fields = new();
            editorInstance.BalanceCurveData.animationCurves = new();
            var newFieldData = CreateNewField();
            editorInstance.BalanceCurveData.fields.Add(newFieldData);
            editorInstance.IsKeyFieldValue = false;
            newFieldData = CreateNewField();
            editorInstance.BalanceCurveData.fields.Add(newFieldData);
            editorInstance.BalanceCurveData.maxBalanceLevelValue = editorInstance.MaxBalanceLevelValue;
            editorInstance.BalanceCurveData.balanceSliderValue = editorInstance.BalanceSliderValue;
            editorInstance.CurvesLoaded = false;
            Initialize();
            editorInstance.IsKeyFieldValue = false;
        }
        #endregion
    }
}