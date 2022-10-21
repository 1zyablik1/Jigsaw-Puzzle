using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Sprite sprite;

    public Vector2 puzzleSize = Vector2.one;
    public int width = 1, height = 1;


    public PieceForm[,] puzzle;

    
    private void Awake()
    {
        PuzzleGenerator puzzleGenerator = new PuzzleGenerator(this.transform);
        puzzleSize.x = width;
        puzzleSize.y = height;
        puzzle = puzzleGenerator.GenerateNewPuzzle(puzzleSize, sprite);
            //.texture.SetPixels32(puzzleGenerator.GetRotatedTexture().GetPixels32());

        Events.puzzleSelected += ButClick;
    }

    private void ButClick(Sprite sprite)
    {
    }

}
