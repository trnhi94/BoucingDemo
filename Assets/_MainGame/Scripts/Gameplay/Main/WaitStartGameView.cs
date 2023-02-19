using TMPro;
using UnityEngine;

namespace _MainGame.Scripts.Gameplay.Main
{
    public class WaitStartGameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text delayTxt;

        public void SetDelay(float remainTime)
        {
            delayTxt.SetText(remainTime.ToString("N0"));
        }
        //
        // public void SetTxt(string txt)
        // {
        //     delayTxt.SetText(txt);
        // }
    }
}