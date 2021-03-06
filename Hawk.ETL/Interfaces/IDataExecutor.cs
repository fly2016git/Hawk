﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls.WpfPropertyGrid.Attributes;
using Hawk.Core.Connectors;
using Hawk.Core.Utils;
using Hawk.Core.Utils.MVVM;
using Hawk.Core.Utils.Plugins;

namespace Hawk.ETL.Interfaces
{
    public interface IDataExecutor : IColumnProcess
    {
        IEnumerable<IFreeDocument> Execute(IEnumerable<IFreeDocument> documents);
    }


    public abstract class DataExecutorBase : PropertyChangeNotifier, IDataExecutor
    {
        private bool _enabled;

        protected DataExecutorBase()
        {
            this.Enabled = true;
        }
        protected bool IsExecute;

        public void SetExecute(bool value)
        {
            IsExecute = value;
        }
    public virtual FreeDocument DictSerialize(Scenario scenario = Scenario.Database)
        {
            var dict = this.UnsafeDictSerialize();

            dict.Add("Type", this.GetType().Name);

            dict.Add("Group", "Executor");
            return dict;
        }

        public virtual void DictDeserialize(IDictionary<string, object> docu, Scenario scenario = Scenario.Database)
        {
            this.UnsafeDictDeserialize(docu);
            var doc = docu as FreeDocument;
          
        }
        [LocalizedDisplayName("介绍")]
        [PropertyOrder(100)]
        public string Description
        {
            get
            {
                var item = AttributeHelper.GetCustomAttribute(GetType());
                if (item == null)
                    return GetType().ToString();
                return item.Description;
            }
        }
        [LocalizedCategory("1.基本选项"), PropertyOrder(1), DisplayName("输入列")]
        public string Column { get; set; }




        [LocalizedCategory("1.基本选项")]
        [LocalizedDisplayName("启用")]
        [PropertyOrder(5)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        [LocalizedCategory("1.基本选项")]
        [LocalizedDisplayName("类型")]
        [PropertyOrder(0)]
        public string TypeName
        {
            get
            {
                var item = AttributeHelper.GetCustomAttribute(GetType());
                return item == null ? GetType().ToString() : item.Name;
            }
        }

     

        public virtual void Finish()
        {
        }

        public virtual bool Init(IEnumerable<IFreeDocument> datas)
        {
            return false;
        }

       public   abstract IEnumerable<IFreeDocument> Execute(IEnumerable<IFreeDocument> documents);
        
        

        public override string ToString()
        {
            return TypeName + " " + Column;
        }

    }
}