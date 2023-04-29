using System.Collections.Generic;
using AutoMapper;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Notes;
using Pirat.Notes.Domain.Contracts.Models.Users;

namespace Pirat.Notes.Domain.Implementations.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;

        private readonly INoteRepository _noteRepository;

        private readonly INoteService _noteService;

        private readonly IUserRepository _userRepository;

        private readonly IUserService _userService;

        public AdminService(
            IUserService userService,
            INoteService noteService,
            IUserRepository userRepository,
            INoteRepository noteRepository,
            IMapper mapper)
        {
            _userService = userService;

            _noteService = noteService;

            _userRepository = userRepository;

            _noteRepository = noteRepository;

            _mapper = mapper;
        }

        public void DeleteNote(int id)
        {
            _noteService.Delete(id);
        }

        public void DeleteUser(int id)
        {
            _userService.Delete(id);
        }

        public UserModel SetUserRole(int id, UserRoleUpdateRequest request)
        {
            var entity = _userRepository.GetById<UserEntity>(id);

            if (entity != null)
            {
                _mapper.Map(request, entity);

                _userRepository.Update(entity);

                var model = _mapper.Map<UserModel>(entity);

                return model;
            }

            throw new KeyNotFoundException("User not found!");
        }

        public NoteModel UpdateNote(int id, NoteUpdateRequest model)
        {
            var entity = _noteRepository.GetById<NoteEntity>(id);

            _mapper.Map(model, entity);

            _noteRepository.UpdateNote(entity);

            var responce = _mapper.Map<NoteModel>(entity);

            return responce;
        }

        public UserModel UpdateUser(int id, UpdateRequest updateRequest)
        {
            _userService.Update(id, updateRequest);

            var entity = _userRepository.GetById<UserEntity>(id);

            var responce = _mapper.Map<UserModel>(entity);

            return responce;
        }
    }
}