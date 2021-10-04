using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mek.Navigation
{
    public class ContentBase : MonoBehaviour
    {
        public event Action Opened; 
        public event Action Closed; 

        public virtual void Open(ViewParams viewParams)
        {
            gameObject.SetActive(true);
            Opened?.Invoke();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }
}