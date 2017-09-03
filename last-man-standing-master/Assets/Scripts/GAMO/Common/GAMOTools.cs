using UnityEngine;
using System.Collections;

namespace GAMO.Common
{
    public static class GAMOTools
    {
        public static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            if (obj)
            {
                UnityEditor.EditorUtility.SetDirty(obj);
            }
#endif
        }
    }
}
