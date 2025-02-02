using System;
using Data;
using UnityEngine;

public class Global : MonoBehaviour
{
    static Global m_instance;

    public static Global Instance => m_instance;


    [Header("Variables")] 
    public float gameSpeed = 1.0f;
    public bool isParticleEnable;
    
    
    [Header("Data")]
    public GameState gameState;
    public GameSettings gameSettings;
    public LevelPhase levelPhase;
    public InputSystemActions input;
    

    
    [Header("Materials")]
    public Material transportSystemMaterial;
    
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

    Vector2 uvSpeed = new Vector2(0.0f, 8.0f);
    Vector2 uvOffset;
    void Update()
    {
        if (transportSystemMaterial != null)
        {
            uvOffset += uvSpeed * Time.deltaTime;
            transportSystemMaterial.SetTextureOffset("_BaseMap", uvOffset);
        }
    }

    void OnDisable()
    {
        input.Disable();
    }
}