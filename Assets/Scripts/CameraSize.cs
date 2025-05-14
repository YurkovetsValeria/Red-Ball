using UnityEngine;

namespace GameDevLabirinth.Tools
{
    public class CameraSize : MonoBehaviour
    {
        [SerializeField] private float _targetOrthographicSize = 9.6f; // Фиксированный размер камеры
        [SerializeField] private bool _isVertical = true; // Для вертикальной ориентации

        private const float ReferenceWidth = 1080f;  // Ширина (для вертикального режима)
        private const float ReferenceHeight = 1920f; // Высота (для вертикального режима)

        private void Start()
        {
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            if (Camera.main == null) return;

            float currentAspect = (float)Screen.width / Screen.height;
            float referenceAspect = ReferenceWidth / ReferenceHeight;

            if (_isVertical)
            {
                // Вертикальная ориентация (высота важнее)
                if (currentAspect < referenceAspect)
                {
                    // Если экран уже, чем 9:16 (например, 9:18), увеличиваем orthographicSize
                    Camera.main.orthographicSize = _targetOrthographicSize * (referenceAspect / currentAspect);
                }
                else
                {
                    // Если шире или такое же — оставляем базовый размер
                    Camera.main.orthographicSize = _targetOrthographicSize;
                }
            }
            else
            {
                // Горизонтальная ориентация (ширина важнее)
                if (currentAspect > referenceAspect)
                {
                    Camera.main.orthographicSize = _targetOrthographicSize * (currentAspect / referenceAspect);
                }
                else
                {
                    Camera.main.orthographicSize = _targetOrthographicSize;
                }
            }

            Camera.main.transform.position = new Vector3(0, 0, -10);
        }

#if UNITY_EDITOR
        [ContextMenu("Force Update Camera")]
        private void ForceUpdate()
        {
            UpdateCamera();
            Debug.Log($"Camera size: {Camera.main.orthographicSize}");
        }
#endif
    }
}