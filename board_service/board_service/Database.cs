using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace board_service
{
    public class Database
    {
        public int lastBoardId = 0;
        public List<Board> boards_ = new List<Board>();
    }

    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
    }
}
