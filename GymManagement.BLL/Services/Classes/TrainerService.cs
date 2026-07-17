using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainer;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService(IUnitOfWork unitOfWork , IMapper mapper) : ITrainerService
    {

        public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainerAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepository<Trainer>().GetAllAsync(false , ct);

            var trainersMapped = mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

            return Result<IEnumerable<TrainerViewModel>>.Ok(trainersMapped);
        }

        public async Task<Result<bool>> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var PhoneCheck = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone, ct);

            var EmailCheck = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email, ct);

            if (PhoneCheck || EmailCheck) return Result<bool>.Failed("Phone number Or Email has been exist!");

            var trainer = mapper.Map<Trainer>(model);

            unitOfWork.GetRepository<Trainer>().Add(trainer);

            var count = await unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Create Trainer !");
                
        }

        public async Task<Result<TrainerViewModel?>> GetTrainerDetailsAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return Result<TrainerViewModel>.NotFound("Trainer is not found")!;

            var trainerMapped = mapper.Map<TrainerViewModel>(trainer);

            return Result<TrainerViewModel>.Ok(trainerMapped)!;
        }

        public async Task<Result<UpdateTrainerViewModel?>> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return Result<UpdateTrainerViewModel>.NotFound("Trainer is not found")!;

            var trainerMapped = mapper.Map<UpdateTrainerViewModel>(trainer);

            return Result<UpdateTrainerViewModel>.Ok(trainerMapped)!;
        }

        public async Task<Result<bool>> UpdateTrainerAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return Result<bool>.NotFound("Trainer is not found");

            var PhoneCheck = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct);
            var EmailCheck = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct);

            if (PhoneCheck || EmailCheck) return Result<bool>.Failed("Phone number Or Email has been exist!");

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Specialty = model.Specialty;

            unitOfWork.GetRepository<Trainer>().Update(trainer);

            var count = await unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Update Trainer !");
        }

        public async Task<Result<bool>> DeleteTrainerAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id , ct);

            if (trainer is null)
                 return Result<bool>.NotFound("Trainer is not found");

            var hasSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.UtcNow );

            if (hasSessions)
                return Result<bool>.Failed("Can not Delete a Trainer Has Sessions!");

            unitOfWork.GetRepository<Trainer>().Delete(trainer!);

            var count = await unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Delete Trainer !");
        }
    }
}
