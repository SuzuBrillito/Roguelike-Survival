using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyObject : CellObject
{
    public int Health = 3;

    private int m_CurrentHealth;

    public void Awake()
    {
        GameManager.Instance.turnManager.OnTick += EnemyTurnHappen;
    }

    private void OnDestroy()
    {
        GameManager.Instance.turnManager.OnTick -= EnemyTurnHappen;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHealth -= 1;
        if (m_CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
        GameManager.Instance.playerManager.Animator.SetTrigger("Attack");
        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.boardManager;
        var targetCell =board.GetCellData(coord);

        if (targetCell == null 
            || !targetCell.pasable
            || targetCell.ContainObject != null)
        {
            return false;
        }
        //desocupa la casilla actual
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainObject = null;

        //añadirlo a otra casilla
        targetCell.ContainObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);
        return true;
    }

    void EnemyTurnHappen()
    {
        //buscar la posicion actual del player
        var playerCell = GameManager.Instance.playerManager.CellPosition;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.FoodChange(-3);
            GameManager.Instance.playerManager.Animator.SetTrigger("Damage");
        }

        //logica para ver si me muevo der izq arriba o abajo

        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    //si no puedo moverme en x (ni atacar) entonces me muevo en la y
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
            
        }
    }

    //metodo para mover en x

    bool TryMoveInX (int xDist)
    {
        //player a la derecha
        if (xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }

        //player a la izq
        return MoveTo(m_Cell + Vector2Int.left);
    }

    //metodo para mover en y

    bool TryMoveInY(int yDist)
    {
        //player arriba
        if (yDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.up);
        }

        //player abajo
        return MoveTo(m_Cell + Vector2Int.down);
    }
}
