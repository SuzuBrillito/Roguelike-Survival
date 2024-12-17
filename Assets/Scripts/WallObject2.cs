using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject2 : CellObject
{
    public Tile Obstacle2Tile;
    public int maxHealth = 5;
    private int m_HealthPoint;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoint = maxHealth;
        m_OriginalTile = GameManager.Instance.boardManager.GetCellTile(cell);
        GameManager.Instance.boardManager.SetCellTile(cell, Obstacle2Tile);
    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoint -= 1;
        GameManager.Instance.playerManager.Animator.SetTrigger("Attack");
        if (m_HealthPoint > 0)
        {
            return false;
        }
        GameManager.Instance.boardManager.SetCellTile(m_Cell, m_OriginalTile);
        
        Destroy(gameObject);
        return true;
    }



   
}
