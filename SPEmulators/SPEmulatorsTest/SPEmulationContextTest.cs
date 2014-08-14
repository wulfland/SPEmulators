namespace SPEmulatorsTest
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPEmulators;

    [TestClass]
    public class SPEmulationContextTest
    {
        [TestMethod]
        public void Can_Construct_Level_Fake()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake))
            {
                Assert.AreSame(sut.Site, SPContext.Current.Site);
                Assert.AreSame(sut.Web, SPContext.Current.Web);
                Assert.AreEqual(IsolationLevel.Fake, sut.IsolationLevel);
            }
        }

        [TestMethod]
        public void Can_Construct_Level_Integration()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Integration, "http://localhost"))
            {
                Assert.AreSame(sut.Site, SPContext.Current.Site);
                Assert.AreSame(sut.Web, SPContext.Current.Web);
                Assert.AreEqual(IsolationLevel.Integration, sut.IsolationLevel);
            }
        }

        [TestMethod]
        public void Can_Construct_Level_None()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.None, "http://localhost"))
            {
                Assert.IsNull(SPContext.Current);
                Assert.IsNotNull(sut.Site);
                Assert.IsNotNull(sut.Web);
                Assert.AreEqual(IsolationLevel.None, sut.IsolationLevel);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Construct_Trows_On_Invalid_level()
        {
            using (var sut = new SPEmulationContext((IsolationLevel)255, "http://localhost"))
            {
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_Throws_On_x86_Process()
        {
            using (var outerShimsContect = ShimsContext.Create())
            {
                System.Fakes.ShimEnvironment.Is64BitProcessGet = () => false;
                using (var sut = new SPEmulationContext((IsolationLevel)255, "http://localhost"))
                {
                }   
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOrCreateList_Throws_ArgumentNullException()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                sut.GetOrCreateList(null, SPListTemplateType.DocumentLibrary);
            }  
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOrCreateList_Overload_Throws_ArgumentNullException()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                sut.GetOrCreateList(null);
            }
        }

        [TestMethod]
        public void GetOrCreateList_Returns_List_On_Level_Integration()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Integration, "http://localhost"))
            {
                var id = sut.Web.Lists.Add("MyList", "a description", SPListTemplateType.GenericList);
                var list = sut.Web.Lists[id];

                var result = sut.GetOrCreateList("MyList", SPListTemplateType.DocumentLibrary);

                Assert.AreEqual<Guid>(list.ID, result.ID);

                list.Delete();
            }  
        }

        [TestMethod]
        public void GetOrCreateList_Returns_List_On_Level_None()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.None, "http://localhost"))
            {
                var id = sut.Web.Lists.Add("MyList", "a description", SPListTemplateType.GenericList);
                var list = sut.Web.Lists[id];

                var result = sut.GetOrCreateList("MyList", SPListTemplateType.DocumentLibrary);

                Assert.AreEqual<Guid>(list.ID, result.ID);

                list.Delete();
            }
        }

        [TestMethod]
        public void GetOrCreateList_Returns_List_On_Level_Fake()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var result = sut.GetOrCreateList("MyList", SPListTemplateType.DocumentLibrary);

                var list = sut.Web.Lists["MyList"];

                Assert.IsNotNull(list);
            }
        }

        [TestMethod]
        public void GetOrCreateList_Returns_List_With_Fields()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var result = sut.GetOrCreateList("MyList", SPListTemplateType.DocumentLibrary, "MyCustomField1", "MyCustomField2");

                var list = sut.Web.Lists["MyList"];

                Assert.IsNotNull(list);
                Assert.IsNotNull(list.Fields.GetFieldByInternalName("MyCustomField1"));
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Returns_ListInstance_by_Elements()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var result = sut.GetOrCreateList(@"..\..\..\SharePointSampleProject\ADefaultList\Elements.xml");

                var list = sut.Web.Lists["ADefaultList"];

                Assert.IsNotNull(list);
                Assert.AreEqual<string>("My List Instance", list.Description);
                Assert.AreEqual<SPListTemplateType>(SPListTemplateType.GenericList, list.BaseTemplate);
                Assert.IsTrue(list.OnQuickLaunch);
                Assert.AreNotEqual<int>(0, list.Fields.Count);
                Assert.IsNotNull(list.Fields[SPBuiltInFieldId.Title]);
                Assert.IsNotNull(list.Fields[SPBuiltInFieldId.ID]);
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Supports_Default_Data()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var result = sut.GetOrCreateList(@"..\..\..\SharePointSampleProject\ADefaultList\Elements.xml");

                var list = sut.Web.Lists["ADefaultList"];

                Assert.IsNotNull(list);
                Assert.AreEqual<int>(2, list.ItemCount);
                Assert.AreEqual<int>(1, list.Items[0].ID);
                Assert.AreEqual<int>(2, list.Items[1].ID);
                Assert.AreEqual<string>("Default Item 1", list.Items[0]["Title"].ToString());
                Assert.AreEqual<string>("Default Item 2", list.Items[1]["Title"].ToString());
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Returns_List_On_Level_Integration()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Integration, "http://localhost"))
            {
                var id = sut.Web.Lists.Add("ADefaultList", "My List Instance", SPListTemplateType.GenericList);
                var list = sut.Web.Lists[id];

                var result = sut.GetOrCreateList(@"..\..\..\SharePointSampleProject\ADefaultList\Elements.xml");

                Assert.AreEqual<Guid>(list.ID, result.ID);

                list.Delete();
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Returns_List_On_Level_None()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.None, "http://localhost"))
            {
                var id = sut.Web.Lists.Add("ADefaultList", "My List Instance", SPListTemplateType.GenericList);
                var list = sut.Web.Lists[id];

                var result = sut.GetOrCreateList(@"..\..\..\SharePointSampleProject\ADefaultList\Elements.xml");

                Assert.AreEqual<Guid>(list.ID, result.ID);

                list.Delete();
            }
        }
    }
}
