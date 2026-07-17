using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
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

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;
        private readonly IMembershipRepository _membershipRepository;

        public MemberService(
            IUnitOfWork unitOfWork,
            IAttachmentService attachmentService,
            IMembershipRepository membershipRepository)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _membershipRepository = membershipRepository;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await _unitOfWork.GetRepository<Member>().GetAllAsync(false, ct);

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

            var PhoneCheck = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone , ct);

            var EmailCheck = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);

            if(PhoneCheck ||  EmailCheck) return false;

            var photoName = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream() , "Picture" , model.PhotoFile.FileName, ct);

            if (string.IsNullOrWhiteSpace(photoName)) return false;

            var newMember = new Member()
            {
                Photo = photoName,
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

            _unitOfWork.GetRepository<Member>().Add(newMember);

            var Count = await _unitOfWork.SaveChangesAsync();

            if (Count > 0)
                return true;
            else
            {
                // Delete Photo from folder
                _attachmentService.Delete("Picture", model.PhotoFile.FileName);

                return false;
            }
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId);

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

            var memberShip = await _membershipRepository.GetMembershipsWithPlanAndMemberAsync(ms => ms.MemberId == memberId && ms.EndDate > DateTime.UtcNow , ct);

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
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultEntityAsync(h => h.MemberId == memberId , ct);

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
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

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
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var PhoneCheck = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            var EmailCheck = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);

            if (PhoneCheck || EmailCheck) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;

            _unitOfWork.GetRepository<Member>().Update(member);

            var Count = await _unitOfWork.SaveChangesAsync();

            return Count > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var hasUpcomingSession = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.UtcNow);

            if(hasUpcomingSession) return false;

            // Delete Photo from folder
            _attachmentService.Delete("Picture", member.Photo);

            _unitOfWork.GetRepository<Member>().Delete(member);

            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0;
        }

    }
}
