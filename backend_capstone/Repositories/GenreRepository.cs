using System.Collections.Generic;
using System.Linq;
using backend_capstone.Models;
using backend_capstone.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using backend_capstone.Utils;

public class GenreRepository : BaseRepository, IGenreRepository
{
    public GenreRepository(IConfiguration config) : base(config) { }

    public List<Genre> GetAllGenres()
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT id, name FROM Genre ORDER BY name Asc";
                var reader = cmd.ExecuteReader();

                var genres = new List<Genre>();

                while (reader.Read())
                {
                    genres.Add(new Genre()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                    });
                }

                reader.Close();

                return genres;
            }
        }
    }
    public Genre GetGenreById(int id)
    {
        using (SqlConnection conn = Connection)
        {
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Name, Id FROM Genre WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Genre genre = new Genre
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name")
                        };
                        reader.Close();
                        return genre;

                    }
                    reader.Close();
                    return null;
                }
            }
        }
    }
    public void AddGenre(Genre genre)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Genre (Name) OUTPUT INSERTED.ID VALUES (@name)";

                DbUtils.AddParameter(cmd, "@name", genre.Name);

                int id = (int)cmd.ExecuteScalar();

                genre.Id = id;
            }
        }
    }
    public void UpdateGenre(Genre genre)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                               UPDATE Genre
                               SET 
                                Name = @name
                               WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@name", genre.Name);
                DbUtils.AddParameter(cmd, "@id", genre.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteGenre(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Genre WHERE Id = @id";

                DbUtils.AddParameter(cmd, "@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}