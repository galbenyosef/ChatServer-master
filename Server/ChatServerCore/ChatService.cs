using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Globalization;
using System.Threading.Tasks;

namespace ChatServerCore
{
    public class ChatService
    {
        const string timeFormat = "dd/MM/yyyy HH:mm:ss";
        public async Task<bool> addNewContact(Contact contact)
        {
            using (var db = new ChatContext())
            {
                try
                {
                    db.Contacts.Add(contact);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        public async Task<bool> contactExists(Contact contact)
        {
            using (var db = new ChatContext())
            {
                try
                {
                    return await db.Contacts.AnyAsync(c => c.Username == contact.Username);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return true;
                }
            }

        }
        public async Task<bool> contactPassword(Contact contact)
        {
            using (var db = new ChatContext())
            {
                if (await getContactByUsername(contact.Username) != null)
                {
                    if ((await getContactByUsername(contact.Username)).Password.Equals(contact.Password))
                        return true;
                }
                return false;
            }
        }
        public async Task<bool> addNewMessage(Message message)
        {
            using (var db = new ChatContext())
            {
                try
                {
                    db.Contacts.Attach(message.Sender);
                    db.Contacts.Attach(message.Receiver);
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        public async Task<List<Contact>> getContacts()
        {
            using (var db = new ChatContext())
            {
                try
                {
                    return await (db.Contacts.ToListAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        public async Task<Contact> getContactByUsername(string username)
        {
            using (var db = new ChatContext())
            {
                return await db.Contacts.SingleOrDefaultAsync(c => c.Username == username);
            }
        }
        public async Task<List<Message>> getContactMessages(int self_id)
        {
            using (var db = new ChatContext())
            {
                try
                {
                    return await db.Messages
                        .Include(msg => msg.Sender)
                        .Include(msg => msg.Receiver)
                        .Where(msg => msg.Sender.ID == self_id || msg.Receiver.ID == self_id).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        public async Task<PendingItems> fetchAsync(int self_id, string lastUpdated)
        {
            PendingItems retval = new PendingItems();
            string timeFormatAPI = "ddMMyyyyHHmmss";
            string lastTime = DateTime.ParseExact(lastUpdated, timeFormatAPI, CultureInfo.CurrentCulture).ToString(timeFormat);
            try
            {
                using (var db = new ChatContext())
                {
                    var newMessages = await (db.Messages
                        .Include(msg => msg.Sender)
                        .Include(msg => msg.Receiver)
                        .Where(msg => msg.Receiver.ID == self_id)
                        .Where(msg => msg.Date.CompareTo(lastTime) == 1)).ToListAsync();
                    retval.addPendingMessages(newMessages);

                    var newContacts = await (db.Contacts
                        .Where(contact => contact.CreatedOn.CompareTo(lastTime) == 1)).ToListAsync();
                    retval.addPendingContacts(newContacts);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
