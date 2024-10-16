using System.Collections.Generic;
using System.Linq;
using backend_capstone.Models;
using backend_capstone.Repositories;
using backend_capstone.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

public class MessageBoardRepository : BaseRepository, IMessageBoardRepository
{
    public MessageBoardRepository(IConfiguration config) : base(config) { }

    public List<MessageBoard> GetAllMessages()
    {
        using (var conn = Connection)  // Now using the BaseRepository's Connection property
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT p.Id, p.userId, p.message, p.DateCreated
                        FROM MessageBoard p
                        JOIN UserProfile u ON p.UserProfileId = u.Id
                        WHERE p.IsApproved = 1 
                          AND p.PublishDateTime <= GETDATE()
                        ORDER BY p.PublishDateTime DESC"
                ;

                var reader = cmd.ExecuteReader();
                var posts = new List<MessageBoard>();

                while (reader.Read())
                {
                    posts.Add(new MessageBoard
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        userId = reader.GetInt32(reader.GetOrdinal("userId")),
                        Message = reader.GetString(reader.GetOrdinal("message")),
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("dateCreated")),
                    
                       
                    });
                }

                reader.Close();
                return posts;
            }
        }
    }

    public List<MessageBoard> GetMessageByUser(int userId)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                SELECT p.Id, p.userId, p.message, p.DateCreated
                FROM MessageBoard p
                JOIN UserProfile u ON p.UserProfileId = u.Id
                JOIN MessageBoard c ON p.MessageBoardId = c.Id
                WHERE p.UserProfileId = @userId
                ORDER BY p.DateCreated DESC";

                cmd.Parameters.AddWithValue("@userId", userId);

                var reader = cmd.ExecuteReader();
                var posts = new List<MessageBoard>();

                while (reader.Read())
                {
                    posts.Add(new MessageBoard
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        userId = reader.GetInt32(reader.GetOrdinal("userId")),
                        Message = reader.GetString(reader.GetOrdinal("Message")),
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                        

                    });
                }

                reader.Close();
                return posts;
            }
        }
    }
    public MessageBoard GetMessageById(int messageBoardId)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                SELECT p.Id, p.userId, p.message, p.DateCreated
                FROM MessageBoard p
                JOIN UserProfile u ON p.UserProfileId = u.Id
                JOIN MessageBoard c ON p.MessageBoardId = c.Id
                WHERE p.UserProfileId = @userId
                ORDER BY p.DateCreated DESC";

                cmd.Parameters.AddWithValue("@messageBoardId", messageBoardId);

                var reader = cmd.ExecuteReader();
                MessageBoard post = null;

                if (reader.Read())
                {
                    post = new MessageBoard
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        userId = reader.GetInt32(reader.GetOrdinal("userId")),
                        Message = reader.GetString(reader.GetOrdinal("Message")),
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),

                    };
                }

                reader.Close();
                return post;
            }
        }
    }

    public void AddMessage(MessageBoard messageBoard)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        INSERT INTO Post (Message, DateCreated, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Message, @DateCreated, @UserProfileId)";

                cmd.Parameters.AddWithValue("@Message", messageBoard.Message);
                cmd.Parameters.AddWithValue("@DateCreated", messageBoard.DateCreated);
                cmd.Parameters.AddWithValue("@UserProfileId", messageBoard.userId);

                messageBoard.Id = (int)cmd.ExecuteScalar();
            }
        }
    }

    public void UpdateMessage(MessageBoard messageBoard)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                               UPDATE MessageBoard
                               SET 
                                Message = @message
                               WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@name", messageBoard.Message);
                DbUtils.AddParameter(cmd, "@id", messageBoard.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteMessage(int messageBoardId)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM MessageBoard WHERE Id = @messageBoardId";
                cmd.Parameters.AddWithValue("@postId", messageBoardId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    



}
