using System;
using System.Collections.Generic;
using AutoMapper;
using Elasticsearch.Net;
using Microsoft.Extensions.Caching.Memory;
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

        private readonly IMemoryCache _cache;

        public NoteService(INoteRepository noteRepository, 
            IMapper mapper, 
            IDateTimeProvider dateTimeProvider,
            IMemoryCache cache)
        {
            _mapper = mapper;

            _noteRepository = noteRepository;

            _dateTimeProvider = dateTimeProvider;

            _cache = cache;

        }

        public List<NoteModel> GetAll()
        {
            _cache.TryGetValue("notes", out List<NoteEntity> col);

            if (col == null)
            {
                col = _noteRepository.GetAll();

                if (col != null)
                {
                    _cache.Set("notes", col, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }

            var result = _mapper.Map<List<NoteModel>>(col);

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