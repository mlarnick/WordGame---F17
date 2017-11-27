using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameMode
{
    preGame,
    loading,
    makeLevel,
    levelPrep,
    inLevel
}

public class WordGame : MonoBehaviour {

    static public WordGame S;

    public GameObject prefabLetter;
    public Rect wordArea = new Rect(-24, 19, 48, 28);
    public float letterSize = 1.5f;
    public bool showAllWyrds = true;
    public float bigLetterSize = 4f;

    public bool _______________;

    public GameMode mode = GameMode.preGame;
    public WordLevel currLevel;
    public List<Wyrd> wyrds;

    void Awake()
    {
        S = this;
    }

	// Use this for initialization
	void Start () {
        mode = GameMode.loading;
        WordList.S.Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WordListParseComplete()
    {
        mode = GameMode.makeLevel;
        currLevel = MakeWordLevel();
    }

    public WordLevel MakeWordLevel(int levelNum = -1)
    {
        WordLevel level = new WordLevel();
        if (levelNum == -1)
        {
            level.longWordIndex = Random.Range(0, WordList.S.longWordCount);
        }
        else
        {

        }

        level.levelNum = levelNum;
        level.word = WordList.S.GetLongWord(level.longWordIndex);
        level.charDict = WordLevel.MakeCharDict(level.word);

        StartCoroutine(FindSubWordsCoroutine(level));

        return (level);
    }

    public IEnumerator FindSubWordsCoroutine(WordLevel level)
    {
        level.subWords = new List<string>();
        string str;

        List<string> words = WordList.S.GetWords();

        for (int i = 0; i < WordList.S.wordCount; i++)
        {
            str = words[i];

            if (WordLevel.CheckWordInLevel(str, level))
            {
                level.subWords.Add(str);
            }

            if (1 % WordList.S.numToParseBeforeYield == 0)
            {
                yield return null;
            }
        }
        level.subWords.Sort();
        level.subWords = SortWordsByLength(level.subWords).ToList();

        SubWordSearchComplete();
    }

    public static IEnumerable<string> SortWordsByLength(IEnumerable<string> e)
    {
        var sorted = from s in e
            orderby s.Length ascending
                select s;

        return sorted;
    }

    public void SubWordSearchComplete()
    {
        mode = GameMode.levelPrep;
        Layout();
    }

    void Layout()
    {
        wyrds = new List<Wyrd>();

        GameObject go;
        Letter lett;
        string word;
        Vector3 pos;
        float left = 0;
        float columnWidth = 3;
        char c;
        Color col;
        Wyrd wyrd;

        int numRows = Mathf.RoundToInt(wordArea.height / letterSize);

        for (int i = 0; i <currLevel.subWords.Count; i++)
        {
            wyrd = new Wyrd();
            word = currLevel.subWords[i];

            columnWidth = Mathf.Max(columnWidth, word.Length);

            for (int j = 0; j < word.Length; j++)
            {
                c = word[j];
                go = Instantiate(prefabLetter) as GameObject;
                lett = go.GetComponent<Letter>();
                lett.c = c;
                pos = new Vector3(wordArea.x + left + j * letterSize, wordArea.y, 0);
                pos.y -= (i % numRows) * letterSize;
                lett.pos = pos;
                go.transform.localScale = Vector3.one * letterSize;
                wyrd.Add(lett);
            }

            if (showAllWyrds)
            {
                wyrd.visible = true;
            }

            wyrds.Add(wyrd);

            if (1 % numRows == numRows - 1)
            {
                left += (columnWidth + .5f) * letterSize;
            }
        }

    }
}
