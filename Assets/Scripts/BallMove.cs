using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevLabirinth
{
    public class BallMove : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private bool _isActiv;
        private const float Force = 300f;
        private float _lastPositionX;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backspace) && !_isActiv)
            {
                BallActivete();
            }
#endif

#if UNITY_ANDROID
            if (Input.touchCount > 0 && !_isActiv)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.tapCount > 1)
                {
                    BallActivete();
                }
            }
#endif
        }

        private void BallActivete()
        {
            _lastPositionX = transform.position.x;
            _isActiv = true;
            transform.SetParent(null);
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.AddForce(Vector2.up * Force);
        }

        public void MoveCollision(Collision2D collision)
        {
            float ballPositionX = transform.position.x;

            if(collision.gameObject.TryGetComponent(out PlayerMove player))
            {
                if (ballPositionX < _lastPositionX + 0.1 && ballPositionX > _lastPositionX - 0.1)  // движение почти вертикальное
                {
                    float collisionPointX = collision.contacts[0].point.x;
                    _rigidbody2D.velocity = Vector2.zero;
                    float playerCenterPosition = player.gameObject.GetComponent<Transform>().position.x;
                    float difference = playerCenterPosition - collisionPointX; // разница между центром вагенетки и местом касания
                    float direction = collisionPointX < playerCenterPosition ? -1 : 1; // расчет направления
                    _rigidbody2D.AddForce(new Vector2(direction * Mathf.Abs(difference * (Force / 2)), Force));
                }
            }

            _lastPositionX = ballPositionX;
        }
    }
}
