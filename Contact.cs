using System;
using System.Xml.Serialization;

namespace WpfApp
{
    [Serializable]
    public class Contact
    {
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("Patronymic")]
        public string Patronymic { get; set; }

        [XmlElement("Organization")]
        public string Organization { get; set; }

        [XmlElement("Phone")]
        public string Phone { get; set; }

        public Contact()
        {
            LastName = "";
            FirstName = "";
            Patronymic = "";
            Organization = "";
            Phone = "";
        }
    }
}