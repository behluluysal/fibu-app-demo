using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases
{
    public class ChatsScreenUseCases : IChatsScreenUseCases
    {
        private readonly IChatRepository _chatRepository;
        public ChatsScreenUseCases(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<ChatDto>)> ViewChatsAsync(QueryParams qp)
        {
            return await _chatRepository.GetAsync(qp);
        }

        public async Task<ChatDto> CreateChatAsync(ChatDto chat)
        {
            return await _chatRepository.CreateAsync(chat);
        }

        public async Task<ChatDto> ViewChatByIdAsync(string id)
        {
            return await _chatRepository.GetByIdAsync(id);
        }

        public async Task UpdateChatAsync(string id, ChatDto chat)
        {
            await _chatRepository.UpdateAsync(id, chat);
        }

        public async Task DeleteChatAsync(string id)
        {
            await _chatRepository.DeleteAsync(id);
        }

        #endregion
    }
}
