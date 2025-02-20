using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClassicLevels
{
    public class ClassicLevelManager : MonoBehaviour
    {
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Assembler character;
        [SerializeField] Transform levelTransform;
        [SerializeField] Transform pointsTransform;
        [SerializeField] Transform boxedTransform;

        ClassicLevelBuilder m_classicLevelBuilder;


        void Start()
        {
            m_classicLevelBuilder = new ClassicLevelBuilder(cameraManager, character, pointsTransform, boxedTransform, levelTransform);
        }


        void Update()
        {
            
        }


        bool IsLevelCompleted()
        {
            if (m_classicLevelBuilder.boxContainers != null)
            {
                if (m_classicLevelBuilder.boxContainers.All(container => container.GetContact()))
                {
                    foreach (var container in m_classicLevelBuilder.boxContainers)
                    {
                        container.PlayEffectWhirlCube();
                    }

                    foreach (var box in m_classicLevelBuilder.boxes)
                    {
                        box.Freezed = true;
                    }

                    return true;
                }
            }

            return false;
        }


        public async UniTask StartClassicGame(int levelIndex)
        {
            if (await m_classicLevelBuilder.NextLevel(levelIndex))
            {
                Debug.Log($"Classic level {levelIndex} was successfully loaded");
            }
            else
            {
                //  TODO больше нет уровней.
            }
        }
    }
}