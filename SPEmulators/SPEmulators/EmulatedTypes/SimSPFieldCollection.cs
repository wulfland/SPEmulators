namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;


    internal class SimSPFieldCollection : CollectionIsolator<SPField, SPFieldCollection, ShimSPFieldCollection>
    {
        private SPWeb web;

        public SPList List
        {
            get;
            set;
        }
        public SPWeb Web
        {
            get
            {
                SPWeb result;
                if (this.web != null)
                {
                    result = this.web;
                }
                else
                {
                    if (this.List != null)
                    {
                        result = this.List.Lists.Web;
                    }
                    else
                    {
                        result = this.web;
                    }
                }
                return result;
            }
            set
            {
                this.web = value;
            }
        }
        public SimSPFieldCollection()
            : this(null)
        {
        }
        public SimSPFieldCollection(SPFieldCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.ContainsGuid = (Guid id) => this.Any((SPField field) => field.Id == id);
            base.Fake.ContainsFieldString = name => this.Any(field => field.Title == name || field.InternalName == name);
            base.Fake.ItemGetInt32 = (int index) => base[index];
            base.Fake.ItemAtIndexInt32 = (int index) => base[index];
            base.Fake.ItemGetString = (string title) =>
            {
                foreach (SPField current in this)
                {
                    if (current.Title == title)
                    {
                        return current;
                    }
                }
                throw new ArgumentException();
            };
            base.Fake.ItemGetGuid = (Guid id) =>
            {
                foreach (SPField current in this)
                {
                    if (current.Id == id)
                    {
                        return current;
                    }
                }
                throw new ArgumentException();
            };
            base.Fake.ListGet = () => this.List;
            base.Fake.WebGet = () => this.Web;
            base.Fake.TryGetFieldByStaticNameString = (string staticName) =>
            {
                if (string.IsNullOrEmpty(staticName))
                {
                    throw new ArgumentNullException("staticName");
                }
                return this.FirstOrDefault((SPField field) => field.StaticName == staticName);
            };
            base.Fake.AddFieldAsXmlString = (string schema) => base.Instance.AddFieldAsXml(schema, false, 0);
            base.Fake.AddFieldAsXmlStringBooleanSPAddFieldOptions = (string schema, bool addToView, SPAddFieldOptions op) =>
            {
                var emSPField = this.CreateField();
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(schema)))
                {
                    xmlReader.ReadToFollowing("Field");
                    xmlReader.MoveToAttribute("Name");
                    emSPField.InternalName = xmlReader.Value;
                    emSPField.Title = xmlReader.Value;
                }
                base.Add(emSPField.Instance);
                return emSPField.InternalName;
            };
            base.Fake.AddStringSPFieldTypeBoolean = (string displayName, SPFieldType type, bool required) => base.Instance.Add(displayName, type, required, false, null);
            base.Fake.AddStringSPFieldTypeBooleanBooleanStringCollection = new FakesDelegates.Func<string, SPFieldType, bool, bool, StringCollection, string>(this.Add);
            base.Fake.DeleteString = new FakesDelegates.Action<string>(this.Delete);
            base.Fake.GetIndexForInternalNameString = (string internalName) =>
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i].InternalName == internalName)
                    {
                        return i;
                    }
                }
                throw new ArgumentException();
            };
            base.Fake.GetFieldByInternalNameString = (string internalName) =>
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i].InternalName == internalName)
                    {
                        return base[i];
                    }
                }
                throw new ArgumentException();
            };
            base.Fake.GetFieldString = (string name) =>
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i].InternalName == name)
                    {
                        return base[i];
                    }
                }
                throw new ArgumentException();
            };
        }
        public string Add(string title, SPFieldType type, bool required, bool compactName, StringCollection choices)
        {
            if (type <= SPFieldType.Computed)
            {
                if (type == SPFieldType.Computed)
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                switch (type)
                {
                    case SPFieldType.File:
                    case SPFieldType.Attachments:
                    case SPFieldType.Recurrence:
                    case SPFieldType.CrossProjectLink:
                    case SPFieldType.AllDayEvent:
                        throw new InvalidOperationException();
                    case SPFieldType.User:
                    default:
                        break;
                }
            }
        
            var simSPField = new SimSPField
            {
                Title = title,
                Id = Guid.NewGuid(),
                Type = type,
                Required = required,
                InternalName = title,
                TypeDisplayName = "TypeDisplayName",
                Fields = base.Instance
            };

            base.Add(simSPField.Instance);

            return simSPField.InternalName;
        }
        public void Delete(string name)
        {
            foreach (SPField current in this)
            {
                if (current.InternalName == name)
                {
                    base.Remove(current);
                    break;
                }
            }
        }
        protected override SPField CreateItem()
        {
            return this.CreateField().Instance;
        }
        private SimSPField CreateField()
        {
            return new SimSPField
            {
                Fields = base.Instance
            };
        }
        public void SetAll(params string[] fieldNames)
        {
            if (fieldNames == null)
            {
                throw new ArgumentNullException("fieldNames");
            }
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string title = fieldNames[i];
                this.Add(title, SPFieldType.Text, false, false, null);
            }
        }
        internal static void Initialize()
        {
            ShimSPFieldCollection.BehaveAsNotImplemented();
        }
    }
}
