using System;
using System.Collections.Generic;
using TrelloNet;

namespace TrelloReport.Models
{
    public class HomeModel
    {
        public string Message { get; set; }

        public Uri TrelloAuthUrl { get; set; }

        public string TrelloUserKey { get; set; }

        public IEnumerable<Board> Boards { get; set; }
    }
}