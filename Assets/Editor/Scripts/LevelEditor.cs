using UnityEditor;
using UnityEngine;

namespace GameDevLabirinth
{
    public class LevelEditor : EditorWindow
    {
        private Transform _parent;
        private EditorData _data;
        private int _index;
        private bool _isEditing;
        private GameLevel _gameLevel;
        private SceneEditor _sceneEditor;

        [MenuItem("Window/Level Editor")]
        public static void Init()
        {
            GetWindow<LevelEditor>("Level Editor").minSize = new Vector2(350, 500);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            DrawParentField();
            DrawClearButton();

            if (_data == null)
            {
                DrawLoadDataButton();
            }
            else
            {
                EditorGUILayout.Space(20);
                DrawBlockSelector();
                EditorGUILayout.Space(30);
                DrawEditToggle();
                EditorGUILayout.Space(30);
                DrawLevelSaveLoad();
            }
        }

        private void DrawParentField()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Parent Object:", GUILayout.Width(100));
            _parent = (Transform)EditorGUILayout.ObjectField(_parent, typeof(Transform), true);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawClearButton()
        {
            if (GUILayout.Button("Clear All Saved Data", GUILayout.Height(30)))
            {
                new LevelsData().Clear();
                new LevelIndex().Clear();
                Debug.Log("All saved data cleared");
            }
        }

        private void DrawLoadDataButton()
        {
            if (GUILayout.Button("Load Editor Data", GUILayout.Height(40)))
            {
                _data = AssetDatabase.LoadAssetAtPath<EditorData>("Assets/Editor/Data/EditorData.asset");
                _sceneEditor = CreateInstance<SceneEditor>();
                _sceneEditor.SetLevelEditor(this, _parent);
            }
        }

        private void DrawBlockSelector()
        {
            EditorGUILayout.LabelField("CURRENT BLOCK", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            // Объявляем переменную в начале метода
            EditorBlockData currentBlockData = _data.BlockDatas[_index];

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("◄", GUILayout.Width(40), GUILayout.Height(60)))
                        _index = (_index - 1 + _data.BlockDatas.Count) % _data.BlockDatas.Count;

                    GUI.color = currentBlockData.BlockData is ColoredBlock cb ? cb.BaseColor : Color.white;
                    Rect textureRect = GUILayoutUtility.GetRect(80, 80);
                    GUI.DrawTexture(textureRect, currentBlockData.Texture2D, ScaleMode.ScaleToFit);
                    GUI.color = Color.white;

                    if (GUILayout.Button("►", GUILayout.Width(40), GUILayout.Height(60)))
                        _index = (_index + 1) % _data.BlockDatas.Count;

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                // Используем currentBlockData вместо blockData
                EditorGUILayout.LabelField(currentBlockData.BlockData.name, new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 12,
                    margin = new RectOffset(0, 0, 5, 0)
                });
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEditToggle()
        {
            GUIStyle editButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                fixedHeight = 40
            };

            GUI.color = _isEditing ? Color.red : new Color(0.2f, 0.8f, 0.2f);
            if (GUILayout.Button(_isEditing ? "STOP EDITING" : "START EDITING", editButtonStyle))
            {
                _isEditing = !_isEditing;
                SceneView.duringSceneGui -= _sceneEditor.OnSceneGUI;
                if (_isEditing) SceneView.duringSceneGui += _sceneEditor.OnSceneGUI;
            }
            GUI.color = Color.white;
        }

        private void DrawLevelSaveLoad()
        {
            EditorGUILayout.LabelField("LEVEL MANAGEMENT", EditorStyles.boldLabel);
            _gameLevel = (GameLevel)EditorGUILayout.ObjectField("Level Asset", _gameLevel, typeof(GameLevel), false);

            if (_gameLevel != null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("SAVE", GUILayout.Height(30)))
                    {
                        new SaveLevel().Save(_gameLevel);
                        EditorUtility.SetDirty(_gameLevel);
                        Debug.Log("Level saved successfully");
                    }

                    if (GUILayout.Button("LOAD", GUILayout.Height(30)))
                    {
                        FindObjectOfType<ClearLevel>().Clear();
                        new BlocksGenerator().Generate(_gameLevel, _parent);
                        Debug.Log("Level loaded");
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        public BlockData GetBlock() => _data.BlockDatas[_index].BlockData;

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= _sceneEditor.OnSceneGUI;
        }
    }
}