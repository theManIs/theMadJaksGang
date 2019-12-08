using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GamePrimal.Navigation.HighlightFrame
{
    public class GetRealHeight
    {
        public GameObject FrameHighlighterGameObject;
        public Transform GameObjectToHighlight;

        private MeshRenderer m_FrameMeshRenderer;
        private GameObject m_LocalInstance;
        private SpriteRenderer m_LocalSpriteRenderer;

        // Start is called before the first frame update
        public void Start(Transform parentObjectForMovables)
        {
//            GameObjectToHighlight = Object.FindObjectOfType<ClickToMove>().gameObject.transform;
            FrameHighlighterGameObject = Resources.Load<GameObject>("FrameHighlighter");
            m_LocalInstance = Object.Instantiate(FrameHighlighterGameObject, parentObjectForMovables);
            m_LocalSpriteRenderer = m_LocalInstance.GetComponent<SpriteRenderer>();
        }

        public void FixedUpdate()
        {
            TraceMousePosition();
        }

        public void FixedUpdate(Transform objToHighlight)
        {
            if (objToHighlight is null) {
                RemoveHighlight();
            } else {
                GameObjectToHighlight = objToHighlight;

                HighlightTheTarget(objToHighlight.gameObject);
            }

            
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
            if (m_LocalSpriteRenderer)
                m_LocalSpriteRenderer.enabled = false;
        }
    }
}
