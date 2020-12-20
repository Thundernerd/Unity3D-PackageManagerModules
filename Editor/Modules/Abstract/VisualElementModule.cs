using System;
using TNRD.PackageManager.Modules;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VisualElementModule : PackageManagerModule
    {
        /// <summary>
        /// 
        /// </summary>
        protected enum InjectionMethod
        {
            /// <summary>
            /// 
            /// </summary>
            Add,
            /// <summary>
            /// 
            /// </summary>
            Insert
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract InjectionMethod Method { get; }
        /// <summary>
        /// 
        /// </summary>
        protected abstract ModuleVisualElement ElementToInject { get; }
        /// <summary>
        /// 
        /// </summary>
        protected abstract VisualElement ElementRoot { get; }
        /// <summary>
        /// 
        /// </summary>
        protected virtual int InsertIndex { get; }

        protected override void OnEnable()
        {
            switch (Method)
            {
                case InjectionMethod.Add:
                    ElementRoot.Add(ElementToInject);
                    break;
                case InjectionMethod.Insert:
                    ElementRoot.Insert(InsertIndex, ElementToInject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ElementToInject.Enable();
        }

        protected override void OnDisable()
        {
            ElementRoot.Remove(ElementToInject);
            ElementToInject.Disable();
        }
    }
}