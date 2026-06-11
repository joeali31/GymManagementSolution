using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.OwnedType;
using GymManagement.DAL.Repositories.Interface;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IGenericRepository<Member> _genericRepository;
        private readonly IGenericRepository<Membership> _memberShipRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public MemberService(IGenericRepository<Member> genericRepository ,
            IGenericRepository<Membership> MemberShipRepository,
            IGenericRepository<HealthRecord> healthRecordRepository,
            IGenericRepository<Booking> BookingRepository)
        {
            _genericRepository = genericRepository;
            _memberShipRepository = MemberShipRepository;
            _healthRecordRepository = healthRecordRepository;
            _bookingRepository = BookingRepository;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await _genericRepository.GetAllAsync(false, ct);

            var membersViewModel = Members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Photo = member.Photo,
                Phone = member.Phone,
                Name = member.Name,
                Email = member.Email,
                Gender = member.Gender.ToString(),
            });

            return membersViewModel;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {

            var PhoneCheck = await _genericRepository.AnyAsync(m => m.Phone == model.Phone , ct);

            var EmailCheck = await _genericRepository.AnyAsync(m => m.Email == model.Email, ct);

            if(PhoneCheck ||  EmailCheck) return false;

            var newMember = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                },
                healthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note,
                }
            };

            var Count = await _genericRepository.AddAsync(newMember);

            return Count > 0;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _genericRepository.GetByIdAsync(memberId);

            if (member is null) return null;

            var memberViewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
                DateOfBirth = member.DateOfBirth.ToString(),
                Gender = member.Gender.ToString(),
                Photo = member.Photo
                
            };

            var memberShip = await _memberShipRepository.FirstOrDefaultEntityAsync(ms => ms.MemberId == memberId && ms.EndDate > DateTime.UtcNow , ct);

            if(memberShip is not null)
            {
                memberViewModel.PlanName = memberShip.Plan.Name;
                memberViewModel.MembershipStartDate = memberShip.CreatedAt.ToString();
                memberViewModel.MembershipEndDate = memberShip.EndDate.ToString();
            }

            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await _healthRecordRepository.FirstOrDefaultEntityAsync(h => h.MemberId == memberId , ct);

            if (healthRecord is null) return null;

            var model = new HealthRecordViewModel()
            {
                BloodType = healthRecord.BloodType,
                Height = healthRecord.Height,
                Note = healthRecord.Note,
                Weight = healthRecord.Weight,
            };

            return model;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _genericRepository.GetByIdAsync(memberId, ct);

            if (member is null) return null;

            var model = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo
            };

            return model;
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _genericRepository.GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var PhoneCheck = await _genericRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            var EmailCheck = await _genericRepository.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);

            if (PhoneCheck || EmailCheck) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;

            var Count = await _genericRepository.UpdateAsync(member);

            return Count > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _genericRepository.GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var hasUpcomingSession = await _bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.UtcNow);

            if(hasUpcomingSession) return false;

            var count = await _genericRepository.DeleteAsync(member);

            return count > 0;
        }

    }
}
