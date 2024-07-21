using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


namespace Variables.Editor
{
    public class VariableEditorWindow : EditorWindow
    {
        private enum VariableType
        {
            INT,
            FLOAT,
            BOOL
        }
        private VariableType selectedType = VariableType.INT; 

        private IOUtilities IO = new IOUtilities();

        private VariablesDatabase _varDatabase;
        private Vector2 scrollPos;

        private List<IntegerVariableData> integersToDelete = new();
        private List<FloatVariableData> floatsToDelete = new();
        private List<BooleanVariableData> boolsToDelete = new();

        [MenuItem("DialogueSystem/Variables Editor")]
        public static VariableEditorWindow ShowWindow()
        {
           return GetWindow<VariableEditorWindow>("Variables Editor");
        }

        public static VariableEditorWindow OpenWindowInGraphView<T>() where T : EditorWindow
        {
            return GetWindow<VariableEditorWindow>("Variables Editor", typeof(T));
        }

        public static void CloseWindow(VariableEditorWindow window)
        {
            window.Close();
        }
        private void OnEnable()
        {
            LoadDatabase();
        }
        private void LoadDatabase()
        {
            _varDatabase = IO.LoadAsset<VariablesDatabase>(VariableSystem.GetDatabasePath());
        }
        private void OnGUI()
        {
            integersToDelete.Clear();
            floatsToDelete.Clear();
            boolsToDelete.Clear();

            GUILayout.Space(30);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("INT VARIABLES", GUILayout.Width(150), GUILayout.Height(50)))
            {
                selectedType = VariableType.INT;
            }
            if (GUILayout.Button("FLOAT VARIABLES", GUILayout.Width(150), GUILayout.Height(50)))
            {
                selectedType = VariableType.FLOAT;
            }
            if (GUILayout.Button("BOOL VARIABLES", GUILayout.Width(150), GUILayout.Height(50)))
            {
                selectedType = VariableType.BOOL;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("ADD VARIABLE", GUILayout.Width(150), GUILayout.Height(50)))
            {
                AddVarible();
            }
            if (GUILayout.Button("COMPILE VARIABLES", GUILayout.Width(150), GUILayout.Height(50)))
            {
                VariableCompiler.Compile();
            }
            EditorGUILayout.EndHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            DrawAllVariables();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            DeleteVariables();
        }

        private void DrawAllVariables()
        {
            switch (selectedType) 
            {
                case VariableType.INT:
                    var intVariables = _varDatabase.GetIntegers();
                    foreach (var var in intVariables)
                    {
                        DrawIntVariable(var);
                    }
                    break;
                case VariableType.FLOAT:
                    var floatVariables = _varDatabase.GetDecimals();
                    foreach (var var in floatVariables)
                    {
                        DrawFloatVariable(var);
                    }
                    break;
                case VariableType.BOOL:
                    var boolVariables = _varDatabase.GetBooleans();
                    foreach (var var in boolVariables)
                    {
                        DrawBoolVariable(var);
                    }
                    break;
            }
        }
        private void DrawIntVariable(IntegerVariableData var)
        {
            GUILayout.BeginHorizontal(GUILayout.MaxWidth(400));
            GUILayout.Label("Name:");
            var nameValue = EditorGUILayout.TextField("", var.Name, GUILayout.MaxWidth(150));
            if (nameValue != var.Name) var.Name = nameValue;
            GUILayout.Label("Valore:");
            var intValue = EditorGUILayout.IntField("", var.Value, GUILayout.MaxWidth(150));
            if (intValue != var.Value) var.Value = intValue;
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                integersToDelete.Add(var);
            }
            GUILayout.EndHorizontal();
        }
        private void DrawFloatVariable(FloatVariableData var)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");
            var nameValue = EditorGUILayout.TextField("", var.Name, GUILayout.MaxWidth(150));
            if(nameValue != var.Name) var.Name = nameValue;
            GUILayout.Label("Valore:");
            var floatValue = EditorGUILayout.FloatField("", var.Value, GUILayout.MaxWidth(150));
            if (floatValue != var.Value) var.Value = floatValue;
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                floatsToDelete.Add(var);    
            }
            GUILayout.EndHorizontal();
        }
        private void DrawBoolVariable(BooleanVariableData var)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");
            var nameValue = EditorGUILayout.TextField("", var.Name, GUILayout.MaxWidth(150));
            if (nameValue != var.Name) var.Name = nameValue;
            GUILayout.Label("Valore:");
            var boolValue = EditorGUILayout.Toggle("", var.Value, GUILayout.MaxWidth(150));
            if (boolValue != var.Value) var.Value = boolValue;
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                boolsToDelete.Add(var);
            }
            GUILayout.EndHorizontal();
        }
        private void AddVarible()
        {
            switch(selectedType)
            {
                case VariableType.INT:
                    _varDatabase.AddIntegerVariable();
                break;
                case VariableType.FLOAT:
                    _varDatabase.AddFloatVariable();
                    break;
                case VariableType.BOOL:
                    _varDatabase.AddBooleanVariable();
                    break;
            }
        }

        public void DeleteVariables()
        {
            foreach(var var in integersToDelete) _varDatabase.RemoveVariable<IntegerVariableData>(var.Id);
            foreach (var var in boolsToDelete) _varDatabase.RemoveVariable<BooleanVariableData>(var.Id);
            foreach (var var in floatsToDelete) _varDatabase.RemoveVariable<FloatVariableData>(var.Id);
        }
    }
}
