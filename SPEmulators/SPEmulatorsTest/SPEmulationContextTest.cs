namespace SPEmulatorsTest
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPEmulators;
    using FluentAssertions;

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

        [TestMethod]
        public void GetOrCreateList_Overload_Supports_Document_Libraries()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var result = sut.GetOrCreateList(@"..\..\..\SharePointSampleProject\ADefaultDocumentLibrary\Elements.xml");

                var list = sut.Web.Lists["ADefaultDocumentLibrary"];

                Assert.AreSame(result, list);

                Assert.IsNotNull(list);
                Assert.AreEqual<string>("My List Instance", list.Description);
                Assert.AreEqual<SPListTemplateType>(SPListTemplateType.DocumentLibrary, list.BaseTemplate);
                Assert.IsTrue(list.OnQuickLaunch);
                Assert.AreNotEqual<int>(0, list.Fields.Count);
                Assert.IsNotNull(list.Fields[SPBuiltInFieldId.Title]);
                Assert.IsNotNull(list.Fields[SPBuiltInFieldId.ID]);
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Returns_Custom_List_Instance_by_Elements_And_Schema()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var elements = @"..\..\..\SharePointSampleProject\ACustomList\ACustomListInstance\Elements.xml";
                var schema = @"..\..\..\SharePointSampleProject\ACustomList\schema.xml";
                var result = sut.GetOrCreateList(elements, schema);

                var list = sut.Web.Lists["ACustomList"];

                Assert.AreSame(result, list);

                //<Field Name="StringColumn" ID="{081246d3-2d7c-4f8c-b754-48dfd4c13646}" DisplayName="String Column" Type="Text" Required="TRUE" />
                var fieldName = "String Column";
                Assert.IsNotNull(list.Fields[fieldName]);
                Assert.IsNotNull(list.Fields.GetFieldByInternalName("StringColumn"));
                Assert.AreEqual<Guid>(new Guid("{081246d3-2d7c-4f8c-b754-48dfd4c13646}"), list.Fields[fieldName].Id);
                Assert.AreEqual<string>("StringColumn", list.Fields[fieldName].InternalName);
                Assert.AreEqual<string>("String Column", list.Fields[fieldName].Title);
                Assert.AreEqual<SPFieldType>(SPFieldType.Text, list.Fields[fieldName].Type);
                Assert.IsTrue(list.Fields[fieldName].Required);

                //<Field Name="DateAndTimeColumn" ID="{3ca9275d-7289-4ed4-86b8-9fe0d8921330}" DisplayName="Date And Time Column" Type="DateTime" />
                fieldName = "Date And Time Column";
                Assert.IsNotNull(list.Fields[fieldName]);
                Assert.IsNotNull(list.Fields.GetFieldByInternalName("DateAndTimeColumn"));
                Assert.AreEqual<Guid>(new Guid("{3ca9275d-7289-4ed4-86b8-9fe0d8921330}"), list.Fields[fieldName].Id);
                Assert.AreEqual<string>("DateAndTimeColumn", list.Fields[fieldName].InternalName);
                Assert.AreEqual<string>("Date And Time Column", list.Fields[fieldName].Title);
                Assert.AreEqual<SPFieldType>(SPFieldType.DateTime, list.Fields[fieldName].Type);
                Assert.IsFalse(list.Fields[fieldName].Required);

                //<Field Name="LookupField" ID="{86af6c5f-3a60-49b1-b77d-ce7bf0d02d7c}" DisplayName="Lookup Field" Type="Lookup" ShowField="Title" List="Lists/ADefaultList" />
                fieldName = "Lookup Field";
                Assert.IsNotNull(list.Fields[fieldName]);
                Assert.IsNotNull(list.Fields.GetFieldByInternalName("LookupField"));
                Assert.AreEqual<Guid>(new Guid("{86af6c5f-3a60-49b1-b77d-ce7bf0d02d7c}"), list.Fields[fieldName].Id);
                Assert.AreEqual<string>("LookupField", list.Fields[fieldName].InternalName);
                Assert.AreEqual<string>(fieldName, list.Fields[fieldName].Title);
                Assert.AreEqual<SPFieldType>(SPFieldType.Lookup, list.Fields[fieldName].Type);
                Assert.IsFalse(list.Fields[fieldName].Required);
                
                //<Field Name="User1" ID="{e7665fd0-64de-4068-a56e-3f1dff5432a3}" DisplayName="User Field" Type="User" List="UserInfo" />
                fieldName = "User Field";
                Assert.IsNotNull(list.Fields[fieldName]);
                Assert.IsNotNull(list.Fields.GetFieldByInternalName("User1"));
                Assert.AreEqual<Guid>(new Guid("{e7665fd0-64de-4068-a56e-3f1dff5432a3}"), list.Fields[fieldName].Id);
                Assert.AreEqual<string>("User1", list.Fields[fieldName].InternalName);
                Assert.AreEqual<string>(fieldName, list.Fields[fieldName].Title);
                Assert.AreEqual<SPFieldType>(SPFieldType.User, list.Fields[fieldName].Type);
                Assert.IsFalse(list.Fields[fieldName].Required);
            }
        }

        [TestMethod]
        public void GetOrCreateList_Overload_Returns_Custom_List_Instance_by_Elements_And_Schema_With_Default_Data()
        {
            using (var sut = new SPEmulationContext(IsolationLevel.Fake, "http://localhost"))
            {
                var elements = @"..\..\..\SharePointSampleProject\ACustomList\ACustomListInstance\Elements.xml";
                var schema = @"..\..\..\SharePointSampleProject\ACustomList\schema.xml";
                var result = sut.GetOrCreateList(elements, schema);

                result.ItemCount.Should().Be(1);
                result.Items.Count.Should().Be(1);

                var defaultItem = result.Items[0];

                defaultItem.ID.Should().Be(1);
                defaultItem[SPBuiltInFieldId.ID].Should().Be(1);
                defaultItem[SPBuiltInFieldId.Title].Should().Be("MyTitle");
                defaultItem["String Column"].Should().Be("MyString");
                defaultItem[new Guid("{3ca9275d-7289-4ed4-86b8-9fe0d8921330}")].Should().Be(new DateTime(1999, 12, 31));
                defaultItem["User Field"].Should().BeOfType<SPUser>();
                var user = (SPUser)defaultItem["User Field"];
                user.ID.Should().Be(1);
                defaultItem["Lookup Field"].Should().Be(1);
            }
        }
    }
}
