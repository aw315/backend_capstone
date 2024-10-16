using backend_capstone.Models;

public interface IMessageBoardRepository
{
    void AddMessage(MessageBoard message);
    void DeleteMessage(int id);
    List<MessageBoard> GetAllMessages();
    MessageBoard GetMessageById(int id);
    void UpdateMessage(MessageBoard message);

    List<MessageBoard> GetMessageByUser(int userId);
}