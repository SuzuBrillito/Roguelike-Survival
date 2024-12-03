using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        NuevoNivel();
        
        
    }

    public void OnTurnHappen()
    {
        FoodChange(-1);
    }

    public void FoodChange(int amount)
    {
        m_comida += amount;
        m_FoodLabel.text = "Comida: " + m_comida;
    }

    public void NuevoNivel()
    {
        boardManager.Limpiar();
        boardManager.Init();
        playerManager.Spawn(boardManager, new Vector2Int(1, 1));

        currentLevel++;
    }
}
