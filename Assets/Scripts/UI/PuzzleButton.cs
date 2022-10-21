using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour
{
    private Sprite image;

    void Start()
    {
        image = GetComponent<Image>().sprite;
    }

    public void PuzzleSelected()
    {
        Events.puzzleSelected?.Invoke(image);
    }
}
