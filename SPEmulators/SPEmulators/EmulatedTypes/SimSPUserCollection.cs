namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPUserCollection : CollectionIsolator<SPUser, SPUserCollection, ShimSPUserCollection>
    {
        public SimSPUserCollection()
            : this(null)
        {
        }

        public SimSPUserCollection(SPUserCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.ItemGetInt32 = ((int index) => base[index]);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.ItemGetString = (delegate(string login)
            {
                var user = this.SingleOrDefault((SPUser u) => string.Equals(u.LoginName, login, StringComparison.Ordinal));
                if (user != null)
                {
                    return user;
                }
                throw new SPException();
            });
            base.Fake.GetByIDInt32 = (delegate(int id)
            {
                var user = this.SingleOrDefault((SPUser u) => u.ID == id);
                if (user != null)
                {
                    return user;
                }
                throw new SPException();
            });
            base.Fake.GetByEmailString = (delegate(string email)
            {
                if (string.IsNullOrEmpty(email) || email.Length > 255)
                {
                    throw new SPException();
                }
                var user = this.SingleOrDefault((SPUser u) => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    return user;
                }
                throw new SPException();
            });
            base.Fake.AddStringStringStringString = (delegate(string loginName, string email, string name, string notes)
            {
                this.Add(loginName, email, name, notes);
            });
        }

        public SimSPUser Add(string loginName, string email, string name, string notes)
        {
            var user = new SimSPUser
            {
                LoginName = loginName,
                Email = email,
                Name = name,
                Notes = notes
            };
            base.Add(user.Instance);
            return user;
        }
    }
}
