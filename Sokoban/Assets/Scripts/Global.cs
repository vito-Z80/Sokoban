using Data;
using UnityEngine;

public class Global : MonoBehaviour
{
    static Global m_instance;

    public static Global Instance => m_instance;


    public GameState gameState;
    public GameSettings gameSettings;
    public LevelPhase levelPhase;
    
    void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }
}