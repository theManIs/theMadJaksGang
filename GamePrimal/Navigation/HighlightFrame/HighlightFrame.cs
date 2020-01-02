using Assets.TeamProjects.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame
{
    public class HighlightFrame
    {
        public GameObject fhgm;
        public Transform GameObjectToHighlight;

        public bool Engaged = true;
        private MeshRenderer m_FrameMeshRenderer;
        private GameObject m_LocalInstance;
        private SpriteRenderer m_LocalSpriteRenderer;

        private readonly string _frame = ResourcesList.FrameHighlighter;

        // Start is called before the first frame update
        public void Start()
        {
            fhgm = Resources.Load<GameObject>(_frame);
            m_LocalInstance = Object.Instantiate(fhgm, Vector3.one, fhgm.transform.rotation);
            m_LocalSpriteRenderer = m_LocalInstance.GetComponent<SpriteRenderer>();
        }

//        public void UserUpdate()
//        {
//            if (!Engaged) return;
//
//            TraceMousePosition();
//        }

        public void FixedUpdate(Transform objToHighlight)
        {
            if (!Engaged) return;
//                Debug.Log("Object to highlight " + (objToHighlight ? objToHighlight.GetInstanceID().ToString() : "null"));
            if (objToHighlight is null) {
                RemoveHighlight();
            } else {
                GameObjectToHighlight = objToHighlight;

                HighlightTheTarget(objToHighlight.gameObject);
            }

            
        }
    
        private void HighlightTheTarget(GameObject targetGameObject)
        {
            m_FrameMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
            Vector3 pos = targetGameObject.transform.position;
            
            //pos.x = m_FrameMeshRenderer.bounds.center.x;
            pos.y = m_FrameMeshRenderer.bounds.min.y + m_FrameMeshRenderer.bounds.size.y * .2f;
            //pos.z = m_FrameMeshRenderer.bounds.center.z;

            m_LocalInstance.transform.position = pos;
            m_LocalSpriteRenderer.enabled = true;
        }

        /// <summary>
        /// <Deprecated>Deprecated</Deprecated>
        /// </summary>
//        private void TraceMousePosition()
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
//            RaycastHit hit;
//
//            if (Physics.Raycast(ray, out hit, 100))
//            {
//                if (hit.collider.tag == "Targetable")
//                    HighlightTheTarget(hit.collider.gameObject);
//                else
//                    RemoveHighlight();
//            }
//        }

        private void RemoveHighlight()
        {
            if (m_LocalSpriteRenderer)
                m_LocalSpriteRenderer.enabled = false;
        }
    }
}
