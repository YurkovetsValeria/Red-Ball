using UnityEngine;

namespace GameDevLabirinth
{
    public class GlobalFunction : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
            Debug.Log("Вы вышли из игры");
        }
    }
}