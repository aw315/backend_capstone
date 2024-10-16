using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using backend_capstone.Repositories;
using backend_capstone.Models;


namespace backend_capstone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageBoardController : ControllerBase
    {
        private readonly IMessageBoardRepository _messageBoardRepository;

        public MessageBoardController(IMessageBoardRepository messageBoardRepository)
        {
            _messageBoardRepository = messageBoardRepository;
        }

        // GET: api/messageboard
        [HttpGet]
        public ActionResult<IEnumerable<MessageBoard>> GetAllMessages()
        {
            var messages = _messageBoardRepository.GetAllMessages();
            return Ok(messages);
        }

        // GET: api/messageboard/{id}
        [HttpGet("{id}")]
        public ActionResult<MessageBoard> GetMessageById(int id)
        {
            var message = _messageBoardRepository.GetMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        // POST: api/messageboard
        [HttpPost]
        public ActionResult<MessageBoard> AddMessage([FromBody] MessageBoard message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            _messageBoardRepository.AddMessage(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = message.Id }, message);
        }

        // PUT: api/messageboard/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateMessage(int id, [FromBody] MessageBoard message)
        {
            if (message == null || message.Id != id)
            {
                return BadRequest("Message ID mismatch.");
            }

            var existingMessage = _messageBoardRepository.GetMessageById(id);
            if (existingMessage == null)
            {
                return NotFound();
            }

            _messageBoardRepository.UpdateMessage(message);
            return NoContent();
        }

        // DELETE: api/messageboard/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteMessage(int id)
        {
            var existingMessage = _messageBoardRepository.GetMessageById(id);
            if (existingMessage == null)
            {
                return NotFound();
            }

            _messageBoardRepository.DeleteMessage(id);
            return NoContent();
        }
    }
}