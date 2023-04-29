using System;
using System.Collections.Generic;
using AutoMapper;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;

namespace Pirat.Notes.Domain.Implementations.Services
{
    public class NoteService : INoteService
    {
        private readonly IMapper _mapper;

        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;

            _mapper = mapper;
        }

        public List<NoteModel> GetAll()
        {
            var collection = _noteRepository.GetAll<NoteEntity>();

            var result = _mapper.Map<List<NoteModel>>(collection);

            return result;
        }

        public NoteModel GetById(int id)
        {
            var entity = _noteRepository.GetById<NoteEntity>(id);

            var model = _mapper.Map<NoteModel>(entity);

            return model;
        }

        public void CreateNote(NoteModel note)
        {
            var entity = _mapper.Map<NoteEntity>(note);

            entity.NoteDate = DateTime.Now;

            _noteRepository.AddNote(entity);
        }

        public void Delete(int id)
        {
            var entity = _noteRepository.GetById<NoteEntity>(id);

            if (entity != null)
                _noteRepository.DeleteNote(id);

            else throw new KeyNotFoundException("Note not found");
        }
    }
}