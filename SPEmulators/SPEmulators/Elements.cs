namespace SPEmulators
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;
    using SPEmulators.EmulatedTypes;

    internal class Elements
    {
        XNamespace ns = "http://schemas.microsoft.com/sharepoint/";
        XElement listInstance;

        public Elements(string pathToElementsXml)
        {
            LoadListInstanceFromXml(pathToElementsXml);

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

            return shimList.Instance;
        }

        private void LoadListInstanceFromXml(string pathToElementsXml)
        {
            var xml = XDocument.Load(pathToElementsXml);
            listInstance = xml.Root.Descendants(ns + "ListInstance").First();
        }

        public void AddDefaultData(SPList list)
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

                    var spField = list.Fields.GetFieldByInternalName(name);
                    item[name] = ConvertValueForFieldType(spField.Type, value);
                }

                item.Update();
            }
        }

        private object ConvertValueForFieldType(SPFieldType fieldType, string stringValue)
        {
            switch (fieldType)
            {
                case SPFieldType.DateTime:
                    return DateTime.Parse(stringValue);

                case SPFieldType.Counter:
                case SPFieldType.Integer:
                case SPFieldType.Lookup:
                    return int.Parse(stringValue);

                case SPFieldType.Number:
                    return double.Parse(stringValue);

                case SPFieldType.User:
                    var user = new ShimSPUser()
                    { 
                        IDGet = () => int.Parse(stringValue),
                        LoginNameGet = () => stringValue
                    };

                    return user.Instance;

                case SPFieldType.AllDayEvent:
                case SPFieldType.Attachments:
                case SPFieldType.Boolean:
                case SPFieldType.Calculated:
                case SPFieldType.Choice:
                case SPFieldType.Computed:
                case SPFieldType.ContentTypeId:
                case SPFieldType.CrossProjectLink:
                case SPFieldType.Currency:
                case SPFieldType.Error:
                case SPFieldType.File:
                case SPFieldType.Geolocation:
                case SPFieldType.GridChoice:
                case SPFieldType.Guid:
                case SPFieldType.Invalid:
                case SPFieldType.MaxItems:
                case SPFieldType.ModStat:
                case SPFieldType.MultiChoice:
                case SPFieldType.Note:
                case SPFieldType.OutcomeChoice:
                case SPFieldType.PageSeparator:
                case SPFieldType.Recurrence:
                case SPFieldType.Text:
                case SPFieldType.ThreadIndex:
                case SPFieldType.Threading:
                case SPFieldType.URL:
                case SPFieldType.WorkflowEventType:
                case SPFieldType.WorkflowStatus:
                    break;
            }

            return stringValue;
        }
    }
}
