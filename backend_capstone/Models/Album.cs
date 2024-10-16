using System;
using System.Collections.Generic;
using System.Linq;


namespace backend_capstone.Models
{
    public class Album
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string Name { get; set; }
        public int genreId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artwork { get; set; }
        public int Rating { get; set; }
        public bool IsNoSkip { get; set; }
        public string Notes { get; set; }
    }
}