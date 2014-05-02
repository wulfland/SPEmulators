namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Administration.Fakes;

    internal class SimSPPersistedObject : Isolator<SPPersistedObject, ShimSPPersistedObject>
    {
        private readonly string defaultName;
        private SPFarm farm;
        private string name;
        private Hashtable properties;
        private int updated;

        public bool WasDeleted
        {
            get;
            private set;
        }

        public int Updated
        {
            get
            {
                return this.updated;
            }
            private set
            {
                if (this.WasDeleted)
                {
                    throw new InvalidOperationException();
                }
                this.updated = value;
            }
        }

        public Hashtable Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new Hashtable();
                }

                return this.properties;
            }
            set
            {
                this.properties = value;
            }
        }

        public SPFarm Farm
        {
            get
            {
                return base.Instance as SPFarm ?? this.farm;
            }
            set
            {
                if (base.Instance is SPFarm)
                {
                    throw new InvalidOperationException();
                }

                this.farm = value;
            }
        }

        public SPPersistedObject Parent
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    this.name = this.defaultName;
                }

                return this.name;
            }
            set
            {
                this.name = value.Trim();
            }
        }

        public SimSPPersistedObject()
            : this(ShimRuntime.CreateUninitializedInstance<SPPersistedObject>())
        {
        }

        public SimSPPersistedObject(SPPersistedObject instance)
            : this(instance, null)
        {
        }

        public SimSPPersistedObject(SPPersistedObject instance, string defaultName)
            : base(instance)
        {
            if (string.IsNullOrEmpty(defaultName))
                throw new ArgumentNullException("defaultName");

            this.defaultName = defaultName;
            base.Fake.FarmGet = (() => this.Farm);
            base.Fake.PropertiesGet = (() => this.Properties);
            base.Fake.Delete = (delegate
            {
                this.WasDeleted = true;
            });
            base.Fake.Update = (delegate
            {
                this.Updated++;
            });
            base.Fake.UpdateBoolean = (delegate(bool ensureOnUpdate)
            {
                this.Updated++;
            });
            base.Fake.ParentGet = (() => this.Parent);
            base.Fake.NameGet = (() => this.Name);
            base.Fake.NameSetString = (delegate(string value)
            {
                this.Name = value;
            });
        }

        public static SimSPPersistedObject FromInstance(SPPersistedObject instance)
        {
            return InstancedPool.CastAsInstanced<SPPersistedObject, SimSPPersistedObject>(instance);
        }
    }
}
