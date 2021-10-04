using System;

namespace Mek.Models
{
    public enum DebugLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
    public class MekLog
    {
        protected readonly string _className;
        protected readonly DebugLevel _level;
        public MekLog(string className, DebugLevel level)
        {
            _className = className;
            _level = level;
        }

        public void Debug(string context)
        {
            if ((int) _level > (int) DebugLevel.Debug) return;
            UnityEngine.Debug.Log($"[{_className}]: {context}");
        }

        public void Info(string context)
        {
            if ((int)_level > (int)DebugLevel.Info) return;
            UnityEngine.Debug.Log($"[{_className}]: {context}");
        }

        public void Warning(string context)
        {
            if ((int)_level > (int)DebugLevel.Warning) return;
            UnityEngine.Debug.LogWarning($"[{_className}]: {context}");
        }

        public void Error(string context)
        {
            if ((int)_level > (int)DebugLevel.Error) return;
            UnityEngine.Debug.LogError($"[{_className}]: {context}");
        }
    }
}

