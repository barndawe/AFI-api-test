namespace AnimalFriends.Domain.Common;

public interface IValidatable
{
    void ThrowIfInvalid();
}