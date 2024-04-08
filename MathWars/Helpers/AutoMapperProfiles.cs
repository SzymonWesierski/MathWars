using AutoMapper;
using MathWars.Entities;
using MathWars.Models;

namespace MathWars.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles() 
		{
			CreateMap<Tasks, TaskToSolveModel>();
			CreateMap<UsersReportsModel, UsersReports>();
			CreateMap<UserAnswer, UserProfileAnswerModel>()
				.ForMember(dest => dest.DifficultyLevel, opt =>
					opt.MapFrom(src => src.Task.DifficultyLevel))
				.ForMember(dest => dest.Content, opt =>
					opt.MapFrom(src => src.Task.Content));
        }
	}
}
