using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Users;
using Pirat.Notes.Shared.Authorization;
using Pirat.Notes.Shared.Helpers;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;
using Pirat.Notes.DAL.Contracts.Roles;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Elasticsearch.Net;

namespace Pirat.Notes.Domain.Implementations.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        private IJwtUtils _jwtUtils;

        private readonly IMapper _mapper;

        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(
            IJwtUtils jwtUtils,
            IMapper mapper,
            IUserRepository userRepository, 
            IDateTimeProvider dateTimeProvider)
        {
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _mapper.Map<UserEntity>(model);

            var entity = _userRepository.GetByUsername(user);

            // validate
            if (entity == null || !BCryptNet.Verify(model.Password, entity.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(entity);

            response.Token = _jwtUtils.GenerateToken(entity);

            return response;
        }

        public List<UserModel> GetAll()
        {
            var collection = _userRepository.GetAll();

            var result = _mapper.Map<List<UserModel>>(collection);

            if (result == null || !result.Any()) return new List<UserModel>();
            return result;
        }

        public UserModel GetById(int id)
        {
            var user = _userRepository.GetById(id);

            var model = _mapper.Map<UserModel>(user);

            if (model == null)
                throw new KeyNotFoundException("User not found!");

            return model;
        }

        public void Register(RegisterRequest model)
        {
            if (_userRepository.IsUsernameExist(model.Username))
                throw new AppException("Username " + model.Username + " is already taken");

            // map model to new user object
            var user = _mapper.Map<UserEntity>(model);

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password); //TODO: move to separate type

            user.RegisterDate = _dateTimeProvider.Now(); // untestable, consider using IDateTimeProvider

            user.UserRole = Role.User; 

            //add
            _userRepository.Add(user);
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = _userRepository.GetById(id);

            if (model.Username != user.Username && _userRepository.IsUsernameExist(model.Username))
                throw new AppException("Username " + model.Username + " is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);

            _userRepository.Update(user);
        }

        public void Delete(int id)
        {
            var entity = _userRepository.GetById(id);

            if (entity != null)
            {
                _userRepository.Delete(id);
            }

            else throw new KeyNotFoundException("User not found!");
        }
    }
}