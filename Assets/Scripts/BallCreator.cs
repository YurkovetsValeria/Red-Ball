using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevLabirinth
{
    public class BallCreator : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        private const float OffsetY = 0.5f;

        public void Start()
        {
            Create();
        }

        public void Create()
        {
            Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y + OffsetY), Quaternion.identity, transform);
        }

        public void CreateClone()
        {
            Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y + OffsetY), Quaternion.identity);
        }
    }
}

