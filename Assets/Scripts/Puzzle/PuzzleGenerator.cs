using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator
{
    private const string puzzlePath = "Prefabs/Piece";
    private const string puzzleBodyPath = "PuzzlePiece/pieceBody";
    private const string puzzleBodyConnector = "PuzzlePiece/pieceConnector";

    private Vector2 puzzleSize;
    private Sprite puzzleSprite;

    private Texture2D puzzleFormBody;
    private Texture2D puzzleFormConnectorUp;
    private Texture2D puzzleFormConnectorLeft;
    private Texture2D puzzleFormConnectorDown;
    private Texture2D puzzleFormConnectorRight;
    
    private Transform parentTransform;
    private Piece puzzlePiece;

    private PieceForm[,] connectors;
    private Piece[,] pieces;

    private Pool<Piece> piecesPool;

    private MaterialPropertyBlock propertyBlock;

    public PuzzleGenerator(Transform parentTransform)
    {
        this.puzzleSize = Vector2.one;
        this.puzzleSprite = null;
        this.parentTransform = parentTransform;

        propertyBlock = new MaterialPropertyBlock();

        GetPuzzlePiece();

        piecesPool = new Pool<Piece>(puzzlePiece, this.parentTransform, 160);
    }

    public PieceForm[,] GenerateNewPuzzle(Vector2 puzzleSize, Sprite puzzleSprite)
    {
        this.puzzleSize = puzzleSize;
        this.puzzleSprite = puzzleSprite;

        connectors = new PieceForm[(int)puzzleSize.y, (int)puzzleSize.x];
        pieces = new Piece[(int)puzzleSize.y, (int)puzzleSize.x];

        //SetPuzzleTexture();
        GenerateField();

        return connectors;
    }

    private void GetPuzzlePiece()
    {
        puzzlePiece = Resources.Load<Piece>(puzzlePath);

        puzzleFormBody = Resources.Load<Sprite>(puzzleBodyPath).texture;
        puzzleFormConnectorUp = Resources.Load<Sprite>(puzzleBodyConnector).texture;

        puzzleFormConnectorLeft = TextureUtility.RotateTexture(puzzleFormConnectorUp, false);
        puzzleFormConnectorDown = TextureUtility.RotateTexture(puzzleFormConnectorLeft, false);
        puzzleFormConnectorRight = TextureUtility.RotateTexture(puzzleFormConnectorUp, true);
    }

    private void SetPuzzleTexture()
    {
        puzzlePiece.GetComponent<Renderer>().sharedMaterial.SetTexture("_piecePuzzle", puzzleSprite.texture);
    }


    private void GenerateField()
    {
        for(int i = 0; i < puzzleSize.y; i++)
        {
            for(int j = 0; j < puzzleSize.x; j++)
            {
                GeneratePieceData(i, j);
                GeneratePieceForm(i, j);

                SetPiece(i, j);
            }
        }
    }

    private void SetPiece(int i, int j)
    {
        pieces[i, j] = piecesPool.GetFreeElement(new Vector3(j, -i, 0));

        //var pieceRenderer = pieces[i, j].GetComponent<SpriteRenderer>();
        //pieceRenderer.sprite = Sprite.Create(connectors[i, j].pieceTexture, new Rect(0, 0, connectors[i, j].pieceTexture.width, connectors[i, j].pieceTexture.height), new Vector2(0.5f, 0.5f));
        //Sprite sprite = Sprite.Create(connectors[i, j].pieceTexture, new Rect(0, 0, connectors[i, j].pieceTexture.width, connectors[i, j].pieceTexture.height), new Vector2(0.5f, 0.5f));
        //var a = pieceRenderer.GetComponent<Sprite>();
        //a = sprite;

        Sprite sprite = Sprite.Create(connectors[i, j].pieceTexture, new Rect(0, 0, connectors[i, j].pieceTexture.width, connectors[i, j].pieceTexture.height), new Vector2(0, 0)); 
        SpriteRenderer pieceRenderer = pieces[i, j].GetComponent<SpriteRenderer>();
        pieceRenderer.sprite = sprite;

        //pieceRenderer = pieces[i, j].GetComponent<Renderer>();

        //pieceRenderer.GetPropertyBlock(propertyBlock);

        //propertyBlock.SetTexture("_pieceForm", connectors[i, j].pieceTexture);

        //pieceRenderer.SetPropertyBlock(propertyBlock);
    }

    private void GeneratePieceData(int i, int j)
    {
        PieceForm newPiece = new PieceForm();
        //up
        {
            if (i == 0)
            {
                newPiece.upConnector = PieceConnector.NoConnection;
            }
            else
            {
                int connector = 0;
                if (connectors[i - 1, j].downConnector == 0)
                {
                    connector = 1;
                }

                newPiece.upConnector = (PieceConnector)connector;
            }
        }
        //left
        {
            if (j == 0)
            {
                newPiece.leftConnector = PieceConnector.NoConnection;
            }
            else
            {
                int connector = 0;
                if (connectors[i, j - 1].rightConnector == 0)
                {
                    connector = 1;
                }

                newPiece.leftConnector = (PieceConnector)connector;
            }
        }
        //down
        {
            if (i == puzzleSize.y - 1)
            {
                newPiece.downConnector = PieceConnector.NoConnection;
            }
            else
            {
                int connector = UnityEngine.Random.Range(0, 2);
                newPiece.downConnector = (PieceConnector)connector;
            }
        }
        //right
        {
            if (j == puzzleSize.x - 1)
            {
                newPiece.rightConnector = PieceConnector.NoConnection;
            }
            else
            {
                int connector = UnityEngine.Random.Range(0, 2);
                newPiece.rightConnector = (PieceConnector)connector;
            }
        }
        connectors[i, j] = newPiece;
    }

    private void GeneratePieceForm(int i, int j)
    {
        int upMultyOffset = (connectors[i, j].upConnector is PieceConnector.NoConnection) ? 0 : (int)connectors[i, j].upConnector;
        int leftMultyOffset = (connectors[i, j].leftConnector is PieceConnector.NoConnection) ? 0 : (int)connectors[i, j].leftConnector;
        int downMultyOffset = (connectors[i, j].downConnector is PieceConnector.NoConnection) ? 0 : (int)connectors[i, j].downConnector;
        int rightMultyOffset = (connectors[i, j].rightConnector is PieceConnector.NoConnection) ? 0 : (int)connectors[i, j].rightConnector;

        int newHeight = upMultyOffset * puzzleFormConnectorUp.height +
                        downMultyOffset * puzzleFormConnectorDown.height +
                        (puzzleFormBody.height);

        int newWidth = leftMultyOffset * puzzleFormConnectorLeft.width +
                       rightMultyOffset * puzzleFormConnectorRight.width +
                       (puzzleFormBody.width);

        Texture2D newPieceTexture = new Texture2D(newWidth, newHeight);
        newPieceTexture = TextureUtility.SetAlphaTexture(newPieceTexture);

        //main
        newPieceTexture.SetPixels32(leftMultyOffset * puzzleFormConnectorLeft.width,
                                    downMultyOffset * puzzleFormConnectorDown.height,
                                    puzzleFormBody.width,
                                    puzzleFormBody.height,
                                    puzzleFormBody.GetPixels32()
                                    );
        //up
        if (connectors[i, j].upConnector is PieceConnector.ConnectionPlus)
        {
            int xCoordConnection = puzzleFormConnectorLeft.width * leftMultyOffset + puzzleFormBody.width / 2 - puzzleFormConnectorUp.width/ 2;
            int yCoordConnection = newPieceTexture.height - puzzleFormConnectorUp.height; 

            newPieceTexture.SetPixels32(xCoordConnection,
                                        yCoordConnection,
                                        puzzleFormConnectorUp.width,
                                        puzzleFormConnectorUp.height,
                                        puzzleFormConnectorUp.GetPixels32()
                                        );
        }
        else if (connectors[i, j].upConnector is PieceConnector.ConnectionMinus)
        {
            int xCoordConnection = puzzleFormConnectorLeft.width * leftMultyOffset + puzzleFormBody.width / 2 - puzzleFormConnectorDown.width / 2;
            int yCoordConnection = newPieceTexture.height - puzzleFormConnectorUp.height;

            newPieceTexture.SetPixels32(xCoordConnection,
                                        yCoordConnection,
                                        puzzleFormConnectorDown.width,
                                        puzzleFormConnectorDown.height,
                                        TextureUtility.InvertTextureAlpha(puzzleFormConnectorDown).GetPixels32()
                                        );
        }
        //left
        if (connectors[i, j].leftConnector is PieceConnector.ConnectionPlus)
        {
            int yCoordConnection = puzzleFormConnectorDown.height * downMultyOffset + puzzleFormBody.height / 2 - puzzleFormConnectorLeft.height / 2;

            newPieceTexture.SetPixels32(0,
                                        yCoordConnection,
                                        puzzleFormConnectorLeft.width,
                                        puzzleFormConnectorLeft.height,
                                        puzzleFormConnectorLeft.GetPixels32()
                                        );
        }
        else if (connectors[i, j].leftConnector is PieceConnector.ConnectionMinus)
        {
            int yCoordConnection = puzzleFormConnectorDown.height * downMultyOffset + puzzleFormBody.height / 2 - puzzleFormConnectorRight.height / 2;

            newPieceTexture.SetPixels32(0,
                                        yCoordConnection,
                                        puzzleFormConnectorRight.width,
                                        puzzleFormConnectorRight.height,
                                        TextureUtility.InvertTextureAlpha(puzzleFormConnectorRight).GetPixels32()
                                        );
        }
        //down
        if (connectors[i, j].downConnector is PieceConnector.ConnectionPlus)
        {
            int xCoordConnection = puzzleFormConnectorLeft.width * leftMultyOffset + puzzleFormBody.width / 2 - puzzleFormConnectorDown.width / 2;

            newPieceTexture.SetPixels32(xCoordConnection,
                                        0,
                                        puzzleFormConnectorDown.width,
                                        puzzleFormConnectorDown.height,
                                        puzzleFormConnectorDown.GetPixels32()
                                        );
        }
        else if (connectors[i, j].downConnector is PieceConnector.ConnectionMinus)
        {
            int xCoordConnection = puzzleFormConnectorLeft.width * leftMultyOffset + puzzleFormBody.width / 2 - puzzleFormConnectorUp.width / 2;

            newPieceTexture.SetPixels32(xCoordConnection,
                                        0,
                                        puzzleFormConnectorUp.width,
                                        puzzleFormConnectorUp.height,
                                        TextureUtility.InvertTextureAlpha(puzzleFormConnectorUp).GetPixels32()
                                        );
        }
        //right
        if (connectors[i, j].rightConnector is PieceConnector.ConnectionPlus)
        {
            int yCoordConnection = puzzleFormConnectorDown.height * downMultyOffset + puzzleFormBody.height / 2 - puzzleFormConnectorRight.height / 2;
            int xCoordConnection = newWidth - puzzleFormConnectorRight.width;

            newPieceTexture.SetPixels32(xCoordConnection,
                                        yCoordConnection,
                                        puzzleFormConnectorRight.width,
                                        puzzleFormConnectorRight.height,
                                        puzzleFormConnectorRight.GetPixels32()
                                        );
        }
        else if(connectors[i, j].rightConnector is PieceConnector.ConnectionMinus)
        {
            int yCoordConnection = puzzleFormConnectorDown.height * downMultyOffset + puzzleFormBody.height / 2 - puzzleFormConnectorLeft.height / 2;
            int xCoordConnection = newPieceTexture.width - puzzleFormConnectorRight.width;

            newPieceTexture.SetPixels32(xCoordConnection,
                                        yCoordConnection,
                                        puzzleFormConnectorLeft.width,
                                        puzzleFormConnectorLeft.height,
                                        TextureUtility.InvertTextureAlpha(puzzleFormConnectorLeft).GetPixels32()
                                        );
        }

        newPieceTexture.Apply();
        newPieceTexture.filterMode = FilterMode.Point;

        connectors[i, j].pieceTexture = newPieceTexture;
    }

}


