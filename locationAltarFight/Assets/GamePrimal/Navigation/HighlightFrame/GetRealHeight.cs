using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRealHeight : MonoBehaviour
{
    public GameObject FrameHighlighterGameObject;
    private MeshRenderer m_FrameMeshRenderer;
    private GameObject m_LocalInstance;
    private SpriteRenderer m_LocalSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_LocalInstance = Instantiate(FrameHighlighterGameObject, transform.parent.transform);
        m_LocalSpriteRenderer = m_LocalInstance.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        TraceMousePosition();;
    }
    
    void HighlightTheTarget(GameObject targetGameObject)
    {
        m_FrameMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
        Vector3 pos = targetGameObject.transform.position;
            
        //pos.x = m_FrameMeshRenderer.bounds.center.x;
        pos.y = m_FrameMeshRenderer.bounds.min.y + m_FrameMeshRenderer.bounds.size.y * .2f;
        //pos.z = m_FrameMeshRenderer.bounds.center.z;

        m_LocalInstance.transform.position = pos;
        m_LocalSpriteRenderer.enabled = true;
    }

    void TraceMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Targetable")
                HighlightTheTarget(hit.collider.gameObject);
            else
                RemoveHighlight();
        }
    }

    private void RemoveHighlight()
    {
        m_LocalSpriteRenderer.enabled = false;
    }
}
