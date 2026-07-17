using AutoMapper;
using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;


namespace GymManagement.BLL.Services.Classes
{
    public class MembershipService(IUnitOfWork unitOfWork , IMapper mapper , IMembershipRepository membershipRepository) : IMembershipService
    {
        public async Task<Result<bool>> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(model.MemberId , ct);
            if (member is null)
                return Result<bool>.NotFound("Member is not Found");

            var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId , ct);
            if (plan is null)
                return Result<bool>.NotFound("plan is not Found");

            if (!plan.IsActive) return Result<bool>.Failed("Plan is not active");

            var hasActive = await unitOfWork.GetRepository<Membership>()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.UtcNow , ct);

            if (hasActive) return Result<bool>.Failed("Member already has an active membership");

            var membership = new Membership()
            {
                MemberId = model.MemberId,
                EndDate = (model.StartDate ?? DateTime.UtcNow).AddDays(plan.DurationDays),
                CreatedAt = DateTime.UtcNow,
                PlanId = plan.Id
            };

            unitOfWork.GetRepository<Membership>().Add(membership);

            var count = await unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Create Membership !");
        }

        public async Task<Result<bool>> DeleteMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var active = await unitOfWork.GetRepository<Membership>()
                .FirstOrDefaultEntityAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);

            if (active is null) return Result<bool>.Failed("Can Not Delete Membership");

            unitOfWork.GetRepository<Membership>().Delete(active);
            var count = await unitOfWork.SaveChangesAsync();

            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Delete Membership !");
        }

        public async Task<Result<IEnumerable<MemberShipViewModel>>> GetAllAsync(CancellationToken ct = default)
        {
            var memberships = await membershipRepository.GetMembershipsWithPlanAndMemberAsync(ct);

            var membershipsMapped = mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);


            return Result<IEnumerable<MemberShipViewModel>>.Ok(membershipsMapped);
        }

        public async Task<IEnumerable<MemberViewDropDown>> GetMembersDropDownAsync(CancellationToken ct = default)
            => mapper.Map<IEnumerable<MemberViewDropDown>>(await unitOfWork.GetRepository<Member>().GetAllAsync(false, ct));

        public async Task<IEnumerable<PlanViewDropDown>> GetPlansDropDownAsync(CancellationToken ct = default)
        => mapper.Map<IEnumerable<PlanViewDropDown>>(await unitOfWork.GetRepository<Plan>().GetAllAsync(false, ct));
    }
}
