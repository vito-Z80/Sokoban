﻿
using Cysharp.Threading.Tasks;
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


    public async UniTask Run()
    {
        var globalGameSpeed = Global.Instance.gameSpeed;
        Global.Instance.gameSpeed = 1.0f;
        
        //  Посмотреть на игрока.
        //  Открыть дверь.
        //  Подойти к двери + слежение камеры за ГГ.
        await m_character.LookBackAnimation();

        m_character.SetAutoMove(m_character.transform.position + m_character.transform.forward, m_character.transform.forward);

        m_cameraManager.SetCameraState(CameraManager.State.FollowPath);
        while (m_character.IsMoving())
        {
            await UniTask.Yield();
        }
        
        var toDoorPosition = m_levelZero.exitDoor.transform.position + Vector3.down * 0.5f;
        var forward = m_levelZero.exitDoor.transform.forward;

        m_character.SetAutoMove(toDoorPosition, forward);
        m_levelZero.OpenDoor();

        while (m_cameraManager.GetCameraState() == CameraManager.State.FollowPath)
        {
            await UniTask.Yield();
        }
        
        Global.Instance.gameSpeed = globalGameSpeed;
        // m_character.Freezed = false;
        // m_cameraManager.SetCameraState(CameraManager.State.FollowPath);
        
    }
}