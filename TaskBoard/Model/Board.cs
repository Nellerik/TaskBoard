using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Model
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Model.Task> Tasks { get; set; } = new();
    }
}
