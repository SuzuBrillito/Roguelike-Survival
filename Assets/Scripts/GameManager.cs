using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerManager playerManager;
    public TurnManager turnManager { get; private set; }

    private int m_comida = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;

    

    public int currentLevel = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;
        //Debug.Log("Comida actual: " + m_comida);

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

        StartNewGame();
        
        
    }

    public void OnTurnHappen()
    {
        FoodChange(-1);
    }

    public void FoodChange(int amount)
    {
        m_comida += amount;
        m_FoodLabel.text = "Comida: " + m_comida;

        if (m_comida <= 0)
        {
            playerManager.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!! \n\nHas avanzado a través de " + currentLevel + " niveles";
            
            
        }
    }

    

    public void NuevoNivel()
    {
        boardManager.Limpiar();
        boardManager.Init();
        playerManager.Spawn(boardManager, new Vector2Int(1, 1));

        currentLevel++;
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility= Visibility.Hidden;
        currentLevel = 1;
        m_comida = 60;
        m_FoodLabel.text = "Comida: " + m_comida;

        boardManager.Limpiar();
        boardManager.Init();
        playerManager.Init();
        playerManager.Spawn(boardManager, new Vector2Int(1, 1));
        
    }
}
