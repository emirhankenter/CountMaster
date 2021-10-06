using System;
using Game.Scripts.Behaviours;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private FriendlyTeam _team;
        
        private Vector2? _lastMousePos;
        private Vector2? _currentMousePos;

        private bool IsPressing => _lastMousePos.HasValue;
        public FriendlyTeam Team => _team;
        public bool CanPlay => true;

        private Camera _cam;

        public void Initialize()
        {
            if (_cam == null)
            {
                _cam = CameraController.Instance.MainCamera;
            }
            var teamT = _team.transform;
            teamT.position = Vector3.zero;
            teamT.eulerAngles = Vector3.zero;
            
            _team.Initialize(1);
        }

        public void Dispose()
        {
            _team.Dispose();
        }

        private void Update()
        {
            if (!CanPlay) return;
            
            EvaluateInputs();
        }

        private void EvaluateInputs()
        {
            _currentMousePos = Input.mousePosition;
            
            if (!IsPressing && Input.GetMouseButtonDown(0))
            {
                _lastMousePos = _currentMousePos;
                OnPressPerformed();
                return;
            }

            if (IsPressing && Input.GetMouseButtonUp(0))
            {
                OnPressCanceled();
                _currentMousePos = null;
                _lastMousePos = null;
                return;
            }

            if (IsPressing && Input.GetMouseButton(0) && _currentMousePos != _lastMousePos)
            {
                OnDragPerformed();
                _lastMousePos = _currentMousePos;
            }
            else if (IsPressing && Input.GetMouseButton(0) && _currentMousePos == _lastMousePos)
            {
                OnHoldingPerformed();
            }
        }

        private void OnPressPerformed()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;
        }

        private void OnPressCanceled()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;
            
            _team.CancelLeaning();
        }

        private void OnDragPerformed()
        {
            if (!CanPlay) return;
            if (_lastMousePos == null || _currentMousePos == null) return;
            
            // var currentScreenPoint = _cam.ScreenToViewportPoint(new Vector3(_currentMousePos.Value.x, _currentMousePos.Value.y, _cam.nearClipPlane));
            // var lastScreenPoint = _cam.ScreenToViewportPoint(new Vector3(_lastMousePos.Value.x, _lastMousePos.Value.y, _cam.nearClipPlane));
            var delta = _currentMousePos - _lastMousePos;
            
            // Debug.Log(delta);
            // delta = _currentMousePos.Value - _lastMousePos.Value;
            float screenRatio = Screen.width * 1f / Screen.height * 1f;
            
            // _stickMan.transform.position += new Vector3(delta.x * screenRatio * 10f, 0, 0);

            var direction = new Vector3(delta.Value.x, 0, 0);

            _team.LeanTowards(direction);
        }

        private void OnHoldingPerformed()
        {
            _team.LeanTowards(Vector3.zero);
        }
    }
}
