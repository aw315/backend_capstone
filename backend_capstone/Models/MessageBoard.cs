

namespace backend_capstone.Models
{
    public class MessageBoard
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }

    }
}