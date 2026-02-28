using Application.DataTransfertObjects;
using Application.DataTransfertObjects.Requests;

namespace Application.Models;

public record BroadcastMessage<TDto>(TDto Record) where TDto : IBaseDto;
