using UnityEngine;
using DG.Tweening;


public enum TileType
{
    DEATH,
    ALIVE,
    GOLD,
}

public class TileManager : MonoBehaviour
{
    public TileType type;
    public Sprite deathSprite;
    public Sprite aliveSprite;
    public Sprite goldSprite;

    SpriteRenderer spriteRenderer;
    Vector2Int intPosition;

    public delegate void Clicked(Vector2Int center);
    public Clicked clicked;

    public delegate void MakeTurn(Vector2Int center);
    public MakeTurn makeTurn;

    public delegate void Bonused();
    public Bonused bonused;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.DOMoveX(10f, 0f).SetRelative(true);
    }

    public void SetInit(TileType tileType, Vector2Int position, bool isFever)
    {
        intPosition = position;
        SetType(tileType);

        if (isFever == false)
        {
            transform.DOMoveX(-10f, 0.4f).SetRelative(true);
        }
        else
        {
            transform.DOMoveX(-10f, 0f).SetRelative(true);
        }
    }

    void SetType(TileType tileType)
    {
        type = tileType;
        SetImage(tileType);
    }

    void SetImage(TileType tileType)
    {
        if (type == TileType.DEATH)
        {
            spriteRenderer.sprite = deathSprite;
        }
        else if (type == TileType.ALIVE)
        {
            spriteRenderer.sprite = aliveSprite;
        }
        else if (type == TileType.GOLD)
        {
            spriteRenderer.sprite = goldSprite;
        }
    }

    public void OnTile()
    {
        if (type == TileType.DEATH || type == TileType.ALIVE)
        {
            ReverseTile(true);
            clicked(intPosition);
        }
        else if (type == TileType.GOLD)
        {
            ReverseTile(true);
            bonused();
        }
    }

    public void Make()
    {
        ReverseALIVE();
        makeTurn(intPosition);
    }

    public void ReverseALIVE()
    {
        if (type == TileType.DEATH)
        {
            SetType(TileType.ALIVE);
        }
        else if (type == TileType.ALIVE)
        {
            SetType(TileType.DEATH);
        }
    }

    public void MakeGOLD()
    {
        SetType(TileType.GOLD);
    }

    public void ReverseTile(bool F)
    {

        if (type == TileType.DEATH)
        {
            SetType(TileType.ALIVE);
        }
        else if (type == TileType.ALIVE)
        {
            SetType(TileType.DEATH);
        }
        else if (type == TileType.GOLD)
        {
            if (F == true)
            { 
                SetType(TileType.DEATH);
            }
            else if(F == false)
            {
                SetType(TileType.ALIVE);
            }
        }

        transform.DORotate(Vector3.up * -90f, 0f);
        transform.DORotate(Vector3.up * 0f, 0.1f);
    }

    public void ClearTile(float time)
    {
        transform.DOMoveY(-10f, time).SetRelative(true);
    }

    public void AllAliveTile()
    {
        SetType(TileType.DEATH);
    }
}