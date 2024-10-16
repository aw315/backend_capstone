using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using backend_capstone.Models;
using backend_capstone.Repositories;


namespace backend_capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // GET: api/genre
        [HttpGet]
        public ActionResult<IEnumerable<Genre>> GetAllGenres()
        {
            var genres = _genreRepository.GetAllGenres();
            return Ok(genres);
        }

        // GET: api/genre/{id}
        [HttpGet("{id}")]
        public ActionResult<Genre> GetGenreById(int id)
        {
            var genre = _genreRepository.GetGenreById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // POST: api/genre
        [HttpPost]
        public ActionResult<Genre> AddGenre([FromBody] Genre genre)
        {
            if (genre == null || string.IsNullOrWhiteSpace(genre.Name))
            {
                return BadRequest("Genre name cannot be empty.");
            }

            _genreRepository.AddGenre(genre);
            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
        }

        // PUT: api/genre/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateGenre(int id, [FromBody] Genre genre)
        {
            if (genre == null || genre.Id != id)
            {
                return BadRequest("Genre ID mismatch.");
            }

            var existingGenre = _genreRepository.GetGenreById(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            _genreRepository.UpdateGenre(genre);
            return NoContent();
        }

        // DELETE: api/genre/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteGenre(int id)
        {
            var existingGenre = _genreRepository.GetGenreById(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            _genreRepository.DeleteGenre(id);
            return NoContent();
        }
    }
}