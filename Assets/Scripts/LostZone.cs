using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevLabirinth
{
    public class LostZone : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<BallMove>(out BallMove ball))
            {
                Destroy(ball.gameObject);
            }
        }
    }
}
