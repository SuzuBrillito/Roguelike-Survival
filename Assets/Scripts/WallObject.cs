using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public int maxHealth = 3;
    private int m_HealthPoint;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoint = maxHealth;
        m_OriginalTile = GameManager.Instance.boardManager.GetCellTile(cell);
        GameManager.Instance.boardManager.SetCellTile(cell, ObstacleTile);
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



    //crear variable que cuente la vida del muro
    //guardar el tile original de esa casilla cuando se inicialice antes de reemplazarlo con el muro
    //el jugador entre en el muro se reduce la vida en 1, y cuando sea 0 se destruye el muro y devuelve el tile original
    
    //varios muros con distintas vidas

    //cambiar el tile cuando le quede 1 hp a uno dañado
}
