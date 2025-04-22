using UnityEngine;
using System.Collections.Generic;

namespace GameDevLabirinth
{
    public class Block : MonoBehaviour
    {
        private static int _coat = 0;
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private int _score;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _life;
        private ParticleSystem _particleSystem;

#if UNITY_EDITOR
        public BlockData BlockData;
#endif

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystem = GetComponent<ParticleSystem>(); // Получаем ParticleSystem

            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer отсутствует на объекте Block!");
                return;
            }

            _spriteRenderer.color = Color.white; // Делаем блок полностью видимым
        }

        public void SetData(ColoredBlock blockData)
        {
            _sprites = new List<Sprite>(blockData.Sprites);
            _score = blockData.Score;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystem = GetComponent<ParticleSystem>();

            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer отсутствует при SetData!");
                return;
            }

            // Проверяем цвет, устанавливаем если он корректен
            if (blockData.BaseColor.a == 0)
            {
                blockData.BaseColor = new Color(blockData.BaseColor.r, blockData.BaseColor.g, blockData.BaseColor.b, 1f);
            }

            _spriteRenderer.color = blockData.BaseColor;
            _life = _sprites.Count;

            if (_life > 0)
            {
                _spriteRenderer.sprite = _sprites[_life - 1];
            }
            else
            {
                Debug.LogWarning("Список спрайтов пуст, объект может быть невидимым!");
            }

            // Устанавливаем цвет частиц
            if (_particleSystem != null)
            {
                var main = _particleSystem.main;
                main.startColor = blockData.BaseColor;
            }
        }

        public void ApplyDamage()
        {
            _life--;
            if (_life < 1)
            {
                _spriteRenderer.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;

                // Проверяем, есть ли ParticleSystem, и запускаем
                if (_particleSystem != null)
                {
                    _particleSystem.Play();
                }
            }
            else if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _sprites[_life - 1];
            }
        }

        private void OnEnable()
        {
            _coat++;
        }

        private void OnDisable()
        {
            _coat--;
            if (_coat < 1)
            {
                Debug.Log("block ended");
            }
        }
    }
}
