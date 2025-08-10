using DG.Tweening;
using UnityEngine;

namespace PSW.Cord.Ui
{
    public class ButtonManager : MonoBehaviour
    {
        public void UpScale(Transform trm) => trm.DOScale(new Vector2(1.2f, 1.2f), 0.2f);
        public void DownScale(Transform trm) => trm.DOScale(new Vector2(1f, 1f), 0.2f);
        public void EndGame() => Application.Quit();
    }
}