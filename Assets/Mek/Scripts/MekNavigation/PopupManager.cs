using System.Collections;
using System.Collections.Generic;
using Mek.Coroutines;
using UnityEngine;

namespace Mek.Navigation
{
    public class PopupManager : BaseUiManager
    {
        private Popup _activeView => ActiveView as Popup;
        
        private Queue<ViewParams> _queue = new Queue<ViewParams>();

        public void Register(ViewParams viewParams)
        {
            if (Contents.TryGetValue(viewParams.Key, out ContentBase content))
            {
                if (!_activeView)
                {
                    Open(viewParams);
                }
                else
                {
                    _queue.Enqueue(viewParams);
                }
            }
        }

        public void Register(string viewType)
        {
            if (Contents.TryGetValue(viewType, out ContentBase content))
            {
                var viewParams = new ViewParams(viewType);

                if (!_activeView)
                {
                    Open(viewParams);
                }
                else
                {
                    _queue.Enqueue(viewParams);
                }
            }
        }

        public override void CloseActiveContent()
        {
            if (_activeView == null)
            {
                Debug.LogError($"There is not an active content!");
            }
            else
            {
                if (_activeView.IsAnimating) return;

                _activeView.Close();
            }
        }

        protected override void OnActiveViewClosed()
        {
            base.OnActiveViewClosed();
            
            if (_queue.Count > 0)
            {
                var nextPopupParams = _queue.Dequeue();
                
                Open(nextPopupParams);
            }
            
        }
    }
}