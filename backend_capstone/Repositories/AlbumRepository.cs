using System.Collections.Generic;
using System.Linq;
using backend_capstone.Models;
using backend_capstone.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using backend_capstone.Utils;

public class AlbumRepository : BaseRepository, IAlbumRepository
{
    public AlbumRepository(IConfiguration config) : base(config) { }

    public List<Album> GetAllAlbums()
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT id, name FROM Album ORDER BY name Asc";
                var reader = cmd.ExecuteReader();

                var albums = new List<Album>();

                while (reader.Read())
                {
                    albums.Add(new Album()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                    });
                }

                reader.Close();

                return albums;
            }
        }
    }
    public Album GetAlbumById(int id)
    {
        using (SqlConnection conn = Connection)
        {
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Name, Id FROM Album WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Album album = new Album
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name")
                        };
                        reader.Close();
                        return album;

                    }
                    reader.Close();
                    return null;
                }
            }
        }
    }
    public void AddAlbum(Album album)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Album (Name) OUTPUT INSERTED.ID VALUES (@name)";

                DbUtils.AddParameter(cmd, "@name", album.Name);

                int id = (int)cmd.ExecuteScalar();

                album.Id = id;
            }
        }
    }
    public void UpdateAlbum(Album album)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                               UPDATE Album
                               SET 
                                Name = @name
                               WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@name", album.Name);
                DbUtils.AddParameter(cmd, "@id", album.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteAlbum(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Album WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
