using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wyrd{
    public string str;
    public List<Letter> letters = new List<Letter>();
    public bool found = false;

    public bool visible
    {
        get
        {
            if (letters.Count == 0)
            {
                return (false);
            }
            return (letters[0].visible);
        }
        set
        {
            foreach (Letter lett in letters)
            {
                lett.visible = value;
            }
        }
    }

    public Color color
    {
        get
        {
            if (letters.Count == 0)
            {
                return (Color.black);
            }
            return (letters[0].color);
        }
        set
        {
            foreach (Letter lett in letters)
            {
                lett.color = value;
            }
        }
    }

    public void Add(Letter lett)
    {
        letters.Add(lett);
        str += lett.c.ToString();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
