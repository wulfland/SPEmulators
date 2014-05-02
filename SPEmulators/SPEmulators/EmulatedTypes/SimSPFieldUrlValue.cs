namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFieldUrlValue : Isolator<SPFieldUrlValue, ShimSPFieldUrlValue>
    {
        private const string Delimiter = ",";

        internal string Description
        {
            get;
            set;
        }

        internal string Url
        {
            get;
            set;
        }
        public SimSPFieldUrlValue()
            : this(ShimRuntime.CreateUninitializedInstance<SPFieldUrlValue>())
        {
        }
        public SimSPFieldUrlValue(SPFieldUrlValue instance)
            : this(instance, null)
        {
        }
        public SimSPFieldUrlValue(SPFieldUrlValue instance, string fieldValue)
            : base(instance)
        {
            base.Fake.UrlGet = () => this.Url;
            base.Fake.UrlSetString = delegate(string value)
            {
                this.Url = value;
            };
            base.Fake.DescriptionGet = () => this.Description;
            base.Fake.DescriptionSetString = delegate(string value)
            {
                this.Description = value;
            };
            base.Fake.ToString = new FakesDelegates.Func<string>(this.ToString);
            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (!fieldValue.Contains(Delimiter))
                {
                    throw new ArgumentException("Value has the wrong format and does not contain the delimiter.", "fieldValue");
                }
                string[] array = fieldValue.Split(Delimiter.ToCharArray());
                this.Url = array[0].Trim();
                this.Description = array[1].Trim();
            }
        }

        public override string ToString()
        {
            return string.Join(",", new string[]
            {
                this.Url,
                this.Description
            });
        }

        public static void Initialize()
        {
            ShimSPFieldUrlValue.BehaveAsNotImplemented();
            ShimSPFieldUrlValue.Constructor = delegate(SPFieldUrlValue me)
            {
                new SimSPFieldUrlValue(me);
            };
            ShimSPFieldUrlValue.ConstructorString = delegate(SPFieldUrlValue me, string s)
            {
                new SimSPFieldUrlValue(me, s);
            };
        }
    }
}
