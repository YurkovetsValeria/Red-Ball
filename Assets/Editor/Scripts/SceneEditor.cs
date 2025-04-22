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
            if (current.type == EventType.MouseDown)
            {
                Vector3 point = sceneView.camera.ScreenToWorldPoint(new Vector3(current.mousePosition.x,
                    sceneView.camera.pixelHeight - current.mousePosition.y,
                    sceneView.camera.nearClipPlane));
                Vector3 position = ValidatePosition(point); // Заменено CheckPosition

                if (position != Vector3.zero)
                {
                    if (IsEmpty(position))
                    {
                        GameObject game = PrefabUtility.InstantiatePrefab(_levelEditor.GetBlock(), _parent) as GameObject; // Исправлено Prefub
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

            if (current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }
        }

        private bool IsEmpty(Vector3 position)
        {
            Collider2D collider = Physics2D.OverlapCircle(position, 0.01f);
            return collider == null;
        }

        private Vector3 ValidatePosition(Vector3 point)
        {
            // Временно возвращаем point, можно заменить на реальную проверку
            return point;
        }
    }
}
