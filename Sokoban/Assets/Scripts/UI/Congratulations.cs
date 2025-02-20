
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class Congratulations : MonoBehaviour
    {
        [SerializeField] GameObject congratulationsText;
        
        public async UniTask<bool> CongratulationsTask()
        {
            return await Global.Instance.alphaScreen.Fade(o => { congratulationsText.SetActive(true); });
        }
    }
}