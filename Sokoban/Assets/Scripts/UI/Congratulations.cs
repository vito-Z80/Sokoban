using System.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class Congratulations : MonoBehaviour
    {
        [SerializeField] GameObject congratulationsText;
        
        public async Task<bool> CongratulationsTask()
        {
            return await Global.Instance.alphaScreen.Fade(o => { congratulationsText.SetActive(true); });
        }
    }
}