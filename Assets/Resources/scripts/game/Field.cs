using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private PlayerBlock _playerBlockPrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private float maxScaleValue = 0.25f;
    private Block[,] blockList = new Block[1,1];

    private float currentBlockScale = 0.25f;
    private float cellSize = 3f;
    private Vector2 topLeftCellPos = new Vector2();
    
    
    public PlayerBlock Init(LevelDate levelDate)
    {
        //Evaluate field parameter
        currentBlockScale = Mathf.Clamp(1f / levelDate.columns,0.01f,maxScaleValue);
        cellSize = 4f * currentBlockScale;
        topLeftCellPos.x = -(cellSize * ((levelDate.columns / 2f) - 0.5f));
        topLeftCellPos.y = (cellSize * ((levelDate.rows / 2f) + 0.5f));
        
        //Create blocks
        blockList = new Block[levelDate.columns, levelDate.rows];
        for (int x = 0; x < levelDate.columns; x++)
        {
            for (int y = 0; y < levelDate.rows; y++)
            {
                if (levelDate.GetState(x, y) != LevelDate.DrawBlockType.NONE)
                {
                    Block createdBlock = Instantiate(_blockPrefab, transform);

                    switch (levelDate.GetState(x, y))
                    {
                        case LevelDate.DrawBlockType.PLAYER_START_POSITION:
                        {
                            createdBlock.Init(Block.BLOCK_TYPE.BLOCK,levelDate.block);
                            break;
                        }
                        case LevelDate.DrawBlockType.BLOCK:
                        {
                            createdBlock.Init(Block.BLOCK_TYPE.BLOCK,levelDate.block);
                            break;
                        }
                        case LevelDate.DrawBlockType.BARRIER:
                        {
                            createdBlock.Init(Block.BLOCK_TYPE.WALL,levelDate.wall);
                            break;
                        }
                    }

                    createdBlock.transform.localPosition = new Vector3(
                        topLeftCellPos.x + cellSize * x,
                        topLeftCellPos.y - cellSize * y
                    );
                    createdBlock.transform.localScale = new Vector3(0, 0, 1f);
                    createdBlock.transform.DOScale(new Vector3(currentBlockScale, currentBlockScale), 0.5f);
                    // createdBlock.transform.localScale = new Vector3(currentBlockScale, currentBlockScale, 1f);
                    blockList[x, y] = createdBlock;
                }
            }
        }

        PlayerBlock playerBlock = Instantiate(_playerBlockPrefab, transform);
        playerBlock.transform.localPosition = new Vector3(
            topLeftCellPos.x + cellSize * levelDate.starPosX,
            topLeftCellPos.y - cellSize * levelDate.startPosY,
            -2f
        );
        playerBlock.transform.localScale = new Vector3(currentBlockScale, currentBlockScale, 1f);

        return playerBlock;
    }

    public bool IsAllBlockFilled()
    {
        for (int x = 0; x < blockList.GetLength(0); x++)
        {
            for (int y = 0; y < blockList.GetLength(1); y++)
            {
                if (blockList[x,y] != null && blockList[x,y].CheckState(Block.BLOCK_TYPE.BLOCK))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool HasPlayerMove(PlayerBlock playerBlock)
    {
        int playerPosX = playerBlock.GetFieldXPos();
        int playerPosY = playerBlock.GetFieldYPos();
        Block topBlock = GetBlockInPos(playerPosX, playerPosY + 1);
        Block downBlock = GetBlockInPos(playerPosX, playerPosY - 1);
        Block leftBlock = GetBlockInPos(playerPosX - 1, playerPosY);
        Block rightBlock = GetBlockInPos(playerPosX + 1, playerPosY);

        return ((topBlock != null && topBlock.CheckState(Block.BLOCK_TYPE.BLOCK)) ||
                (downBlock != null && downBlock.CheckState(Block.BLOCK_TYPE.BLOCK)) ||
                (leftBlock != null && leftBlock.CheckState(Block.BLOCK_TYPE.BLOCK)) ||
                (rightBlock != null && rightBlock.CheckState(Block.BLOCK_TYPE.BLOCK)));
    }
    
    public void MovePlayerBlock(PlayerBlock playerBlock, int newX, int newY)
    { 
        playerBlock.transform.DOLocalMove(new Vector3(
            topLeftCellPos.x + cellSize * newX,
            topLeftCellPos.y - cellSize * newY,
            -2f
        ),0.25f);
        Block currentBlock = GetBlockInPos(playerBlock.GetFieldXPos(), playerBlock.GetFieldYPos());
        currentBlock.TransformToTail(playerBlock.GetTailSprite());
        Block nextBlock = GetBlockInPos(newX, newY);
        nextBlock.TransformToTail(playerBlock.GetTailSprite());
        playerBlock.SetFieldPos(newX,newY);
    }
    public void UndoMovePlayerBlock(PlayerBlock playerBlock, int previsionX, int previsionY)
    { 
        playerBlock.transform.localPosition = new Vector3(
            topLeftCellPos.x + cellSize * previsionX,
            topLeftCellPos.y - cellSize * previsionY,
            -2f
        );
        Block currentBlock = GetBlockInPos(playerBlock.GetFieldXPos(), playerBlock.GetFieldYPos());
        currentBlock.ResetBlock();
        playerBlock.SetFieldPos(previsionX,previsionY);
    }
    public Block GetBlockInPos(int x, int y)
    {
        if (x < 0 || x >= blockList.GetLength(0) || y < 0 || y >= blockList.GetLength(1))
        {
           return null; 
        }

        return blockList[x, y];
    }

    public Sequence AnimationFallAllBlock(PlayerBlock playerBlock)
    {
        Sequence sequence = DOTween.Sequence();
        for (int x = 0; x < blockList.GetLength(0); x++)
        {
            for (int y = 0; y < blockList.GetLength(1); y++)
            {
                if (blockList[x,y] != null )
                {
                    Transform blockTransform = blockList[x, y].transform;
                    Vector3 targetPosition = new Vector3(blockTransform.localPosition.x, blockTransform.localPosition.y - 8f);
                
                    sequence.Join(blockTransform.DOLocalMove(targetPosition, 1f).SetEase(Ease.InBack)); // Падіння
                    sequence.Join(blockTransform.DORotate(new Vector3(0, 0, Random.Range(-90f, 90f)), 1f, RotateMode.FastBeyond360));

                }
            }
        }
        Transform playerTransform = playerBlock.transform;
        Vector3 targetPlayerPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y - 8f);
                
        sequence.Join(playerTransform.DOLocalMove(targetPlayerPosition, 1f).SetEase(Ease.InBack)); // Падіння
        sequence.Join(playerTransform.DORotate(new Vector3(0, 0, Random.Range(-90f, 90f)), 1f, RotateMode.FastBeyond360));


        return sequence;
    }
    public Sequence AnimationBlockWin(PlayerBlock playerBlock)
    {
        Sequence sequence = DOTween.Sequence();
        for (int x = 0; x < blockList.GetLength(0); x++)
        {
            for (int y = 0; y < blockList.GetLength(1); y++)
            {
                if (blockList[x, y] == null)
                {
                    Block createdBlock = Instantiate(_blockPrefab, transform);
                    createdBlock.transform.localPosition = new Vector3(
                        topLeftCellPos.x + cellSize * x,
                        topLeftCellPos.y - cellSize * y
                    );
                    createdBlock.transform.localScale = new Vector3(currentBlockScale, currentBlockScale, 1f);
                    blockList[x, y] = createdBlock;
                }

                Sequence blockSequence = DOTween.Sequence();
                Transform blockTransform = blockList[x, y].transform;

                float distance = Vector2.Distance(blockTransform.localPosition, playerBlock.transform.localPosition); // Обчислюємо відстань
                float delay = distance * 0.15f; // Чим далі — тим більша затримка

                Block block = blockList[x, y];
                blockSequence.AppendInterval(delay);
                blockSequence.AppendCallback(() =>
                {
                    block.TransformToTail(playerBlock.GetMainSprite());
                });
                blockSequence.AppendInterval(0.5f);
                blockSequence.Join(blockList[x, y].transform.DOScale(new Vector3(currentBlockScale * 1.5f, currentBlockScale * 1.5f), 0.5f));
                blockSequence.Append(blockList[x, y].transform.DOScale(new Vector3(0f, 0f), 0.7f));
                sequence.Join(blockSequence);
                // blockSequence.AppendInterval();
                //
                // sequence.Join(blockTransform.DOLocalMove(targetPosition, 1f).SetEase(Ease.InBack)); // Падіння
                // sequence.Join(blockTransform.DORotate(new Vector3(0, 0, Random.Range(-90f, 90f)), 1f, RotateMode.FastBeyond360));
            }
        }
        Sequence playerSequence = DOTween.Sequence();
        
        playerSequence.AppendInterval(0f);
        playerSequence.AppendInterval(0.5f);
        playerSequence.Join(playerBlock.transform.DOScale(new Vector3(currentBlockScale * 1.5f, currentBlockScale * 1.5f), 0.5f));
        playerSequence.Append(playerBlock.transform.DOScale(new Vector3(0f, 0f), 0.7f));
        sequence.Join(playerSequence);
        return sequence;
    }

}
