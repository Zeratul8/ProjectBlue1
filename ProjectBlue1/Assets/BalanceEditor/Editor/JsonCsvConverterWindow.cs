using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit
{
    public class JsonCsvConverterWindow : EditorWindow
    {
        private GUIStyle buttonStyle;
        private Texture2D normalBackground;
        private Texture2D hoverBackground;
        private Object jsonFileObject;
        private Object csvFileObject;
        private string jsonFilePath = "Assets/BalanceEditor/Data/";
        private string csvFilePath = "Assets/BalanceEditor/Data/";
        private string jsonOutputPath = "Assets/BalanceEditor/Data/";
        private string csvOutputPath = "Assets/BalanceEditor/Data/";

        [MenuItem("Custom Editor/JsonCsv Converter")]
        public static void ShowWindow()
        {
            GetWindow<JsonCsvConverterWindow>("JsonCsvConverter");
        }

        private void OnEnable()
        {
            string path = "Assets/BalanceEditor/Data/";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets/BalanceEditor", "Data");
            }
            normalBackground = MakeTexture(Color.gray);
            hoverBackground = MakeTexture(Color.black);
        }
        
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
        
        bool IsJsonFile(Object _fileObject) 
        {
            string filePath = AssetDatabase.GetAssetPath(_fileObject);
            return Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase);
        }

        bool IsCsvFile(Object _fileObject) 
        {
            string filePath = AssetDatabase.GetAssetPath(_fileObject);
            return Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase);
        }

        private void OnGUI()
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
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
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("JSON to CSV", EditorStyles.boldLabel);
            
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            Object newJsonFileObject = EditorGUILayout.ObjectField("Json File Object", jsonFileObject, typeof(Object), false);
            if (newJsonFileObject != jsonFileObject) 
            {
                if (IsJsonFile(newJsonFileObject)) 
                {
                    jsonFileObject = newJsonFileObject;
                    jsonFilePath = AssetDatabase.GetAssetPath(jsonFileObject);
                } 
                else 
                {
                    EditorUtility.DisplayDialog("Error", "Please select a JSON file.", "OK");
                    newJsonFileObject = jsonFileObject;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            GUILayout.Label("JSON File Path: " + jsonFilePath);
            
            if (GUILayout.Button("Browse", buttonStyle,GUILayout.Width(130)))
            {
                var path = EditorUtility.OpenFilePanel("Select JSON file", jsonFilePath, "json");
                if (path == null) return;
                var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                jsonFilePath = path.Substring(assetsIndex);
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            GUILayout.Label("CSV Output Path: " + csvOutputPath);
            
            if (GUILayout.Button("Browse", buttonStyle,GUILayout.Width(130)))
            {
                var path = EditorUtility.SaveFilePanel("Save CSV file", csvOutputPath, "output.csv", "csv");
                if (path == null) return;
                var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                csvOutputPath = path.Substring(assetsIndex);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button("Convert JSON to CSV", buttonStyle))
            {
                if (!string.IsNullOrEmpty(jsonFilePath) && !string.IsNullOrEmpty(csvOutputPath))
                {
                    ConvertJsonToCsv(jsonFilePath, csvOutputPath);
                }
                else if (string.IsNullOrEmpty(jsonFilePath))
                {
                    Debug.LogError("Please select a JSON file.");
                }
                else if (string.IsNullOrEmpty(csvOutputPath))
                {
                    Debug.LogError("Please select a CSV Output Path.");
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            /////////////////////////////////////////
            
            GUILayout.Space(20);
            
            /////////////////////////////////////////

            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("CSV to JSON", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            Object newCsvFileObject = EditorGUILayout.ObjectField("CSV File Object", csvFileObject, typeof(Object), false);
            if (newCsvFileObject != csvFileObject) {
                
                if (IsCsvFile(newCsvFileObject)) 
                {
                    csvFileObject = newCsvFileObject;
                    csvFilePath = AssetDatabase.GetAssetPath(csvFileObject);
                } 
                else 
                {
                    EditorUtility.DisplayDialog("Error", "Please select a CSV file.", "OK");
                    newCsvFileObject = csvFileObject;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            GUILayout.Label("CSV File Path: " + csvFilePath);
            if (GUILayout.Button("Browse", buttonStyle,GUILayout.Width(130)))
            {
                var path = EditorUtility.OpenFilePanel("Select CSV file", csvFilePath, "csv");
                if (path == null) return;
                var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                csvFilePath = path.Substring(assetsIndex);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            GUILayout.Label("JSON Output Path: " + jsonOutputPath);
            if (GUILayout.Button("Browse", buttonStyle,GUILayout.Width(130)))
            {
                var path = EditorUtility.SaveFilePanel("Save JSON file", jsonOutputPath, "output.json", "json");
                if (path == null) return;
                var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal);
                jsonOutputPath = path.Substring(assetsIndex);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button("Convert CSV to JSON", buttonStyle))
            {
                if (!string.IsNullOrEmpty(csvFilePath) && !string.IsNullOrEmpty(jsonOutputPath))
                {
                    ConvertCsvToJson(csvFilePath, jsonOutputPath);
                }
                else if (string.IsNullOrEmpty(csvFilePath))
                {
                    Debug.LogError("Please select a CSV file.");
                }
                else if (string.IsNullOrEmpty(jsonOutputPath))
                {
                    Debug.LogError("Please select a JSON Output Path.");
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        public void ConvertJsonToCsv(string _jsonFile, string _outputFile)
        {
            // JSON file to read
            string json = File.ReadAllText(_jsonFile);

            // Convert JSON data to type List<Dictionary<string, object>>
            var jsonObject = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

            // Save as CSV file
            using (var writer = new StreamWriter(_outputFile))
            {
                // Write headers
                var headers = jsonObject.SelectMany(_dict => _dict.Keys).Distinct();
                var enumerable = headers.ToList();

                writer.WriteLine(string.Join(",", enumerable));

                // Write records
                foreach (var record in jsonObject)
                {
                    writer.WriteLine(string.Join(",",
                        enumerable.Select(_header =>
                            record.TryGetValue(_header, out var value) ? value.ToString() : string.Empty)));
                }
            }
            AssetDatabase.Refresh();
        }

        public void ConvertCsvToJson(string _csvFile, string _outputFile)
        {
           // CSV file to read
           var lines = File.ReadAllLines(_csvFile);

           // Read CSV data and convert it to type List<Dictionary<string, object>>
           var headers = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
           
           var records = new List<Dictionary<string, object>>();

           for (int i = 1; i < lines.Length; i++)
           {
               var line = lines[i].Split(',');
               var record = new Dictionary<string, object>();

               for (int j = 0; j < headers.Length; j++)
               {
                   // Handling integer and decimal values
                   if (int.TryParse(line[j], out int parsedIntValue))
                   {
                       record[headers[j]] = parsedIntValue;
                   }
                   else if (double.TryParse(line[j], NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedDoubleValue))
                   {
                       record[headers[j]] = parsedDoubleValue;
                   }
                   else
                   {
                       record[headers[j]] = line[j];
                   }
               }

               records.Add(record);
           }

           // Write converted data in JSON format and save it into a file.
           string json = JsonConvert.SerializeObject(records, Formatting.Indented);
           File.WriteAllText(_outputFile, json);
           AssetDatabase.Refresh();
        }
    }
}