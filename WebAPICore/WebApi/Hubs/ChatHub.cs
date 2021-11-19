using AutoWrapper.Filters;
using Core.Models.Chats;
using DataStore.EF.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ChatHub(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }

        [AutoWrapIgnore]
        public async Task SendMessage(Message message)
        {
            
            using (var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (message.AdminMessage == false && message.Chat.IsNewMessage == false)
                {
                    Chat chat = _db.Chats.Where(x=>x.Id == message.ChatId).FirstOrDefault();
                    chat.IsNewMessage = true;
                    _db.Update(chat);
                }
                _db.Messages.Add(new Message {
                    SentTime = message.SentTime,
                    AdminMessage = message.AdminMessage,
                    ChatId = message.ChatId,
                    MessageText = message.MessageText,
                });
                await _db.SaveChangesAsync();
                await _db.DisposeAsync();
            }
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        [AutoWrapIgnore]
        public async Task ReadMessage(string chatId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                Chat chat = _db.Chats.Where(x => x.Id == chatId).FirstOrDefault();
                chat.IsNewMessage = false;
                _db.Update(chat);
                await _db.SaveChangesAsync();
                await _db.DisposeAsync();
            }
        }

    }
}
