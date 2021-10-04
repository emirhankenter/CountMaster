using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mek.Navigation
{
    public class Navigation : MonoBehaviour
    {
        private static PanelManager _panel;
        public static PanelManager Panel
        {
            get
            {
                if (_panel == null)
                {
                    _panel = FindObjectOfType<PanelManager>();
                }

                return _panel;
            }
        }

        private static PopupManager _popup;
        public static PopupManager Popup
        {
            get
            {
                if (_popup == null)
                {
                    _popup = FindObjectOfType<PopupManager>();
                }

                return _popup;
            }
        }

        private static LayoutManager _layout;
        public static LayoutManager Layout
        {
            get
            {
                if (_layout == null)
                {
                    _layout = FindObjectOfType<LayoutManager>();
                }

                return _layout;
            }
        }
    }
}