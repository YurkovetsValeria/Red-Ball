using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevLabirinth
{
    public class BallCollisions : MonoBehaviour
    {
        [SerializeField] private BallMove _ball;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _ball.MoveCollision(collision);

            if (collision.gameObject.TryGetComponent(out Block block))
            {
                block.ApplyDamage(); // Теперь вызываем метод у объекта
            }
        }
    }
}
