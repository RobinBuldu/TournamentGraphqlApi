using System;
using System.Collections.Generic;

namespace TournamentGraphqlApi
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Pending";

        public List<User> Participants { get; set; } = new List<User>();

        public List<Match> Matches { get; set; } = new List<Match>();
    }
}