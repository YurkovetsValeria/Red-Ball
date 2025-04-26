using UnityEngine;

namespace GameDevLabirinth
{
    public class EditorGrid
    {
        private const float _leftPosition = -4.5f;
        private const float _upPosition = 8.75f;
        private const int _columnCount = 10;
        private const int _lineCount = 20;
        private const float _offsetDown = 0.5f;
        private const float _offsetRight = 1f;

        public Vector3 CheckPosition(Vector3 position)
        {
            float tempX = 0;
            float tempY = 0;
            float x = _leftPosition - _offsetRight / 2;
            float y = _upPosition + _offsetDown / 2;

            if (position.x > x && position.x < (x + _offsetRight * _columnCount) &&
                position.y < y && position.y > (y - _offsetDown * _lineCount))
            {
                tempX = Mathf.Round((position.x - _leftPosition) / _offsetRight) * _offsetRight + _leftPosition;
                tempY = Mathf.Round((y - position.y) / _offsetDown) * _offsetDown + (y - _offsetDown * _lineCount);

                return new Vector3(tempX, tempY);
            }
            else
            {
                Debug.LogWarning("out of play zone");
                return Vector3.zero; // ”казываем, что позици€ недопустима€
            }
        }
    }
}