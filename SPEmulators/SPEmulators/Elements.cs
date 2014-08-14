namespace SPEmulators
{
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class Elements
    {
        XNamespace ns = "http://schemas.microsoft.com/sharepoint/";
        XElement listInstance;

        public Elements(string relativePathToElementsXml)
        {
            LoadListInstanceFromXml(relativePathToElementsXml);
            
            ListTitle = listInstance.Attribute("Title").Value;
        }

        public string ListTitle { get; private set; }

        public string ListDescription
        {
            get
            {
                return listInstance.Attribute("Description").Value;
            }
        }

        public SPListTemplateType ListTemplateType
        {
            get
            {
                return (SPListTemplateType)int.Parse(listInstance.Attribute("TemplateType").Value, (CultureInfo.InvariantCulture));
            }
        }

        public bool OnQuickLaunch
        {
            get
            {
                return bool.Parse(listInstance.Attribute("OnQuickLaunch").Value);
            }
        }

        public SPList CreateListInstance(SPWeb web)
        {
            var id = web.Lists.Add(ListTitle, null, ListTemplateType);
            var list = web.Lists[id];

            var shimList = new ShimSPList(list)
            {
                DescriptionGet = () => ListDescription,
                BaseTemplateGet = () => ListTemplateType,
                OnQuickLaunchGet = () => OnQuickLaunch
            };

            AddDefaultData(list);

            return shimList.Instance;
        }

        private void LoadListInstanceFromXml(string relativePathToElementsXml)
        {
            var xml = XDocument.Load(relativePathToElementsXml);
            listInstance = xml.Root.Descendants(ns + "ListInstance").First();
        }

        private void AddDefaultData(SPList list)
        {
            var defaultDataRows = listInstance.Descendants(ns + "Data").Descendants(ns + "Rows").Descendants(ns + "Row");

            foreach (var row in defaultDataRows)
            {
                var item = list.AddItem();
                var fields = row.Descendants(ns + "Field");

                foreach (var field in fields)
                {
                    var name = field.Attribute("Name").Value;
                    var value = field.Value;

                    item[name] = value;
                }

                item.Update();
            }
        }
    }
}
