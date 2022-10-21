using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGeneratingPuzzleGrid : MonoBehaviour
{
    public GameObject generatingObj;

    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;
    public Vector2 offsetMulty;
    public Vector2 scaleMulty;
    public Vector2 multy;

    public Vector2 puzzleSize;

    void Start()
    {
        _propBlock = new MaterialPropertyBlock();


        GenerateField();


        //_renderer.SetPropertyBlock(_propBlock);
    }

    public void GenerateField()
    {
        multy.x = 1f / puzzleSize.x;
        multy.y = 1f / puzzleSize.y;

        Vector2 tiling = new Vector2(multy.x, multy.y);

        for (int i = 0; i < puzzleSize.x; i++)
        {

            //row parent
            for(int j = 0; j < puzzleSize.y; j++)
            {
                Vector3 newPos = new Vector3(i, j, 0);
                var newPuzzle = Instantiate(generatingObj ,newPos, Quaternion.identity);
                newPuzzle.transform.SetParent(this.transform);

                _renderer = newPuzzle.GetComponent<Renderer>();
                _renderer.GetPropertyBlock(_propBlock);

                Vector2 offset = new Vector2(i * multy.x, j * multy.y);

                //Vector4 offset_tiling = new Vector4(offset.x, offset.y, tiling.x, tiling.y);
                _propBlock.SetVector("_offset", offset);
                _propBlock.SetVector("_tiling", tiling);


                _renderer.SetPropertyBlock(_propBlock);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


