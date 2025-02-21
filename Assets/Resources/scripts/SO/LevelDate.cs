using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level/LevelDate")]
public class LevelDate : ScriptableObject
{    public enum DrawBlockType
    {
        NONE, BLOCK, BARRIER, PLAYER_START_POSITION
    }
    public Sprite background;
    public Sprite block;
    public Sprite wall;
    
    public int[] _flatStates;

    public DrawBlockType drawType = DrawBlockType.BLOCK;
    public int starPosX = -1;
    public int startPosY = -1;

    public int currentRows = 5;
    public int currentColumns = 5;
    
    public int rows = 5;   // Кількість рядків
    public int columns = 5;
    
    public void OnValidate()
    {
        // Синхронізація розміру масиву станів
        if (_flatStates == null || _flatStates.Length != rows * columns || currentRows != rows || currentColumns != columns)
        {
            currentRows = rows;
            currentColumns = columns;
            
            _flatStates = new int[rows * columns];
        }
    }

    public DrawBlockType GetState(int x, int y)
    {
        if (x < 0 || y < 0 || (x * y) >= _flatStates.Length)
        {
            return DrawBlockType.NONE; // або інше значення за замовчуванням
        }
        int intState = _flatStates[y * columns + x];
        if (x == starPosX && y == startPosY)
        {
            return DrawBlockType.PLAYER_START_POSITION;
        }
        if (intState == 1)
        {
            return DrawBlockType.BLOCK;
        }
        
        if (intState == 2)
        {
            return DrawBlockType.BARRIER;
        }
        return DrawBlockType.NONE;
    }

    public void SetState(int x, int y, DrawBlockType value)
    {
        switch (drawType)
        {
            case DrawBlockType.NONE:
            {
                _flatStates[y * columns + x] = 0;
                break;
            }
            case DrawBlockType.BLOCK:
            {
                _flatStates[y * columns + x] = 1;
                break;
            }
            case DrawBlockType.BARRIER:
            {
                _flatStates[y * columns + x] = 2;
                break;
            }
            case DrawBlockType.PLAYER_START_POSITION:
            {
                starPosX = x;
                startPosY = y;
                break;
            }
        }
        
    }
}
