using AutoMapper;
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagement.BLL.ViewModels.Session;
using GymManagement.BLL.ViewModels.Trainer;
using GymManagement.DAL.Models;
using GymManagement.DAL.OwnedType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.MappProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session , SessionViewModel>()
                .ForMember(d => d.TrainerName , o => o.MapFrom(s => s.Trainer.Name))
                .ForMember(d => d.CategoryName , o => o.MapFrom(s => s.Category.Name));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerViewDropDown>();

            CreateMap<Category, CategoryViewDropDown>();

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(d => d.Address , opt => opt.MapFrom(s => $"{s.Address.City} - {s.Address.Street} - {s.Address.BuildingNumber}"))
                .ForMember(d => d.DateOfBirth , opt => opt.MapFrom(s => s.DateOfBirth.ToString()));
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(d => d.Address , opt => opt.MapFrom(s => new Address()
                {
                    Street = s.Street,
                    City = s.City,
                    BuildingNumber = s.BuildingNumber
                }));

            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(d => d.City , o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street , o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.BuildingNumber , o => o.MapFrom(s => s.Address.BuildingNumber));

            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address()
                {
                    City = s.City,
                    BuildingNumber= s.BuildingNumber,
                    Street = s.Street
                }));

            CreateMap<Membership, MemberShipViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreatedAt));

            CreateMap<Plan, PlanViewDropDown>();
            CreateMap<Member, MemberViewDropDown>();
        }
    }
}
