using System;
using JetBrains.Annotations;

namespace TNRD.PackageManager.Modules
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public interface IPackageManagerModule : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        string Identifier { get; }
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        string DisplayName { get; }
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        bool IsEnabled { get; }

        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        void Enable();

        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        void Disable();
    }
}