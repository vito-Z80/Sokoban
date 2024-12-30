using System.Threading.Tasks;
using Level;
using UnityEngine;

public class StartGame
{
    readonly LevelZero m_levelZero;
    readonly Assembler m_character;
    readonly CameraManager m_cameraManager;

    public StartGame(Assembler character, LevelZero level, CameraManager cameraManager)
    {
        m_character = character;
        m_levelZero = level;
        m_cameraManager = cameraManager;
    }


    public async Task Run()
    {
        //  Посмотреть на игрока.
        //  Открыть дверь.
        //  Подойти к двери + слежение камеры за ГГ.
        Debug.Log("Starting animation");
        await m_character.LookBackAnimation();

        Debug.Log("Ending animation");

        m_character.SetAutoMove(m_character.transform.position + m_character.transform.forward, m_character.transform.forward);

        m_cameraManager.SetCameraState(CameraManager.State.FollowPath);
        while (m_character.IsMoving())
        {
            await Task.Yield();
        }
        
        var toDoorPosition = m_levelZero.exitDoor.transform.position + Vector3.down * 0.5f;
        var forward = m_levelZero.exitDoor.transform.forward;

        m_character.SetAutoMove(toDoorPosition, forward);
        m_levelZero.OpenDoor();

        while (m_cameraManager.GetCameraState() == CameraManager.State.FollowPath)
        {
            await Task.Yield();
        }
        
        m_cameraManager.SetCameraState(CameraManager.State.FollowPath);

        
    }
}