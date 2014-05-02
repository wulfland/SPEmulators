namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPField : Isolator<SPField, ShimSPField>
    {
        public string InternalName
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public SPFieldType Type
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public bool ReadOnly
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Indexed
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string TypeDisplayName
        {
            get;
            set;
        }

        public string DefaultFormula
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        public SPFieldCollection Fields
        {
            get;
            set;
        }

        public SPList ParentList
        {
            get
            {
                return this.Fields.List;
            }
        }

        public bool Updated
        {
            get;
            set;
        }

        public SimSPField()
            : this(ShimRuntime.CreateUninitializedInstance<SPField>())
        {
        }

        public SimSPField(SPField instance)
            : base(instance)
        {
            this.Description = string.Empty;
            base.Fake.InternalNameGet = () => this.InternalName;
            base.Fake.IndexedGet = () => this.Indexed;
            base.Fake.IndexedSetBoolean = (bool value) =>
            {
                this.Indexed = value;
            };
            base.Fake.TitleGet = () => this.Title;
            base.Fake.TitleSetString = (string value) =>
            {
                this.Title = value;
            };
            base.Fake.TypeGet = () => this.Type;
            base.Fake.TypeSetSPFieldType = (SPFieldType value) =>
            {
                this.Type = value;
            };
            base.Fake.RequiredGet = () => this.Required;
            base.Fake.RequiredSetBoolean = (bool value) =>
            {
                this.Required = value;
            };
            base.Fake.ReadOnlyFieldGet = () => this.ReadOnly;
            base.Fake.ReadOnlyFieldSetBoolean = (bool value) =>
            {
                this.ReadOnly = value;
            };
            base.Fake.IdGet = () => this.Id;
            base.Fake.StaticNameGet = () => this.InternalName;
            base.Fake.StaticNameSetString = (string value) =>
            {
                this.InternalName = value;
            };
            base.Fake.Delete = new FakesDelegates.Action(this.Delete);
            base.Fake.DefaultFormulaGet = () => this.DefaultFormula;
            base.Fake.DefaultFormulaSetString = (string value) =>
            {
                this.DefaultFormula = value;
            };
            base.Fake.DefaultValueGet = () => this.DefaultValue;
            base.Fake.DefaultValueSetString = (string value) =>
            {
                this.DefaultValue = value;
            };
            base.Fake.DescriptionGet = () => this.Description;
            base.Fake.DescriptionSetString = (string value) =>
            {
                this.Description = value;
            };
            base.Fake.Update = () =>
            {
                this.Updated = true;
            };
            base.Fake.UpdateBoolean = (bool pushToList) =>
            {
                this.Updated = true;
            };
            base.Fake.ParentListGet = () => this.ParentList;
            base.Fake.FieldsGet = () => this.Fields;
            base.Fake.GetPropertyString = (string name) =>
            {
                string result;
                if (name == "InternalName")
                {
                    result = this.InternalName;
                }
                else
                {
                    if (name == "Title")
                    {
                        result = this.Title;
                    }
                    else
                    {
                        if (name == "Description")
                        {
                            result = this.Description;
                        }
                        else
                        {
                            if (name == "DefaultFormula")
                            {
                                result = this.DefaultFormula;
                            }
                            else
                            {
                                if (name == "DefaultValue")
                                {
                                    result = this.DefaultValue;
                                }
                                else
                                {
                                    result = null;
                                }
                            }
                        }
                    }
                }
                return result;
            };
            base.Fake.TypeDisplayNameGet = () => this.TypeDisplayName;
        }

        public void Delete()
        {
            this.Fields.Delete(this.InternalName);
        }

        public static SimSPField FromInstance(SPField instance)
        {
            return InstancedPool.CastAsInstanced<SPField, SimSPField>(instance);
        }
        internal static void Initialize()
        {
            ShimSPField.BehaveAsNotImplemented();
        }
    }
}
