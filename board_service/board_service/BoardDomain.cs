using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace board_service
{
    public class BoardDomain
    {
        Database db_;

        public BoardDomain(Database db)
        {
            db_ = db;
        }

        public int create(string name, int ownerId)
        {
            var board = new Board()
            {
                Id = db_.lastBoardId++,
                Name = name,
                OwnerId = ownerId
            };
            db_.boards_.Add(board);
            return board.Id;
        }

        public void edit(int id, string newName)
        {
            var board = db_.boards_.FirstOrDefault(b => b.Id == id);
            if (board == null)
            {
                throw new NullReferenceException();
            }
            board.Name = newName;
        }

        public void delete(int id)
        {
            var board = db_.boards_.FirstOrDefault(b => b.Id == id);
            if (board == null)
            {
                throw new NullReferenceException();
            }
            db_.boards_.Remove(board);
        }

        public IEnumerable<Board> getBoards(int ownerId)
        {
            var boards = db_.boards_.Where(b => b.OwnerId == ownerId);
            if(boards == null)
            {
                throw new NullReferenceException();
            }
            return boards;
        }
    }
}
