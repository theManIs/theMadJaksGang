using UnityEngine;
using Assets.GamePrimal;
using Assets.TeamProjects.GamePrimal.MainScene;

namespace Assets.GamePrimal.Navigation.HighlightFrame
{
    public class SubjectFocus
    {
        private readonly int _rayCastDistance = 100;
        private Transform _currentFocus;
        private Transform _hardFocus;
        private bool _hasThisFrameFocused = true;
        private Transform _raycastCaptured;
        private MainScene _ms;
        private Texture2D _cursorTexture;
        private Texture2D _baseCursor;
        private Texture2D _pickCursor;

        public void Start()
        {
            _currentFocus = null;
            _cursorTexture = Resources.Load<Texture2D>("Cursor_Attack_-42768");
            _baseCursor = Resources.Load<Texture2D>("Cursor_Basic_-42304");
            _pickCursor = Resources.Load<Texture2D>("Cursor_Hand_-41952");
            _ms = Object.FindObjectOfType<MainScene>();
        }

        public Transform RetrieveRaycastCapture()
        {
            _raycastCaptured = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayCastDistance))
                _raycastCaptured = hit.collider.transform;

            return _raycastCaptured;
        }

        public void FixedUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _hasThisFrameFocused = true;

            SetDefaultCursor();

            if (Physics.Raycast(ray, out RaycastHit hit, _rayCastDistance))
            {
                if (hit.collider.gameObject.GetComponent<ClickToMove>() != null)
                {
                    _currentFocus = hit.collider.transform;

                    if (_hardFocus && _hardFocus != _currentFocus)
                        SetAggressiveCursor();
                    else
                        SetPickCursor();

                    if (Input.GetMouseButton(0))
                    {
//                        if (_hardFocus != null)
//                            _hardFocus.GetComponent<ClickToMove>().IsActive = false;

                        if (!_hardFocus)
                        {
                            _hardFocus = _currentFocus;
                            _hardFocus.GetComponent<ClickToMove>().IsActive = true;
                        }

                        _hasThisFrameFocused = false;
                    }
                }
                else
                {
                    _currentFocus = null;
                }
            }

            if (Input.GetMouseButton(1) && _hardFocus != null)
            {
                _hardFocus.GetComponent<ClickToMove>().IsActive = false;
                _hardFocus = null;
            }
        }

        private void SetPickCursor() => Cursor.SetCursor(_pickCursor, Vector2.zero, CursorMode.Auto);
        private void SetDefaultCursor() => Cursor.SetCursor(_baseCursor, Vector2.zero, CursorMode.Auto);
        private void SetAggressiveCursor() => Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);

        public bool HasFocused()
        {
            return _hasThisFrameFocused;
        }

        public Transform GetFocus()
        {
            return _hardFocus != null ? _hardFocus : _currentFocus;
        }
    }
}
