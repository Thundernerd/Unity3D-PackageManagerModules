using TNRD.Utilities;
using UnityEditor.Callbacks;

namespace TNRD.PackageManager.Modules.Utility
{
    public class SymbolUtility
    {
        private const string SYMBOL = "PACKAGE_MANAGER_MODULES";

        [DidReloadScripts]
        private static void EnsureSymbol()
        {
#if PACKAGE_MANAGER_INJECTION
            if (!ScriptingDefineUtility.Contains(SYMBOL))
            {
                ScriptingDefineUtility.Add(SYMBOL);
            }
#else
            ScriptingDefineUtility.Remove(SYMBOL);
#endif
        }
    }
}
