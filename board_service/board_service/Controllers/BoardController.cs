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
        BoardDomain boardDomain_;

        public BoardController(Database db)
        {
            boardDomain_ = new BoardDomain(db);
        }

        [HttpPost("create")]
        public ActionResult create([FromBody] CreateParameters parameters)
        {
            try
            {
                int id = boardDomain_.create(parameters.Name, parameters.OwnerId);
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest(-1);
            }
        }

        [HttpPost("edit")]
        public ActionResult edit([FromBody] EditParameters parameters)
        {
            try
            {
                boardDomain_.edit(parameters.Id, parameters.NewName);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("delete")]
        public ActionResult delete([FromBody] DeleteParameters parameters)
        {
            try
            {
                boardDomain_.delete(parameters.Id);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("get_boards")]
        public ActionResult getBoards([FromBody] GetBoardsParameters parameters)
        {
            try
            {
                var boards = boardDomain_.getBoards(parameters.OwnerId);
                return Ok(boards);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
