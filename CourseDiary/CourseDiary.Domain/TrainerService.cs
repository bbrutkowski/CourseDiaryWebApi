﻿using CourseDiary.Domain.Interfaces;
using CourseDiary.Domain.Models;
using System.Threading.Tasks;

namespace CourseDiary.Domain
{
    public interface ITrainerService
    {
        Task<bool> AddTrainer(Trainer trainer);
    }

    public class TrainerService : ITrainerService
    {
        private ITrainerRepository _trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<bool> AddTrainer(Trainer trainer)
        {
            return await _trainerRepository.AddTrainer(trainer);
        }
    }
}