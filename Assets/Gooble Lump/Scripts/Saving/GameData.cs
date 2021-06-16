using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class NameScorePair
    {
        public string name;
        public int score;

        public NameScorePair(string _name, int _score)
        {
            name = _name;
            score = _score;
        }
    }

    [System.Serializable]
    public class GameData
    {
        public List<NameScorePair> highScores;

        public GameData()
        {
            highScores = new List<NameScorePair>();
        }

        public int IndexOfName(string name)
        {
            for (int i = 0; i < highScores.Count; i++)
            {
                if (highScores[i].name == name)
                    return i;
            }
            if (highScores.Count == 0)
                return 1;
            return highScores.Count;
        }

        void SortHighScores()
        {
            List<NameScorePair> highScoresCopy = highScores;
            bool isSorted = false;
            while (!isSorted)
            {
                bool possiblySorted = true;
                for (int j = 0; j < highScoresCopy.Count - 1; j++)
                {
                    if (highScoresCopy[j].score < highScoresCopy[j + 1].score)
                    {
                        possiblySorted = false;
                        NameScorePair higher = highScoresCopy[j];
                        NameScorePair lower = highScoresCopy[j + 1];
                        highScoresCopy[j] = lower;
                        highScoresCopy[j + 1] = higher;
                    }
                }
                isSorted = possiblySorted;
            }
            highScores = highScoresCopy;
        }

        void TrimHighScores()
        {
            int maxHighScores = 8;
            if (highScores.Count > maxHighScores)
            {
                for (int i = highScores.Count; i > maxHighScores; i--)
                    highScores.RemoveAt(i - 1);
            }
        }

        public void AddScore(string name, int score)
        {
            int indexOfName = IndexOfName(name);
            Debug.Log($"indexOfName: {indexOfName}");
            if (indexOfName < highScores.Count)
            {
                if (highScores[indexOfName].score < score)
                {
                    highScores[indexOfName] = new NameScorePair(name, score);
                }
            }
            else
            {
                highScores.Add(new NameScorePair(name, score));
            }
            SortHighScores();
            TrimHighScores();
        }
    }
}