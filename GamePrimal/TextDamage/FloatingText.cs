using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GamePrimal.TextDamage
{
    public class FloatingText : MonoBehaviour
    {
        public Animator AnimatorText;
        public Text damageText;

        private void Start()
        {
            AnimatorClipInfo[] aci = AnimatorText.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, aci[0].clip.length);

            damageText = AnimatorText.GetComponent<Text>();
        }

        public void SetText(string text)
        {
            damageText.text = text;
        }
    }
}
