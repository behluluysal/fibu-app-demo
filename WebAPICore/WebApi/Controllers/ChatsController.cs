using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Models.Chats;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Fluent_Validation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidateAttribute]
    public class ChatsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ChatsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.Chats.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Chats.Count().ToString(),
                chats = _mapper.Map<List<ChatDto>>(await _db.Chats.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Chats.Create)]
        public async Task<ApiResponse> Create([FromBody] ChatDto chat)
        {
            Chat _chatDto = _mapper.Map<Chat>(chat);
            await _db.Chats.AddAsync(_chatDto);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<ChatDto>(_chatDto), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Chats.View)]
        public async Task<ApiResponse> GetById([FromRoute] string id)
        {
            Chat chat = await _db.Chats.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (chat == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<ChatDto>(chat));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.Chats.Edit)]
        public async Task<ApiResponse> Put(string id, ChatDto chat)
        {
            Chat existingChatDto = await _db.Chats.FindAsync(id);
            if (existingChatDto == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingChatDto.IsNewMessage = chat.IsNewMessage;

            _db.Update(existingChatDto);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Chats.Delete)]
        public async Task<ApiResponse> Delete(string id)
        {
            var chat = await _db.Chats.FindAsync(id);
            if (chat == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.Chats.Remove(chat);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
