using TMPro;
using UnityEngine;

namespace LMS.Battle {
    [RequireComponent(typeof(Animator))]
    public class TextDamageObject : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro TextMesh;
        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        public void ShowText(string text)
        {
            ShowText(text, Color.red);
        }

        public void ShowText(string text, Color32 color)
        {
            TextMesh.transform.localScale = Vector3.zero;
            TextMesh.text = text;
            TextMesh.color = color;
            animator.Play("Show");
        }

        public void OnEnd()
        {
            BattleManager.DestroyGameObject(gameObject);
        }

    }

}