using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame
{
    public class AbstractHighlight
    {


        #region Fields

        public bool Enabled = true;
        public Transform GameObjectToHighlight;
        public string ClassName = nameof(AbstractHighlight);

        private MeshRenderer _frameMeshRenderer;
        private GameObject _localInstance;
        private SpriteRenderer _localSpriteRenderer;
        private Transform _localTransform; 

        #endregion


        #region Methods

        public void OnStart(string highlightName)
        {
            Sprite loadedHighlight = Resources.Load<Sprite>(highlightName);
            _localInstance = new GameObject(ClassName);
            _localSpriteRenderer = _localInstance.AddComponent<SpriteRenderer>();
            _localSpriteRenderer.sprite = loadedHighlight;
            _localTransform = _localInstance.GetComponent<Transform>();
            _localTransform.localScale = Vector3.one;
        }

        public void SetScaleFactor(Vector3 sf) => _localTransform.localScale = sf;
        public void SetEulerRotation(Vector3 rot) => _localTransform.eulerAngles = rot;
        public void SetParentObject(Transform trans) => _localTransform.parent = trans;

        public void FixedUpdate(Transform objToHighlight)
        {
            if (!Enabled) return;

            if (objToHighlight is null)
            {
                RemoveHighlight();
            }
            else
            {
                GameObjectToHighlight = objToHighlight;

                HighlightTheTarget(objToHighlight.gameObject);
            }
        }

        private void HighlightTheTarget(GameObject targetGameObject)
        {
            _frameMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
            Vector3 pos = targetGameObject.transform.position;
            pos.y = _frameMeshRenderer.bounds.min.y + _frameMeshRenderer.bounds.size.y * .2f;

            _localInstance.transform.position = pos;
            _localSpriteRenderer.enabled = true;
        }

        private void RemoveHighlight()
        {
            if (_localSpriteRenderer)
                _localSpriteRenderer.enabled = false;
        } 

        #endregion

    }
}
