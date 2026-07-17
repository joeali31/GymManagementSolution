using AutoMapper;
using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Session;
using GymManagement.DAL;
using GymManagement.DAL.Enums;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllWithTrainerAndCategoryAsync(ct);

            var sessionMapped = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.Category.Name,
                TrainerName = s.Trainer.Name,
                Description = s.Description,
                EndDate = s.EndDate,
                StartDate = s.StartDate
            });

            foreach (var item in sessionMapped)
            {
                item.AvailableSlots = item.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(item.Id, ct);
            }

            return Result<IEnumerable<SessionViewModel>>.Ok(sessionMapped);
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.ValidationFailed("End date must be later than start date");
            if (model.StartDate <= DateTime.Now) return Result.ValidationFailed("Start date must be greater than the current date");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.ValidationFailed("Capacity between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.ValidationFailed("Trainer does not existe");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.ValidationFailed("Category does not existe");

            var isValid = Enum.TryParse<TrainerSpecialty>(category.Name , true , out var categorySpecialty);

            if(!isValid || trainer.Specialty != categorySpecialty) return Result.ValidationFailed("Trainer does not match with specialty");

            var hasConflict = await _unitOfWork.GetRepository<Session>()
                                                .AnyAsync(s => s.TrainerId == model.TrainerId 
                                                               && model.StartDate < s.EndDate 
                                                               && model.EndDate > s.StartDate );

            if(hasConflict) return Result.ValidationFailed("The trainer is not available on this date");

            var session = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);

            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0 ? Result.Ok() : Result.Failed("Server Failed");
        }


        public async Task<Result<SessionViewModel>> GetSessionDetailsAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategoryAsync(SessionId , ct);
            if (session is null) return Result<SessionViewModel>.NotFound("session is not exist");

            var mappedSession = _mapper.Map<SessionViewModel>(session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(SessionId);

            return Result<SessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategoryAsync(SessionId, ct);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound("session is not exist");

            if(session.EndDate < DateTime.UtcNow) return Result<UpdateSessionViewModel>.Failed("session had completed");

            var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);

            return Result<UpdateSessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result<bool>> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result<bool>.ValidationFailed("End date must be later than start date");
            if (model.StartDate <= DateTime.Now) return Result<bool>.ValidationFailed("Start date must be greater than the current date");

            var session = await _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategoryAsync(SessionId, ct);
            if (session is null) return Result<bool>.NotFound("session is not exist");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result<bool>.ValidationFailed("Trainer does not existe");

            var isValid = Enum.TryParse<TrainerSpecialty>(session.Category.Name, out var specialty);
            if (!isValid || trainer.Specialty != specialty) return Result<bool>.ValidationFailed("Trainer does not match with specialty");

            var hasConflict = await _unitOfWork.GetRepository<Session>()
                                                .AnyAsync(s => s.Id != SessionId
                                                               && s.TrainerId == model.TrainerId
                                                               && model.StartDate < s.EndDate
                                                               && model.EndDate > s.StartDate);

            if (hasConflict) return Result<bool>.ValidationFailed("The trainer is not available on this date");
            
            session.EndDate = model.EndDate;
            session.StartDate = model.StartDate;
            session.TrainerId = model.TrainerId;
            session.Description = model.Description;
            
            //_unitOfWork.GetRepository<Session>().Update(session);

            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0 ? Result<bool>.Ok(true) : Result<bool>.Failed("Server Failed");
        }

        public async Task<Result<bool>> DeleteSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId, ct);
            if (session is null) return Result<bool>.NotFound("Session does not exist.");

            var hasActiveBookings =  session.EndDate > DateTime.UtcNow && await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == SessionId );
            if(hasActiveBookings) return Result<bool>.Failed("Cannot delete a session with active bookings.");

            _unitOfWork.GetRepository<Session>().Delete(session);

            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true) 
                : 
                Result<bool>.Failed("Server Failed.");
        }

        public async Task<IEnumerable<CategoryViewDropDown>> GetCategoriesDropDownAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<CategoryViewDropDown>>(await _unitOfWork.GetRepository<Category>().GetAllAsync(false , ct));


        public async Task<IEnumerable<TrainerViewDropDown>> GetTrainersDropDownAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<TrainerViewDropDown>>(await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct));
    }
}
