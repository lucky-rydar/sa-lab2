using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace board_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        Database db_;

        public BoardController(Database db)
        {
            db_ = db;
        }

        [HttpPost("create")]
        public ActionResult create([FromBody] CreateParameters parameters)
        {
            var board = new Board()
            {
                Id = db_.lastBoardId++,
                Name = parameters.Name,
                OwnerId = parameters.OwnerId
            };
            db_.boards_.Add(board);
            return Ok(board.Id);
        }

        [HttpPost("edit")]
        public ActionResult edit([FromBody] EditParameters parameters)
        {
            var board = db_.boards_.FirstOrDefault(b => b.Id == parameters.Id);
            if (board == null)
            {
                return BadRequest();
            }
            board.Name = parameters.NewName;
            return Ok();
        }

        [HttpPost("delete")]
        public ActionResult delete([FromBody] DeleteParameters parameters)
        {
            var board = db_.boards_.FirstOrDefault(b => b.Id == parameters.Id);
            if (board == null)
            {
                return BadRequest();
            }
            db_.boards_.Remove(board);
            return Ok();
        }

        [HttpGet("get_boards")]
        public ActionResult getBoards([FromBody] GetBoardsParameters parameters)
        {
            var boards = db_.boards_.Where(b => b.OwnerId == parameters.OwnerId);
            return Ok(boards);
        }

        public class CreateParameters
        {
            public int OwnerId { get; set; }
            public string Name { get; set; }
        }

        public class EditParameters
        {
            public int Id { get; set; }
            public string NewName { get; set; }
        }

        public class DeleteParameters
        {
            public int Id { get; set; }
        }

        public class GetBoardsParameters
        {
            public int OwnerId { get; set; }
        }
    }
}
