using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BLOCK_TYPE
    {
        BLOCK,WALL,PLAYER_TAIL,
    }
    
    [SerializeField] private SpriteRenderer mainSprite;

    private Sprite _blockSprite;
    private BLOCK_TYPE _type = BLOCK_TYPE.BLOCK;


    public void Init(BLOCK_TYPE type, Sprite blockSprite)
    {
        _type = type;
        _blockSprite = blockSprite;
        mainSprite.sprite = _blockSprite;
    }

    public void ResetBlock()
    {
        if (_blockSprite != null)
        {
            mainSprite.sprite = _blockSprite;
            if (_type == BLOCK_TYPE.PLAYER_TAIL)
            {
                _type = BLOCK_TYPE.BLOCK;
            }
        }
    }

    public void TransformToTail(Sprite tailSprite)
    {
        mainSprite.sprite = tailSprite;
        _type = BLOCK_TYPE.PLAYER_TAIL;
    }

    public bool CheckState(BLOCK_TYPE blockType)
    {
        return _type == blockType;
    }
}
