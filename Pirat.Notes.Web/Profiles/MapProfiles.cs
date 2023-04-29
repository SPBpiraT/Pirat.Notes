using System;
using AutoMapper;
using Pirat.Notes.DAL.Contracts.Entities;
using Pirat.Notes.DAL.Contracts.Roles;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Notes;
using Pirat.Notes.Domain.Contracts.Models.Users;

namespace Pirat.Notes.Web.Profiles
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<NoteEntity, NoteModel>();

            CreateMap<NoteModel, NoteEntity>();

            CreateMap<NoteUpdateRequest, NoteEntity>();

            CreateMap<UserEntity, UserModel>();

            CreateMap<UserRoleUpdateRequest, UserEntity>();

            // AuthenticateRequest -> UserEntity
            CreateMap<AuthenticateRequest, UserEntity>();

            // UserEntity -> AuthenticateResponse
            CreateMap<UserEntity, AuthenticateResponse>();

            // RegisterRequest -> UserEntity
            CreateMap<RegisterRequest, UserEntity>();

            // UpdateRequest -> UserEntity
            CreateMap<UpdateRequest, UserEntity>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;

                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
    }
}