namespace SPEmulators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;
    using SPEmulators.EmulatedTypes;

    internal class Schema
    {
        XNamespace ns = "http://schemas.microsoft.com/sharepoint/";
        XElement metaData;

        public Schema(string pathToSchemaXml)
        {
            LoadMetaDataFromXml(pathToSchemaXml);
        }

        public void AddFieldsToList(SPList list)
        {
            var fields = metaData.Descendants(ns + "Fields").Descendants(ns + "Field");
            foreach (var field in fields)
            {
                AddFieldToList(field, list);
            }
        }

        private void AddFieldToList(XElement field, SPList list)
        {
            var name = field.Attribute("Name").Value;
            var id = new Guid(field.Attribute("ID").Value);
            var displayName = field.Attribute("DisplayName").Value;
            var type = (SPFieldType)Enum.Parse(typeof(SPFieldType), field.Attribute("Type").Value);
            var required = GetBoolean(field, "Required");

            var fieldName = list.Fields.Add(displayName, type, required);
            var spfield = list.Fields.GetField(fieldName);

            new ShimSPField(spfield)
            {
                InternalNameGet = () => name, 
                IdGet = () => id
            };

        }

        private static bool GetBoolean(XElement field, string name)
        {
            if (field.Attribute(name) == null)
                return false;

            return bool.Parse(field.Attribute(name).Value);
        }

        private void LoadMetaDataFromXml(string pathToSchemaXml)
        {
            var xml = XDocument.Load(pathToSchemaXml);
            metaData = xml.Root.Descendants(ns + "MetaData").First();
        }
    }
}
