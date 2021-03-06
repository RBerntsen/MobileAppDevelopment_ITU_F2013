﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Settings
{
    /// <summary>
    /// Helper class is needed because IsolatedStorageProperty is generic and 
    /// can not provide singleton model for static content
    /// </summary>
    internal static class IsolatedStoragePropertyHelper
    {
        /// <summary>
        /// We must use this object to lock saving settings
        /// </summary>
        public static readonly object ThreadLocker = new object();

        public static readonly IsolatedStorageSettings Store = IsolatedStorageSettings.ApplicationSettings;
    }

    /// <summary>
    /// This is wrapper class for storing one setting
    /// Object of this type must be single
    /// </summary>
    /// <typeparam name="T">Any serializable type</typeparam>
    public class IsolatedStorageProperty<T>
    {
        private readonly object _defaultValue;
        private readonly string _name;
        private readonly object _syncObject = new object();

        public IsolatedStorageProperty(string name, T defaultValue = default(T))
        {
            _name = name;
            _defaultValue = defaultValue;
        }

        //private bool _settingsChanged = false;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        //public bool hasSettingsChanged
        //{
        //    get
        //    {
        //        return _settingsChanged;
        //    }
        //    set
        //    {
        //        if (value != _settingsChanged)
        //        {
        //            _settingsChanged = value;
        //            NotifyPropertyChanged("SettingsChanged");
        //        }
        //    }
        //}

        /// <summary>
        /// Determines if setting exists in the storage
        /// </summary>
        public bool Exists
        {
            get { return IsolatedStoragePropertyHelper.Store.Contains(_name); }
        }

        /// <summary>
        /// Use this property to access the actual setting value
        /// </summary>
        public T Value
        {
            get
            {
                //If property does not exist - initializing it using default value
                if (!Exists)
                {
                    //Initializing only once
                    lock (_syncObject)
                    {
                        if (!Exists) SetDefault();
                    }
                }

                return (T)IsolatedStoragePropertyHelper.Store[_name];
            }
            set
            {
                IsolatedStoragePropertyHelper.Store[_name] = value;
                Save();
            }
        }

        private static void Save()
        {
            lock (IsolatedStoragePropertyHelper.ThreadLocker)
            {
                IsolatedStoragePropertyHelper.Store.Save();
            }
        }

        public void SetDefault()
        {
            Value = (T)_defaultValue;
        }
    }
}
