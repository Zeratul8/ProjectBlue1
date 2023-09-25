using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace BalanceEditor
{
    public class CustomCurveEditorWindow : EditorWindow
    {
        private Dictionary<string, AnimationCurve> curves;
        private Dictionary<string, string> fieldNameMapping;
        private Dictionary<string, bool> curveChangedFlags = new ();
        private EditorWindow previousWindow;
        private Vector2 scrollPosition;
        private IBalanceEditor editorInstance;
        public bool unsavedChange;

        #region SetEditorInstance
        public void SetEditorInstance(IBalanceEditor _balanceEditor)
        {
            if (editorInstance != null) return;
            editorInstance = _balanceEditor;
        }
        #endregion

        public void ShowWindow(Dictionary<string, AnimationCurve> _curves, Dictionary<string, string> _fieldNameMapping, EditorWindow _previousWindow, IBalanceEditor _balanceEditor)
        {
            Initialize(_curves, _fieldNameMapping, _previousWindow, _balanceEditor);
        }

        public void Initialize(Dictionary<string, AnimationCurve> _curves, Dictionary<string, string> _fieldNameMapping, EditorWindow _previousWindow, IBalanceEditor _balanceEditor)
        {
            curves = _curves;
            fieldNameMapping = _fieldNameMapping;
            previousWindow = _previousWindow;
            editorInstance = _balanceEditor;
            editorInstance.IsOpenCustomCurveEditorWindow = true;
            Repaint();
        }
        
        private void OnDisable()
        {
            if (unsavedChange)
            {
                if (EditorUtility.DisplayDialog("Unsaved Changes",
                        "You have unsaved changes. Would you like to save them before closing the Balance Editor?",
                        "Save", "Cancel"))
                {
                    editorInstance.SaveDataToScriptableObject();
                    unsavedChange = false;
                    editorInstance.UnsavedChange = false;
                    editorInstance.IsOpenCustomCurveEditorWindow = false;
                }
            }
        } 
        private void OnGUI()
        {
            if (curves != null)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField("Curve Editor", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                var modifiedCurves = new Dictionary<string, AnimationCurve>(curves);
                var curveNames = new List<string>(curves.Keys);

                foreach (var curveName in curveNames)
                {
                    var nameToDisplay = fieldNameMapping.TryGetValue(curveName, out var newName) ? newName : curveName;

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    var curve = EditorGUILayout.CurveField(nameToDisplay, modifiedCurves[curveName]);
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        modifiedCurves[curveName] = curve;
                        curves[curveName] = modifiedCurves[curveName];
                        editorInstance.Curves[curveName] = modifiedCurves[curveName];
                        editorInstance.UpdateBalance(curveName);
                        editorInstance.UnsavedChange = true;
                        unsavedChange = true;
                        curveChangedFlags[curveName] = true;
                    }

                    if (curveChangedFlags.ContainsKey(curveName) && curveChangedFlags[curveName])
                    {
                        GUIStyle redBoldLabel = new GUIStyle(EditorStyles.boldLabel)
                        {
                            normal =
                            {
                                textColor = Color.red
                            }
                        };
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        GUILayout.Label("Modified!", redBoldLabel);
                        if (GUILayout.Button("Save", editorInstance.ButtonStyle))
                        {
                            editorInstance.SaveCurve();
                            curveChangedFlags[curveName] = false;
                            editorInstance.UnsavedChange = false;
                            unsavedChange = false;
                            for (var i = 1; i < editorInstance.BalanceCurveData.animationCurves.Count; i++)
                            {
                                var animationCurveData = editorInstance.BalanceCurveData.animationCurves[i];
                                var fieldName = animationCurveData.fieldName;
                                editorInstance.Curves[fieldName] = new AnimationCurve(animationCurveData.curve.keys);
                            }
                            editorInstance.UpdateBalance();
                        }
                        if (GUILayout.Button("Revert"))
                        {
                            curves[curveName] = modifiedCurves[curveName];
                            curveChangedFlags[curveName] = false;
                            editorInstance.UnsavedChange = false;
                            unsavedChange = false;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
        }
        
        public void CurveEditorUpdate()
        {
            if (previousWindow != null)
            {
                var prev = editorInstance;
                if (prev != null)
                {
                    curves = prev.Curves;
                    fieldNameMapping = prev.FieldNameMapping;
                }
            }
        }

        private void OnDestroy()
        {
            if (previousWindow)
            {
                previousWindow.Repaint();
            }
        }
    }
}