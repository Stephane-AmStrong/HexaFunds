using Application.DataTransfertObjects;

namespace Application.Models;

public record BroadcastMessage<TDto>(TDto Record) where TDto : IBaseDto;
