namespace TournamentGraphqlApi
{
    public class Match
    {
        public int Id { get; set; }
        public int Round { get; set; } 

        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public int? WinnerId { get; set; } 
        public int TournamentId { get; set; }
    }
}