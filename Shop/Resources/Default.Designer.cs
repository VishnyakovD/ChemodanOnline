﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Default {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Default() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Shop.Resources.Default", typeof(Default).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Чемоданы.
        /// </summary>
        public static string Chemodans {
            get {
                return ResourceManager.GetString("Chemodans", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Тип чемодана.
        /// </summary>
        public static string ChemodanTypes {
            get {
                return ResourceManager.GetString("ChemodanTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Самые востребованные.
        /// </summary>
        public static string FavoriteProducts {
            get {
                return ResourceManager.GetString("FavoriteProducts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не сохранено.
        /// </summary>
        public static string NotSaved {
            get {
                return ResourceManager.GetString("NotSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это поле обязательно для заполнения.
        /// </summary>
        public static string RequiredInput {
            get {
                return ResourceManager.GetString("RequiredInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сохранено.
        /// </summary>
        public static string Saved {
            get {
                return ResourceManager.GetString("Saved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Какой чемодан вы бы хотели взять на прокат?.
        /// </summary>
        public static string TitleMainPage {
            get {
                return ResourceManager.GetString("TitleMainPage", resourceCulture);
            }
        }
    }
}
