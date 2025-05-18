using UnityEngine;

namespace GameDevLabirinth
{
    public class CalculationLevelProgress : MonoBehaviour
    {
        [SerializeField] private PlayerLife _playerLife;
        [SerializeField] private ScoreController _scoreController;
        private Progress _progress = new Progress();
        private readonly LevelsData _levelsData = new LevelsData();
        private readonly LevelIndex _levelIndex = new LevelIndex();
        private EndGameData _endGameData;

        private void Calculate()
        {
            int currentLevelIndex = _levelIndex.GetIndex();
            _progress = _levelsData.GetLevelsProgress().Levels[currentLevelIndex];

            // Получаем текущий счет и определяем, побит ли рекорд
            int currentScore = _scoreController.GetScore();
            bool isNewRecord = currentScore > _progress.MaxScore;

            // Если рекорд побит, обновляем его сразу
            if (isNewRecord)
            {
                _progress.MaxScore = currentScore;
                _levelsData.SaveLevelData(currentLevelIndex, _progress, false); // false - сохраняем только рекорд
            }

            _endGameData = new EndGameData()
            {
                LevelIndex = currentLevelIndex,
                Life = _playerLife.GetLifeCount(),
                Score = currentScore,
                Record = _progress.MaxScore,
                IsWin = _playerLife.GetLifeCount() > 0,
                IsNewRecord = isNewRecord
            };

            // Сохраняем полный прогресс (включая звезды, если игрок выиграл)
            SaveLevelProgress(_endGameData.IsWin);
        }

        private void SaveLevelProgress(bool isWin)
        {
            if (_endGameData.IsNewRecord)
            {
                _progress.MaxScore = _endGameData.Score;
            }

            if (isWin && _progress.StarsCount < _endGameData.Life)
            {
                _progress.StarsCount = _endGameData.Life;
            }

            _levelsData.SaveLevelData(_levelIndex.GetIndex(), _progress, isWin);
        }

        public EndGameData GetEndGameData()
        {
            Calculate();
            return _endGameData;
        }
    }

    public struct EndGameData
    {
        public int LevelIndex;
        public int Life;
        public int Score;
        public int Record;
        public bool IsWin;
        public bool IsNewRecord;
    }
}