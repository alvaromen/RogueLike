using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    void Awake()
    {
        //entryContainer = transform.Find("highscoreEntryContainer");
        //entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        /*
        highscoreEntryList = new List<HighscoreEntry>();
        AddHighscoreEntry(1000000, "AMG");
        */
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
        
    }        

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        for (int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                case 1:
                    rankString = "1ST";
                    break;
                case 2:
                    rankString = "2ND";
                    break;
                case 3:
                    rankString = "3RD";
                    break;
                default:
                    rankString = rank + "TH";
                    break;
            }

            entryTransform.Find("posText").GetComponent<Text>().text = rankString;

            int score = highscoreEntry.score;

            entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

            string name = highscoreEntry.name;
            entryTransform.Find("nameText").GetComponent<Text>().text = name;

            entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

            if(rank == 1)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.yellow;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.yellow;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.yellow;
            }

            if (rank == 2)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.cyan;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.cyan;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.cyan;
            }

            if (rank == 3)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.red;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.red;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.red;
            }

            transformList.Add(entryTransform);
        }
    }

    private void AddHighscoreEntry(int score, string name)
    {
        //Create the entry
        HighscoreEntry highscoreEntry = new HighscoreEntry(score, name);

        //Load saved data
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        /*
        Highscores highscores = new Highscores();
        highscores.highscoreEntryList = highscoreEntryList;
        */
        //Add the new entry
        highscores.highscoreEntryList.Add(highscoreEntry);

        //Save the updated highscore
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }


    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;

        public HighscoreEntry(int s, string n)
        {
            score = s;
            name = n;
        }
    }
}
