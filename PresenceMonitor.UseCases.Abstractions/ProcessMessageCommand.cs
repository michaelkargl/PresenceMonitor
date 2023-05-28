using MediatR;

// ReSharper disable once ClassNeverInstantiated.Global
public record ProcessMessageCommand(string Message) : IRequest;