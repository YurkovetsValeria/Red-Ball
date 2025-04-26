using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameDevLabirinth
{
    public class LevelEditor : EditorWindow
    {
        private Transform _parent;
        private EditorData _data;
        private int _index;
        private bool _isEnabledEdit;
        private GameLevel _gameLevel;
        private SceneEditor _sceneEditor;
        public bool IsEnabledEdit => _isEnabledEdit;

        [MenuItem("Window/Level Editor")]
        public static void Init()
        {
            LevelEditor levelEditor = GetWindow<LevelEditor>("Level Editor");
            levelEditor.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            _parent = (Transform)EditorGUILayout.ObjectField("Parent", _parent, typeof(Transform), true);
            EditorGUILayout.Space(30);

            if (_data == null)
            {
                if (GUILayout.Button("Load data"))
                {
                    _data = (EditorData)AssetDatabase.LoadAssetAtPath("Assets/Editor/Data/EditorData.asset", typeof(EditorData));
                    _sceneEditor = CreateInstance<SceneEditor>();
                    _sceneEditor.SetLevelEditor(this, _parent);
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Block Preview", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("<", GUILayout.Width(50), GUILayout.Height(50)))
                {
                    _index--;
                    if (_index < 0)
                        _index = _data.BlockDatas.Count - 1;
                }

                GUI.color = Color.white;

                if (_index >= 0 && _index < _data.BlockDatas.Count)
                {
                    if (_data.BlockDatas[_index].BlockData is ColoredBlock coloredBlock)
                    {
                        GUI.color = coloredBlock.BaseColor;
                        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1.0f);
                    }

                    GUILayout.Label(_data.BlockDatas[_index].Texture2D);
                }

                GUI.color = Color.white;

                if (GUILayout.Button(">", GUILayout.Width(50), GUILayout.Height(50)))
                {
                    _index++;
                    if (_index >= _data.BlockDatas.Count)
                        _index = 0;
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(30);

                GUI.color = _isEnabledEdit ? Color.red : Color.white;
                if (GUILayout.Button("Create blocks"))
                {
                    _isEnabledEdit = !_isEnabledEdit;

                    if (_isEnabledEdit)
                    {
                        SceneView.duringSceneGui += _sceneEditor.OnSceneGUI;
                    }
                    else
                    {
                        SceneView.duringSceneGui -= _sceneEditor.OnSceneGUI;
                    }
                }
                GUI.color = Color.white;
                GUILayout.Space(30);

                _gameLevel = EditorGUILayout.ObjectField(_gameLevel, typeof(GameLevel), false) as GameLevel;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save Level"))
                {
                    SaveLevel saveLevel = new SaveLevel();
                    _gameLevel.Blocks = saveLevel.GetBlocks();
                    EditorUtility.SetDirty(_gameLevel);
                    Debug.Log("Level Saved");
                }

                if (GUILayout.Button("Load Level"))
                {
                    GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
                    foreach (var item in allBlocks)
                    {
                        DestroyImmediate(item.gameObject);
                    }

                    BlocksGenerator generator = new BlocksGenerator();
                    generator.Generate(_gameLevel, _parent);
                }
                GUILayout.EndHorizontal();
            }
        }

        public BlockData GetBlock()
        {
            return (_index >= 0 && _index < _data.BlockDatas.Count) ? _data.BlockDatas[_index].BlockData : null;
        }
    }
}
