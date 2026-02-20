using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private ContactsList contactsList = new ContactsList();
        private string xmlFilePath = "contacts.xml";
        private Contact selectedContact = null;

        public MainWindow()
        {
            InitializeComponent();
            dgContacts.ItemsSource = contactsList.Items;
            txtLastName.Focus();
        }

        // Добавление контакта
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Фамилия и Имя обязательны для заполнения.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Contact newContact = new Contact
            {
                LastName = txtLastName.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                Patronymic = txtPatronymic.Text.Trim(),
                Organization = txtOrganization.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            contactsList.Items.Add(newContact);
            ClearForm();
        }

        // Обновление выбранного контакта
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedContact == null)
            {
                MessageBox.Show("Выберите контакт для редактирования.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedContact.LastName = txtLastName.Text.Trim();
            selectedContact.FirstName = txtFirstName.Text.Trim();
            selectedContact.Patronymic = txtPatronymic.Text.Trim();
            selectedContact.Organization = txtOrganization.Text.Trim();
            selectedContact.Phone = txtPhone.Text.Trim();

            dgContacts.Items.Refresh();
            ClearForm();
        }

        // Очистка формы ввода
        private void BtnClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        // Загрузка из XML
        private void BtnLoadXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    MessageBox.Show("Файл данных не найден.", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                contactsList.Items.Clear();

                XmlSerializer serializer = new XmlSerializer(typeof(ContactsList));
                using (FileStream stream = new FileStream(xmlFilePath, FileMode.Open))
                {
                    ContactsList loaded = (ContactsList)serializer.Deserialize(stream);
                    foreach (var contact in loaded.Items)
                        contactsList.Items.Add(contact);
                }

                dgContacts.Items.Refresh();
                MessageBox.Show($"Загружено контактов: {contactsList.Items.Count}",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сохранение в XML
        private void BtnSaveXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (contactsList.Items.Count == 0)
                {
                    MessageBox.Show("Нет данных для сохранения.", "Внимание",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(ContactsList));
                using (FileStream stream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(stream, contactsList);
                }

                MessageBox.Show($"Сохранено контактов: {contactsList.Items.Count}",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранного контакта
        private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (selectedContact == null)
            {
                MessageBox.Show("Выберите контакт для удаления.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить контакт {selectedContact.LastName} {selectedContact.FirstName}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            int index = contactsList.Items.IndexOf(selectedContact);
            contactsList.Items.Remove(selectedContact);
            ClearForm();

            // Пытаемся выделить соседнюю строку
            if (contactsList.Items.Count > 0)
            {
                if (index < contactsList.Items.Count)
                    dgContacts.SelectedIndex = index;
                else
                    dgContacts.SelectedIndex = contactsList.Items.Count - 1;
            }
        }

        // Очистка всей таблицы
        private void BtnClearTable_Click(object sender, RoutedEventArgs e)
        {
            if (contactsList.Items.Count == 0)
            {
                MessageBox.Show("Таблица уже пуста.", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Очистить всю таблицу ({contactsList.Items.Count} записей)?",
                "Подтверждение очистки",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            contactsList.Items.Clear();
            ClearForm();
        }

        // Выделение строки в таблице
        private void DgContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgContacts.SelectedItem is Contact contact)
            {
                selectedContact = contact;
                txtLastName.Text = contact.LastName;
                txtFirstName.Text = contact.FirstName;
                txtPatronymic.Text = contact.Patronymic;
                txtOrganization.Text = contact.Organization;
                txtPhone.Text = contact.Phone;
            }
        }

        // Очистка полей ввода
        private void ClearForm()
        {
            txtLastName.Clear();
            txtFirstName.Clear();
            txtPatronymic.Clear();
            txtOrganization.Clear();
            txtPhone.Clear();
            selectedContact = null;
            txtLastName.Focus();
        }
    }
}