using CSharpFunctionalExtensions;

namespace NotificationService.Shared.Abstractions
{
	public interface ISender<T>
	{
		Task<Result<T>> Send(T email);
	}
}