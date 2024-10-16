using System.Collections.Generic;
using System.Linq;
using backend_capstone.Models;
using backend_capstone.Repositories;
using backend_capstone.Utils;
using Microsoft.Data.SqlClient;

public class UserProfileRepository : BaseRepository, IUserProfileRepository
{
    public UserProfileRepository(IConfiguration config) : base(config) { }

    public UserProfile GetByEmail(string email)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT up.Id, up.Email, up.Username
                          FROM UserProfile up
                         WHERE Email = @email";

                DbUtils.AddParameter(cmd, "@email", email);

                UserProfile userProfile = null;

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userProfile = new UserProfile()
                    {
                        Id = DbUtils.GetInt(reader, "Id"),
                        Username = DbUtils.GetString(reader, "Username"),
                        Email = DbUtils.GetString(reader, "Email")
                    };
                }
                reader.Close();

                return userProfile;
            }
        }
    }

    public void AddUser(UserProfile userProfile)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO UserProfile (Email, Username)
                                        OUTPUT INSERTED.ID
                                        VALUES (@Username, @Email)";
                DbUtils.AddParameter(cmd, "@Username", userProfile.Username);
                DbUtils.AddParameter(cmd, "@Email", userProfile.Email);

                userProfile.Id = (int)cmd.ExecuteScalar();
            }
        }
    }

    public List<UserProfile> GetAll()
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT up.Id, up.Email, up.Username
                          FROM UserProfile up
                               ORDER BY up.Username";


                var reader = cmd.ExecuteReader();

                var users = new List<UserProfile>();

                while (reader.Read())
                {
                    users.Add(new UserProfile()
                    {
                        Id = DbUtils.GetInt(reader, "Id"),
                        Username = DbUtils.GetString(reader, "Username"),
                        Email = DbUtils.GetString(reader, "Email"),
                        
                    });
                }
                reader.Close();

                return users;
            }
        }
    }
    public UserProfile GetById(int id)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT up.Id, up.Email, up.Username, up.DisplayName
                          FROM UserProfile up
                         WHERE up.Id = @Id";

                DbUtils.AddParameter(cmd, "@Id", id);

                UserProfile userProfile = null;

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userProfile = new UserProfile()
                    {
                        Id = DbUtils.GetInt(reader, "Id"),
                        Username = DbUtils.GetString(reader, "Username"),
                        Email = DbUtils.GetString(reader, "Email"),
                       
                    };
                }
                reader.Close();

                return userProfile;
            }
        }
    }

    public void UpdateUser(UserProfile userProfile)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE UserProfile
                                           WHERE Id = @Id";

                DbUtils.AddParameter(cmd, "@Id", userProfile.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteUser(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM UserProfile WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}


