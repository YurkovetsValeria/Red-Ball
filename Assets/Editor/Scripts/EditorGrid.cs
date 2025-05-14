using UnityEngine;

namespace GameDevLabirinth
{
    public class EditorGrid
    {
        // Корректируем параметры сетки под размер камеры 9.6
        private const float _leftPosition = -5f; // Было -4.5f (смещаем влево)
        private const float _upPosition = 9f;
        private const int _columnCount = 10;
        private const int _lineCount = 29;
        private const float _offsetDown = 0.5f;
        private const float _offsetRight = 1f;

        public Vector3 CheckPosition(Vector3 position)
        {
            float x = _leftPosition;
            float y = _upPosition;

            float rightBound = _leftPosition + _offsetRight * _columnCount;
            float bottomBound = _upPosition - _offsetDown * _lineCount;

            if (position.x >= _leftPosition && position.x <= rightBound &&
                position.y <= _upPosition && position.y >= bottomBound)
            {
                for (int i = 0; i < _columnCount; i++)
                {
                    if (position.x >= x && position.x < x + _offsetRight)
                    {
                        x += _offsetRight / 2;
                        break;
                    }
                    x += _offsetRight;
                }

                for (int i = 0; i < _lineCount; i++)
                {
                    if (position.y <= y && position.y > y - _offsetDown)
                    {
                        y -= _offsetDown / 2;
                        break;
                    }
                    y -= _offsetDown;
                }

                return new Vector3(x, y, 0);
            }

            Debug.LogWarning($"Position {position} is out of grid bounds");
            return Vector3.zero;
        }

        public void DrawGrid()
        {
            for (int i = 0; i <= _columnCount; i++)
            {
                float xPos = _leftPosition + i * _offsetRight;
                Vector3 start = new Vector3(xPos, _upPosition, 0);
                Vector3 end = new Vector3(xPos, _upPosition - _lineCount * _offsetDown, 0);
                Debug.DrawLine(start, end, Color.green, 0.5f);
            }

            for (int i = 0; i <= _lineCount; i++)
            {
                float yPos = _upPosition - i * _offsetDown;
                Vector3 start = new Vector3(_leftPosition, yPos, 0);
                Vector3 end = new Vector3(_leftPosition + _columnCount * _offsetRight, yPos, 0);
                Debug.DrawLine(start, end, Color.blue, 0.5f);
            }
        }
    }
}