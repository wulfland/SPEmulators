namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPUser : Isolator<SPUser, ShimSPUser>
    {
        public int ID
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }
        public string LoginName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public SimSPUser()
            : this(null)
        {
        }

        public SimSPUser(SPUser instance)
            : base(instance)
        {
            base.Fake.IDGet = (() => this.ID);
            base.Fake.EmailGet = (() => this.Email);
            base.Fake.EmailSetString = (delegate(string value)
            {
                this.Email = value;
            });
            base.Fake.LoginNameGet = (() => this.LoginName);
            base.Fake.NameGet = (() => this.Name);
            base.Fake.NameSetString = (delegate(string value)
            {
                this.Name = value;
            });
            base.Fake.NotesGet = (() => this.Notes);
            base.Fake.NotesSetString = (delegate(string value)
            {
                this.Notes = value;
            });
        }

        public static SimSPUser FromInstance(SPUser instance)
        {
            return InstancedPool.CastAsInstanced<SPUser, SimSPUser>(instance);
        }
    }
}
