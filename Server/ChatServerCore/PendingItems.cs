using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerCore
{
    public class PendingItems
    {
        public ICollection<Message> PendingMessages;
        public ICollection<Contact> PendingContacts;

        public bool hasChanged, hasMessages, hasContacts;

        public PendingItems()
        {
            PendingMessages = new List<Message>();
            PendingContacts = new List<Contact>();
            hasChanged = hasContacts = hasMessages = false;
        }

        public void addPendingMessages(ICollection<Message> newMessages)
        {
            if (newMessages?.Count > 0)
            {
                hasChanged = true;
                hasMessages = true;
                PendingMessages.Clear();
                foreach (var msg in newMessages)
                {
                    PendingMessages.Add(msg);
                }
            }
        }
        public void addPendingContacts(ICollection<Contact> newContacts)
        {
            if (newContacts?.Count > 0)
            {
                hasChanged = true;
                hasContacts = true;
                PendingContacts.Clear();
                foreach (var contact in newContacts)
                {
                    PendingContacts.Add(contact);
                }
            }
        }
    }
}
