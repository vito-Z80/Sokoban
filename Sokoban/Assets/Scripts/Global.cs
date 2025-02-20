using Data;
using Level;
using UI;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Global : MonoBehaviour
{
    static Global m_instance;

    public static Global Instance => m_instance;


    [Header("Variables")] public float gameSpeed = 1.0f;
    public bool isParticleEnable;


    [Header("Data")] public GameState gameState;
    public LevelPhase levelPhase;
    public InputSystemActions input;
    public GameMode gameMode;
    public LevelManager levelManager;
    public Assembler character;
    public CameraManager cameraManager;
    public AlphaScreen alphaScreen;
    
    [Header("Audio clips")]
    [SerializeField] public AudioClip teleportSound;
    [SerializeField] public AudioClip teleportErrorSound;
    [SerializeField] public AudioClip openDoorSound;
    [SerializeField] public AudioClip closeDoorSound;
    [SerializeField] public AudioClip boxOnPointSound;
    [SerializeField] public AudioClip turnstileSound;
    [SerializeField] public AudioClip buttonDownSound;
    [SerializeField] public AudioClip buttonUpSound;
    
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