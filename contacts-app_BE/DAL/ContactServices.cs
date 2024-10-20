using contacts_app_BE.Models;
using System.Text.Json;

namespace contacts_app_BE.DAL
{
    public class ContactServices
    {
        private readonly string _filePath = "contact-list.json";

        public async Task<List<Contact>> GetAllContact()
        {
            using var reader = new StreamReader(_filePath);
            var json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<List<Contact>>(json);
        }

        public async Task<Contact> GetContactById(int id)
        {
            var contacts = await GetAllContact();
            return contacts.FirstOrDefault(p => p.Id == id);
        }
        public async Task AddContactAsync(Contact contact)
        {
            var contacts = await GetAllContact();
            contact.Id = contacts.Any() ? contacts.Max(p => p.Id) + 1 : 1; // Set a new ID
            contacts.Add(contact);
            await SaveContactsAsync(contacts);
        }
         public async Task<bool> UpdateContactAsync(int id, Contact updatedContact)
        {
            var contacts = await GetAllContact();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return false;

            // Update Contact details
            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.Email = updatedContact.Email;

            await SaveContactsAsync(contacts);
            return true;
        }
        public async Task<bool> DeleteContactAsync(int id)
        {
            var contacts = await GetAllContact();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
           
            if (contact == null) return false;
            contacts.Remove(contact);
            await SaveContactsAsync(contacts);
            return true;
        }

        private async Task SaveContactsAsync(List<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
