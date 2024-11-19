using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    public void Spawn (BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager; 
        MoveTo(cell);

        //el grid tiene un metodo para saber la posicion de una casilla, GetCellCenterWorld, pero esa informacion la tiene el BoardManager
        //el boardmanager deberia ser la clase que lidia con toda la info relacionada al tablero, como convertir una casilla a una posicion del mundo
        //para ello necesitamos una referencia al grid, para poder usar GetCellCenterWorld, coger el componente con un getcomponent, y añadir un metodo que convierta
        //el indice 2d de una celda ydevuelva una posicion del mundo del centro de esa celda

        //mueve el pj a el metodo que hemos hecho que es un vector 3 (la informacion que le damos es cell, que es la casilla)

        
    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    private void Update ()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
            //GameManager.Instance.m_turnManager.Tick();
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            //comprueba si la nueva posicion es pasable, y muevela si lo es
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.pasable)
            {
                MoveTo(newCellTarget);
            }
        }

    }

}
