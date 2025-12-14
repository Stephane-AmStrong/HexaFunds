using Application.Abstractions.Handlers;
using MCS.WatchTower.WebApi.DataTransferObjects.Requests;
using MCS.WatchTower.WebApi.DataTransferObjects.Responses;

namespace Application.UseCases.Connections.Establish;

public record EstablishConnectionCommand(ConnectionEstablishRequest Request): ICommand<ConnectionResponse>;
