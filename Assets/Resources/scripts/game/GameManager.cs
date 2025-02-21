using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //ONE_BLOCK_MOVE - player move only one block which side plyer choose
    //LINE_MOVE - player move in side which he choose,while he dont touch wall, field end or painted block
    public enum PlayMode
    {
        ONE_BLOCK_MOVE, LINE_MOVE
    }

    [SerializeField] private PlayMode currentMode;
    [SerializeField] private WinWindow winWindow;
    [SerializeField] private LoseWindow loseWindow;
    [SerializeField] private List<BlockSkinDate> _blockSkinDateList;
    [SerializeField] private List<LocationDate> _locationDateList;
    [SerializeField] private Image gameBackground;
    private LocationDate _locationDate;
    private PlayerInput _playerInput;
    private LevelDate _levelDate;
    private PlayerBlock mainPlayerBlock;
    [SerializeField] private Field _field;
    
    private List<PlayerMoveTurnDate> playerMoveList = new List<PlayerMoveTurnDate>();

    //count how many undo move can do player
    private int undoCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _locationDate = GetLocationDate();
        
        
        MusicPlayer.Instance.ChangeMusic(_locationDate.gameAudio);
        
        _levelDate = _locationDate.levelList[Mathf.Clamp(
            PrefsController.Instance.GetMaxCurrentLocationLevel(_locationDate.id),0,_locationDate.levelList.Count - 1)];
        gameBackground.sprite = _locationDate.background;
        

        
        
        BlockSkinDate blockSkinDate =
            _blockSkinDateList.Find(it => it.id == PrefsController.Instance.GetCurrentBlockSkin());
        if (blockSkinDate == null)
        {
            blockSkinDate = _blockSkinDateList[0];
        }
        
        mainPlayerBlock = _field.Init(_levelDate);
        mainPlayerBlock.Init(_levelDate,blockSkinDate);
        
        
        _playerInput = new DragPlayerInput();
        _playerInput.onMoveLeft = MoveLeft;
        _playerInput.onMoveRight = MoveRight;
        _playerInput.onMoveTop = MoveTop;
        _playerInput.onMoveDown = MoveDown;
        _playerInput.StartInput(this);
    }

    public void MovePlayerToPos(int x, int y,int moveXSide, int moveYSide,  PlayerMoveTurnDate playerMoveTurnDate)
    {
        Block blockToMove = _field.GetBlockInPos(x, y);
        if (blockToMove != null)
        {
            if (currentMode == PlayMode.ONE_BLOCK_MOVE)
            {
                if (blockToMove.CheckState(Block.BLOCK_TYPE.BLOCK))
                {
                    _field.MovePlayerBlock(mainPlayerBlock, x, y);
                }

                playerMoveList.Add(playerMoveTurnDate);
                undoCount = 1;
            }
            else
            {
                undoCount = 0;
                int newX = x;
                int newY = y;
                while (blockToMove != null && blockToMove.CheckState(Block.BLOCK_TYPE.BLOCK))
                {
                    _field.MovePlayerBlock(mainPlayerBlock, newX, newY);
                    
                    playerMoveList.Add(playerMoveTurnDate);
                    undoCount ++;
                    newX += moveXSide;
                    newY += moveYSide;
                    blockToMove = _field.GetBlockInPos(newX, newY);
                }
            }

            if (_field.IsAllBlockFilled())
            {
                //Open new level. If no more level set current level.
                int currentMaxLvl = PrefsController.Instance.GetMaxCurrentLocationLevel(_locationDate.id);
                if (currentMaxLvl < _locationDate.levelList.Count - 1)
                {
                    PrefsController.Instance.SetMaxCurrentLocationLevel(_locationDate.id,currentMaxLvl + 1);
                }
                
                //Evaluate win and show amount in win window
                PrefsController.Instance.SetCoinCount(PrefsController.Instance.GetCoinCount() + _locationDate.winAmount);
                winWindow.SetWinAmount(_locationDate.winAmount);
                
                Handheld.Vibrate();
                
                //Play win animation to all block, after animation end open win window
                Sequence sequence = DOTween.Sequence();
                sequence.Append(_field.AnimationBlockWin(mainPlayerBlock));
                sequence.AppendCallback(() =>
                {
                    winWindow.OpenWindow();
                });
                
                _playerInput.EndInput(this);
            }
            else if (!_field.HasPlayerMove(mainPlayerBlock))
            {
                Handheld.Vibrate();
                //Play lose animation to all block, after animation end open lose window
                Sequence sequence = DOTween.Sequence();
                sequence.Append(_field.AnimationFallAllBlock(mainPlayerBlock));
                sequence.AppendCallback(() =>
                {
                    loseWindow.OpenWindow();
                });
                _playerInput.EndInput(this);
            }
        }
    }

    public void UndoMove()
    {
        if (currentMode == PlayMode.ONE_BLOCK_MOVE)
        {
            if (undoCount > 0 && playerMoveList.Count > 0)
            {
                PlayerMoveTurnDate playerMoveTurnDate = playerMoveList[^1];
                undoCount--;
                _field.UndoMovePlayerBlock(mainPlayerBlock,
                    mainPlayerBlock.GetFieldXPos() + (playerMoveTurnDate.X * -1),
                    mainPlayerBlock.GetFieldYPos() + (playerMoveTurnDate.Y * -1));
            }
        }
        else
        {
            while (undoCount > 0 && playerMoveList.Count > 0)
            {
                PlayerMoveTurnDate playerMoveTurnDate = playerMoveList[^1];
                undoCount--;
                _field.UndoMovePlayerBlock(mainPlayerBlock,
                    mainPlayerBlock.GetFieldXPos() + (playerMoveTurnDate.X * -1),
                    mainPlayerBlock.GetFieldYPos() + (playerMoveTurnDate.Y * -1));
            } 
        }
    }

    public void RestartLvl()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void NextLevel()
    {


        SceneManager.LoadScene("GameScene");
    }
    
    private void MoveLeft()
    {
        PlayerMoveTurnDate playerMoveTurnDate = new PlayerMoveTurnDate();
        playerMoveTurnDate.X = -1;
        MovePlayerToPos(mainPlayerBlock.GetFieldXPos() - 1,mainPlayerBlock.GetFieldYPos(),-1,0,playerMoveTurnDate);
    }
    private void MoveRight()
    {
        PlayerMoveTurnDate playerMoveTurnDate = new PlayerMoveTurnDate();
        playerMoveTurnDate.X = 1;
        MovePlayerToPos(mainPlayerBlock.GetFieldXPos() + 1,mainPlayerBlock.GetFieldYPos(),1,0,playerMoveTurnDate);
    }
    private void MoveTop()
    {
        PlayerMoveTurnDate playerMoveTurnDate = new PlayerMoveTurnDate();
        playerMoveTurnDate.Y = 1;
        MovePlayerToPos(mainPlayerBlock.GetFieldXPos(),mainPlayerBlock.GetFieldYPos() + 1,0,1,playerMoveTurnDate);
    }
    private void MoveDown()
    {
        PlayerMoveTurnDate playerMoveTurnDate = new PlayerMoveTurnDate();
        playerMoveTurnDate.Y = -1;
        MovePlayerToPos(mainPlayerBlock.GetFieldXPos(),mainPlayerBlock.GetFieldYPos() - 1,0,-1,playerMoveTurnDate);
    }


    private LocationDate GetLocationDate()
    {
        LocationDate locationDate = _locationDateList.Find(it => it.id == PrefsController.Instance.GetCurrentLocation());
        if (locationDate == null)
        {
            locationDate = _locationDateList[0];
        }

        return locationDate;
    }
}
