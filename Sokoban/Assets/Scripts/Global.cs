using System;
using Data;
using UnityEngine;
using UnityEngine.Rendering;

public class Global : MonoBehaviour
{
    static Global m_instance;

    public static Global Instance => m_instance;


    public GameState gameState;
    public GameSettings gameSettings;
    public LevelPhase levelPhase;
    public InputSystemActions input;

    void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
            input ??= new InputSystemActions();
            input.Enable();
        }
    }
    


    void OnDisable()
    {
        input.Disable();
    }
}