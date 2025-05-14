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
            _levelEditor = levelEditor;
            _parent = parent;
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            _grid.DrawGrid();

            Event current = Event.current;
            if (current.type == EventType.MouseDown && current.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
                Vector3 mouseWorldPos = ray.origin;
                mouseWorldPos.z = 0;

                Vector3 gridPosition = _grid.CheckPosition(mouseWorldPos);
                if (gridPosition != Vector3.zero && IsEmpty(gridPosition))
                {
                    CreateBlock(gridPosition);
                    current.Use();
                }
            }
        }

        private bool IsEmpty(Vector3 position)
        {
            Collider2D collider = Physics2D.OverlapCircle(position, 0.1f);
            return collider == null || collider.transform == _parent;
        }

        private void CreateBlock(Vector3 position)
        {
            BlockData blockData = _levelEditor.GetBlock();
            if (blockData == null || blockData.Prefab == null)
            {
                Debug.LogError("Invalid block data or prefab!");
                return;
            }

            GameObject block = PrefabUtility.InstantiatePrefab(blockData.Prefab, _parent) as GameObject;
            if (block == null)
            {
                Debug.LogError("Failed to instantiate block!");
                return;
            }

            block.transform.position = position;
            block.name = "block"; // Изменено здесь - теперь всегда "Block"

            if (block.TryGetComponent(out BaseBlock baseBlock))
            {
                baseBlock.BlockData = blockData;

                if (blockData is ColoredBlock coloredBlockData &&
                    block.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.color = coloredBlockData.BaseColor;
                }
            }

            Undo.RegisterCreatedObjectUndo(block, "Create Block");
            EditorUtility.SetDirty(block);
        }
    }
}