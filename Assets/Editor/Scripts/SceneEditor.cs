using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameDevLabirinth
{
    public class SceneEditor : EditorWindow
    {
        private readonly EditorGrid _grid = new EditorGrid();
        private LevelEditor _levelEditor;
        private Transform _parent;

        public void SetLevelEditor(LevelEditor levelEditor, Transform parent)
        {
            _parent = parent;
            _levelEditor = levelEditor;
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            Event current = Event.current;

            if (_levelEditor.IsEnabledEdit) 
            {
                if (current.type == EventType.MouseDown && current.button == 0) 
                {
                    Vector3 point = sceneView.camera.ScreenToWorldPoint(new Vector3(current.mousePosition.x,
                        sceneView.camera.pixelHeight - current.mousePosition.y,
                        sceneView.camera.nearClipPlane));
                    point.z = 0; 

                    Vector3 gridPosition = GetGridPosition(point);

                    CreateBlock(gridPosition);
                    current.Use(); 
                }
            }

            if (current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }
        }

        private Vector3 GetGridPosition(Vector3 position)
        {
            float gridSize = 1f; 

            float x = Mathf.Round(position.x / gridSize) * gridSize;
            float y = Mathf.Round(position.y / gridSize) * gridSize;

            return new Vector3(x, y, 0); 
        }

        private void CreateBlock(Vector3 position)
        {
            var blockToCreate = _levelEditor.GetBlock().Prefub;
            GameObject game = PrefabUtility.InstantiatePrefab(blockToCreate, _parent) as GameObject;
            game.transform.position = position;

            if (game.TryGetComponent(out OtherBlock other))
            {
                other.BlockData = _levelEditor.GetBlock();
            }

            if (game.TryGetComponent(out Block block))
            {
                block.BlockData = _levelEditor.GetBlock();
                block.SetData(_levelEditor.GetBlock() as ColoredBlock);
            }
        }
    }
}