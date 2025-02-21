using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Sprite tailSprite;
    
    private int fieldXPos = -1;
    private int fieldYPos = -1;

    private Block currentBlock;

    public void Init(LevelDate levelDate, BlockSkinDate blockSkinDate)
    {
        fieldXPos = levelDate.starPosX;
        fieldYPos = levelDate.startPosY;
        _spriteRenderer.sprite = blockSkinDate.mainSprite;
        tailSprite = blockSkinDate.tailSprite;
    }

    public void SetFieldPos(int x, int y)
    {
        fieldXPos = x;
        fieldYPos = y;
    }

    public Sprite GetTailSprite()
    {
        return tailSprite;
    }
    
    public int GetFieldXPos()
    {
        return fieldXPos;
    }

    public int GetFieldYPos()
    {
        return fieldYPos;
    }

    public Sprite GetMainSprite()
    {
        return _spriteRenderer.sprite;
    }
    
}
