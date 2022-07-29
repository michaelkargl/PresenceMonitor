using MediatR;

namespace PresenceMonitor.UseCases.Abstractions;

// ReSharper disable once ClassNeverInstantiated.Global
public record ProcessMessageCommand(string Message) : IRequest;