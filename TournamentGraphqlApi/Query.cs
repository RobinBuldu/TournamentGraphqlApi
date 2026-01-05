using System.Collections.Generic;

namespace TournamentGraphqlApi
{
    public class Query
    {
        public List<User> GetUsers() => Database.Users;
        public List<Tournament> GetTournaments() => Database.Tournaments;
    }
}