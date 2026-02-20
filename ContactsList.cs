using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace WpfApp
{
    [Serializable]
    [XmlRoot("ContactsCollection")]
    public class ContactsList
    {
        [XmlElement("Contact")]
        public ObservableCollection<Contact> Items { get; set; }

        public ContactsList()
        {
            Items = new ObservableCollection<Contact>();
        }
    }
}