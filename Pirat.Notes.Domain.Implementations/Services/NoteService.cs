using System.Collections.Generic;
using AutoMapper;
using Elasticsearch.Net;
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

        private readonly IDateTimeProvider _dateTimeProvider;

        public NoteService(INoteRepository noteRepository, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _mapper = mapper;

            _noteRepository = noteRepository;

            _dateTimeProvider = dateTimeProvider;

        }

        public List<NoteModel> GetAll()
        {
            var collection = _noteRepository.GetAll();

            var result = _mapper.Map<List<NoteModel>>(collection);

            return result;
        }

        public NoteModel GetById(int id)
        {
            var entity = _noteRepository.GetById(id);

            var model = _mapper.Map<NoteModel>(entity);

            return model;
        }

        public void CreateNote(NoteModel note)
        {
            var entity = _mapper.Map<NoteEntity>(note);

            entity.NoteDate = _dateTimeProvider.Now();

            _noteRepository.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = _noteRepository.GetById(id);

            if (entity != null) //fail fast
                _noteRepository.Delete(id);

            else throw new KeyNotFoundException("Note not found");
        }
    }
}