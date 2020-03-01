using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.TeamProjects.HudInterface.MainMenu
{
    public class MenuBulletCollision : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Color ColorInitial = Color.white;
        public Color ColorToward = Color.gray;
        public float Position;
        public MenuButtonStates ButtonPurpose = MenuButtonStates.None;

        private TextMeshProUGUI _tmp;
        private RectTransform _rt;
        private Vector3 _initialScale;

        public bool PickRightNow { get; private set; } = false;

        private void Start()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _rt = GetComponent<RectTransform>();
            _initialScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tmp.color = ColorToward;
            _rt.localScale = _rt.localScale * 1.2f;
            PickRightNow = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tmp.color = ColorInitial;
            _rt.localScale = _initialScale;
            PickRightNow = true;
        }
    }
}
