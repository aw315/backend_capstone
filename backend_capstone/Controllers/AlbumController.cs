using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using backend_capstone.Models;
using backend_capstone.Repositories;

namespace backend_capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        // GET: api/album
        [HttpGet]
        public ActionResult<IEnumerable<Album>> GetAllAlbums()
        {
            var albums = _albumRepository.GetAllAlbums();
            return Ok(albums);
        }

        // GET: api/album/{id}
        [HttpGet("{id}")]
        public ActionResult<Album> GetAlbumById(int id)
        {
            var album = _albumRepository.GetAlbumById(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        // POST: api/album
        [HttpPost]
        public ActionResult<Album> AddAlbum([FromBody] Album album)
        {
            if (album == null || string.IsNullOrWhiteSpace(album.Name))
            {
                return BadRequest("Album name cannot be empty.");
            }

            _albumRepository.AddAlbum(album);
            return CreatedAtAction(nameof(GetAlbumById), new { id = album.Id }, album);
        }

        // PUT: api/album/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateAlbum(int id, [FromBody] Album album)
        {
            if (album == null || album.Id != id)
            {
                return BadRequest("Album ID mismatch.");
            }

            var existingAlbum = _albumRepository.GetAlbumById(id);
            if (existingAlbum == null)
            {
                return NotFound();
            }

            _albumRepository.UpdateAlbum(album);
            return NoContent();
        }

        // DELETE: api/album/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteAlbum(int id)
        {
            var existingAlbum = _albumRepository.GetAlbumById(id);
            if (existingAlbum == null)
            {
                return NotFound();
            }

            _albumRepository.DeleteAlbum(id);
            return NoContent();
        }
    }
}