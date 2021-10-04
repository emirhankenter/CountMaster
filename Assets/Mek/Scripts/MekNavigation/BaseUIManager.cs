using System;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Navigation
{
    public abstract class BaseUiManager : MonoBehaviour
    {
        [Serializable] public class ContentDictionary : SerializableDictionary<string, ContentBase> { };

        public ContentDictionary Contents;

        public ContentBase ActiveView { get; protected set; }

        public virtual bool Open(ViewParams viewParams)
        {
            if (ActiveView != null)
            {
                Debug.LogError($"There is an active content with name: {ActiveView.GetType()}");
                return false;
            }
            if (Contents.TryGetValue(viewParams.Key, out ContentBase content))
            {
                content.Open(viewParams);
                ActiveView = content;
                ActiveView.Closed += OnActiveViewClosed;
                return true;
            }
            return false;
        }

        public virtual bool Open(string viewType)
        {
            if (ActiveView != null)
            {
                Debug.LogError($"There is an active content with name: {ActiveView.GetType()}");
                return false;
            }
            if (Contents.TryGetValue(viewType, out ContentBase content))
            {
                content.Open(new ViewParams(viewType));
                ActiveView = content;
                ActiveView.Closed += OnActiveViewClosed;
                return true;
            }

            return false;
        }

        public bool Change(ViewParams viewParams)
        {
            if (ActiveView != null)
            {
                ActiveView.Close();
            }

            return Open(viewParams);
        }
        public bool Change(string viewType)
        {
            if (ActiveView != null)
            {
                ActiveView.Close();
            }

            return Open(viewType);
        }

        public virtual void CloseActiveContent()
        {
            if (ActiveView == null)
            {
                Debug.LogError($"There is not an active content!");
            }
            else
            {
                ActiveView.Close();
                ActiveView = null;
            }
        }

        protected virtual void OnActiveViewClosed()
        {
            ActiveView.Closed -= OnActiveViewClosed;

            ActiveView = null;
        }

        public void ToggleBlocker(bool state)
        {
            if (transform.childCount == 0) return;
            transform.GetChild(0).gameObject.SetActive(state);
        }
    }
}