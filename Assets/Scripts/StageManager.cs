using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public TextAsset[] stageFiles;
    TileType[,] tileTable;

    public TileManager tilePrefab;
    TileManager[,] tileTableObj;

    // delegate:別のクラスの関数を登録して実行できる
    public delegate void StageClear();
    public StageClear stageClear;

    public delegate void BonusGet();
    public BonusGet bonusGet;

    [SerializeField] Vector2 shiftPosition;

    bool isFever = false;

    [SerializeField] SoundManager soundManager;

    void Update()
    {
        if (isFever == true)
        { 
            if (Input.GetMouseButtonDown(0))
            {
                for (int y = 0; y < tileTableObj.GetLength(1); y++)
                {
                    for (int x = 0; x < tileTableObj.GetLength(0); x++)
                    {
                        tileTableObj[x, y].AllAliveTile();
                    }
                }
                Invoke("getCoin", 0.05f);
                soundManager.BreakPlay();
            }
        }
    }

    public void getCoin()
    {
        stageClear();
        soundManager.ClearPlay();
    }

    public void BonusedTile()
    {
        if (isFever == false)
        {
            soundManager.SlidePlay();
            soundManager.ClearPlay();
            soundManager.BonusPlay();
            bonusGet();
        }
    }

    public void CreateStage()
    {
        Vector2 halfSize;
        float tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        halfSize.x = tileSize * (tileTable.GetLength(0) / 2);
        halfSize.y = tileSize * (tileTable.GetLength(1) / 2);

        for (int y = 0; y < tileTable.GetLength(1); y++)
        {
            for (int x = 0; x < tileTable.GetLength(0); x++)
            {
                TileManager tile = Instantiate(tilePrefab);
                Vector2Int position = new Vector2Int(x, y);
                tile.SetInit(tileTable[x, y], position, isFever);
                tile.clicked += ClickedTile;
                tile.makeTurn += MakeTile;
                tile.bonused += BonusedTile;

                Vector2 setPosition = (Vector2)position * tileSize - halfSize;
                setPosition.y *= -1;
                tile.transform.position = setPosition + shiftPosition;
                tileTableObj[x, y] = tile;
            }
        }
    }

    public void MakeStage(bool isPlay)
    {
        //ランダム実行
        int x = Random.Range(0, 4);
        int y = Random.Range(0, 4);
        TileManager tile = tileTableObj[x, y];
        tile.Make();

        int x2;
        int y2;
        while (true)
        {
            x2 = Random.Range(0, 4);
            y2 = Random.Range(0, 4);
            if (x2 != x || y2 != y) break;
        }
        tile = tileTableObj[x2, y2];
        tile.Make();

        /*ここどうしよう
        int x3 = Random.Range(0, 4);
        int y3 = Random.Range(0, 4);
        while (true)
        {
            x3 = Random.Range(0, 4);
            y3 = Random.Range(0, 4);
            if (x3 != x || x3 != x2 || y2 != y2 || y3 != y) break;
        }
        tile = tileTableObj[x3, y3];
        tile.Make();
        */
        int R = Random.Range(0, 9);
        if (isFever == false && R == 0 && isPlay == true)
        { 
            int jx;
            int jy;

            while (true)
            {
                jx = Random.Range(0, 4);
                jy = Random.Range(0, 4);
                tile = tileTableObj[jx, jx];
                if (tile.type == TileType.DEATH) break;
            }

            tile.MakeGOLD();
            soundManager.JuwelPlay();
        }
    }

    public void MakeTile(Vector2Int center)
    {
        ReverseA(center);
    }

    public void LoadStageFromText()
    {
        string[] lines = stageFiles[Random.Range(0, stageFiles.Length)].text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        int columns = 5;
        int rows = 5;
        tileTable = new TileType[columns, rows];
        tileTableObj = new TileManager[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            string[] values = lines[y].Split(new[] { ',' });
            for (int x = 0; x < columns; x++)
            {
                if (values[x] == "0")
                {
                    tileTable[x, y] = TileType.DEATH;
                }
                if (values[x] == "1")
                {
                    tileTable[x, y] = TileType.ALIVE;
                }
                if (values[x] == "2")
                {
                    tileTable[x, y] = TileType.GOLD;
                }
            }
        }
    }

    public void ClickedTile(Vector2Int center)
    {
        if (isFever == false)
        { 
            ReverseTiles(center);
            soundManager.SlidePlay();
            Invoke("CheckClear", 0.05f);
        }
    }

    public void CheckClear()
    {
        if (IsClear())
        {
            Invoke("getCoin", 0.05f);
        }
    }

    void ReverseTiles(Vector2Int center)
    {
        Vector2Int[] around =
        {
            center + Vector2Int.up,
            center + Vector2Int.down,
            center + Vector2Int.right,
            center + Vector2Int.left,
        };
        foreach (Vector2Int position in around)
        {
            if (position.x < 0 || tileTableObj.GetLength(0) <= position.x)
            {
                continue;
            }
            if (position.y < 0 || tileTableObj.GetLength(1) <= position.y)
            {
                continue;
            }
            tileTableObj[position.x, position.y].ReverseTile(false);
        }
    }

    void ReverseA(Vector2Int center)
    {
        Vector2Int[] around =
    {
            center + Vector2Int.up,
            center + Vector2Int.down,
            center + Vector2Int.right,
            center + Vector2Int.left,
        };
        foreach (Vector2Int position in around)
        {
            if (position.x < 0 || tileTableObj.GetLength(0) <= position.x)
            {
                continue;
            }
            if (position.y < 0 || tileTableObj.GetLength(1) <= position.y)
            {
                continue;
            }
            tileTableObj[position.x, position.y].ReverseALIVE();
        }
    }


    bool IsClear()
    {
        for (int y = 0; y < tileTableObj.GetLength(1); y++)
        {
            for (int x = 0; x < tileTableObj.GetLength(0); x++)
            {
                if (tileTableObj[x, y].type == TileType.ALIVE)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ClearStage(float speed)
    {
        for (int y = 0; y < tileTableObj.GetLength(1); y++)
        {
            for (int x = 0; x < tileTableObj.GetLength(0); x++)
            {
                tileTableObj[x, y].ClearTile(speed);
            }
        }
    }

    public void DestroyStage(float DeTime)
    {
        for (int y = 0; y < tileTableObj.GetLength(1); y++)
        {
            for (int x = 0; x < tileTableObj.GetLength(0); x++)
            {
                Destroy(tileTableObj[x, y].gameObject, DeTime);
            }
        }
    }

    public void startFever()
    {
        isFever = true;
    }

    public void endFever()
    {
        isFever = false;
    }

    void DebugTable()
    {
        for (int y = 0; y < 5; y++)
        {
            string debugText = "";
            for (int x = 0; x < 5; x++)
            {
                debugText += tileTable[x, y] + ",";
            }
            Debug.Log(debugText);
        }
    }
}