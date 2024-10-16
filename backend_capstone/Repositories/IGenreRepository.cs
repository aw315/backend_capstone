using backend_capstone.Models;

public interface IGenreRepository
{
    void AddGenre(Genre genre);
    void DeleteGenre(int id);
    List<Genre> GetAllGenres();
    Genre GetGenreById(int id);
    void UpdateGenre(Genre genre);
}