using backend_capstone.Models;

public interface IAlbumRepository
{
    void AddAlbum(Album album);
    void DeleteAlbum(int id);
    Album GetAlbumById(int id);
    List<Album> GetAllAlbums();
    void UpdateAlbum(Album album);
}