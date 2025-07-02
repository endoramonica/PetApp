using AutoMapper;
using Pet.Core.Application.DTOs;
using Pet.Core.Domain.Entities;

namespace Pet.Core.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserDto>();
            CreateMap<AppUserCreateDto, AppUser>();
            CreateMap<AppUserUpdateDto, AppUser>();
            CreateMap<Address, AddressDto>();
            CreateMap<Pet.Core.Domain.Entities.Pet, PetDto>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
            CreateMap<PetCreateDto, Pet.Core.Domain.Entities.Pet>();
            CreateMap<PetCategory, PetCategoryDto>();
            CreateMap<PetMedicalRecord, PetMedicalRecordDto>();
            CreateMap<PetMedia, PetMediaDto>();
            CreateMap<PetRating, PetRatingDto>();
            CreateMap<AdoptionCart, AdoptionCartDto>();
            CreateMap<AdoptionCartItem, AdoptionCartItemDto>();
            CreateMap<AdoptionRequest, AdoptionRequestDto>();
            CreateMap<AdoptionDetail, AdoptionDetailDto>();
            CreateMap<AdoptionStatus, AdoptionStatusDto>();
            CreateMap<AdoptionCampaign, AdoptionCampaignDto>();
            CreateMap<AdoptionCampaignPet, AdoptionCampaignPetDto>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<Report, ReportDto>();
            CreateMap<UserAdoption, AdoptionRequestDto>().ReverseMap();
            // CreateMap<UserFavorite>().ReverseMap(); chua co class tuong ung ben trong Dto
            CreateMap<ChatMessage, ChatMessageDto>();
            CreateMap<SendMessageRequest, ChatMessage>();
            CreateMap<ChatMessage, ChatHistoryResponse>().ConvertUsing(src => new ChatHistoryResponse { Messages = new List<ChatMessageDto> { new ChatMessageDto { Id = src.Id, SenderId = src.SenderId, ReceiverId = src.ReceiverId, Content = src.Content, SentAt = src.SentAt, IsRead = src.IsRead } } });
        }

    }
}