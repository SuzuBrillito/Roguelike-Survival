using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //una bool para saber si me muevo o no
    //un vector3 el objetivo al que nos ,ovemos y un float para la velocidad
    //que en el update si la bool de movimiento es true, que vaya moviendo al pj poco a poco a su objetivo, y cuando llegue
    //velva a poner la bool en false, y que entre en la casilla

    private bool m_isMoving;
    public float MoveSpeed = 5f;
    private Vector3 m_MoveTarget;
    
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private bool m_IsGameOver;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Init()
    {
        m_IsGameOver = false;
        m_isMoving = false;
    }

    public void Spawn (BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager; 
        MoveTo(cell, true);

        //el grid tiene un metodo para saber la posicion de una casilla, GetCellCenterWorld, pero esa informacion la tiene el BoardManager
        //el boardmanager deberia ser la clase que lidia con toda la info relacionada al tablero, como convertir una casilla a una posicion del mundo
        //para ello necesitamos una referencia al grid, para poder usar GetCellCenterWorld, coger el componente con un getcomponent, y añadir un metodo que convierta
        //el indice 2d de una celda ydevuelva una posicion del mundo del centro de esa celda

        //mueve el pj a el metodo que hemos hecho que es un vector 3 (la informacion que le damos es cell, que es la casilla)

        
    }

    public void MoveTo(Vector2Int cell, bool inmediate)
    {
        m_CellPosition = cell;

        if(inmediate)
        {
            m_isMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_isMoving = true;
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }
       m_Animator.SetBool("Moving", m_isMoving);

    }
    public void GameOver()
    {
        m_IsGameOver = true;
        
    }
    private void Update ()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);

            if (transform.position == m_MoveTarget)
            {
                m_isMoving = false;
                m_Animator.SetBool("Moving", false);
                var cellData = m_Board.GetCellData(m_CellPosition);
                if (cellData.ContainObject != null) cellData.ContainObject.PlayerEntered();
            }
            return;
        }
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
                GameManager.Instance.turnManager.Tick();

                if (cellData.ContainObject == null)
                {
                    MoveTo(newCellTarget, false);
                }
                else if(cellData.ContainObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false);
                    cellData.ContainObject.PlayerEntered();
                }


                

                
            }
        }

    }

}
