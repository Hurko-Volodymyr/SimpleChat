namespace SimpleChat.Models.Abstractions.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> CreateUserAsync(User userDto);
    }

}