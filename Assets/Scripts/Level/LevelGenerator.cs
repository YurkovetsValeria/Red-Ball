using UnityEngine;
using UnityEngine.Events;

namespace GameDevLabirinth
{
    public class LevelGenerator : MonoBehaviour
    {
        private readonly LevelIndex _levelIndex = new LevelIndex();
        private readonly BlocksGenerator _blocksGenerator = new BlocksGenerator();

        [SerializeField] private Transform _parentBlocks;
        [SerializeField] private ClearLevel _clearLevel;
        [SerializeField] private GameState _gameState;
        [SerializeField] private UnityEvent OnGenerated;

        [Space]
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private GameObject gameplayUI;

        private void Start()
        {
            _gameState.SetState(State.StopGame);
            Init();
        }

        private void Init()
        {
            _clearLevel.Clear();

            GameLevel gameLevel = Resources.Load<GameLevel>($"Levels/Level{_levelIndex.GetIndex()}");
            if (gameLevel != null)
            {
                _blocksGenerator.Generate(gameLevel, _parentBlocks);
                _background.sprite = gameLevel.Background;
            }

            LoadingScreen.Screen.Enable(false);

            if (gameplayUI != null)
                gameplayUI.SetActive(true);

            _gameState.SetState(State.Gameplay);
            OnGenerated.Invoke();
        }

        public void Generate()
        {
            if (gameplayUI != null)
                gameplayUI.SetActive(false);

            LoadingScreen.Screen.Enable(true);
            Init();
        }

        public void GenerateNext()
        {
            LevelsData levelsData = new LevelsData();
            int tempIndex = _levelIndex.GetIndex();

            // Проверяем, что текущий уровень завершен (имеет звезды)
            var currentProgress = levelsData.GetLevelsProgress().Levels[tempIndex];
            if (currentProgress.StarsCount > 0 && tempIndex < levelsData.GetLevelsProgress().Levels.Count - 1)
            {
                _levelIndex.SetIndex(tempIndex + 1);
                Generate();
            }
            else
            {
                Loader loader = new Loader();
                _gameState.SetState(State.Other);
                loader.LoadingMainScene(true);
            }
        }
    }
}