
using ChatServerCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatServer.Controllers
{
    [RoutePrefix("chat")]
    public class ChatController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> login(Contact contact)
        {
            var self = await loginAsync(contact);
            if (self != null)
                return Ok(self);
            return Unauthorized();
        }
        [NonAction]
        public async Task<Contact> loginAsync(Contact contact)
        {
            if (await new ChatService().contactPassword(contact))
            {
                return await new ChatService().getContactByUsername(contact.Username);
            }
            return null;
        }


        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> register(Contact contact)
        {
            var result = await registerAsync(contact);
            if (!result)
                return Conflict();
            return Ok();
        }
        [NonAction]
        public async Task<bool> registerAsync(Contact contact)
        {
            if (!await new ChatService().contactExists(contact))
            {
                if (await new ChatService().addNewContact(contact))
                    return true;
            }
            return false;
        }
        [Route("contacts")]
        [HttpGet]
        public async Task<IHttpActionResult> getContacts()
        {
            return Ok(await new ChatService().getContacts());
        }

        [Route("messages/{selfId}")]
        [HttpGet]
        public async Task<IHttpActionResult> getContactMessages(int selfId)
        {
            return Ok(await new ChatService().getContactMessages(selfId));
        }


        [Route("{selfId}/since/{lastUpdated}")]
        [HttpGet]
        public async Task<IHttpActionResult> fetchPendingItems(int selfId, string lastUpdated)
        {
            var items = await new ChatService().fetchAsync(selfId, lastUpdated);
             return Ok(items);
        }

        [Route("timeasync")]
        [HttpGet]
        public async Task<IHttpActionResult> getTimeAsync()
        {
            DateTime date =
            await Task.Run(() => { }).ContinueWith(a =>
            {
                return DateTime.Now;
            });
            return Ok(date);
        }

        [Route("time")]
        [HttpGet]
        public IHttpActionResult getTime()
        {
            return Ok(DateTime.Now);
        }

        [Route("newmessage")]
        [HttpPost]
        public async Task<IHttpActionResult> postMessage(Message new_message)
        {
            return Ok(await new ChatService().addNewMessage(new_message));
        }
    }
}