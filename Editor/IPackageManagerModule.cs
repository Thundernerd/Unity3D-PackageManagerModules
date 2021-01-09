using System;
using JetBrains.Annotations;

namespace TNRD.PackageManager.Modules
{
    /// <summary>
    /// A module that will be injected into the Package Manager Window
    /// </summary>
    [PublicAPI]
    public interface IPackageManagerModule : IDisposable
    {
        /// <summary>
        /// A unique identifier identifying this specific module
        /// </summary>
        [PublicAPI]
        string Identifier { get; }
        /// <summary>
        /// The name to display in the modules menu
        /// </summary>
        [PublicAPI]
        string DisplayName { get; }
        /// <summary>
        /// Is this module currently enabled
        /// </summary>
        [PublicAPI]
        bool IsEnabled { get; }

        /// <summary>
        /// Called after the modules get created
        /// </summary>
        [PublicAPI]
        void Initialize();

        /// <summary>
        /// Called when the user enables the module from the modules menu
        /// </summary>
        [PublicAPI]
        void Enable();

        /// <summary>
        /// Called when the user disables the module from the modules menu
        /// </summary>
        [PublicAPI]
        void Disable();
    }
}