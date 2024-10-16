using backend_capstone.Models;

public interface IUserProfileRepository
{
    UserProfile GetByEmail(string email);
    void AddUser(UserProfile user);
    void DeleteUser(int id);
    List<UserProfile> GetAll();
    UserProfile GetById(int id);
    void UpdateUser(UserProfile user);
}