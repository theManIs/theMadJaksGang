using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBulletCollision : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color ColorInitial = Color.white;
    public Color ColorToward = Color.gray;

    private TextMeshProUGUI _tmp;
    private RectTransform _rt;

    private void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _rt = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tmp.color = ColorToward;
       _rt.localScale = _rt.localScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tmp.color = ColorInitial;
        _rt.localScale = Vector3.one;
    }
}
