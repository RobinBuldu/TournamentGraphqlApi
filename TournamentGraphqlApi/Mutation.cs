using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using HotChocolate.Authorization;

namespace TournamentGraphqlApi
{
    public class Mutation
    {
        private const string SecretKey = "robin_buldu_super_hidden_key_123456";

        public User RegisterUser(string firstName, string lastName, string email, string password)
        {
            var newUser = new User
            {
                Id = Database.Users.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            Database.Users.Add(newUser);
            return newUser;
        }

        public string Login(string email, string password)
        {
            var user = Database.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new Exception("Invalid email or password!");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        [Authorize]
        public Tournament CreateTournament(string name)
        {
            var newTournament = new Tournament
            {
                Id = Database.Tournaments.Count + 1,
                Name = name,
                StartDate = DateTime.Now,
                Status = "Pending"
            };
            Database.Tournaments.Add(newTournament);
            return newTournament;
        }

        [Authorize]
        public Tournament AddParticipant(int tournamentId, int userId)
        {
            var tournament = Database.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
            var user = Database.Users.FirstOrDefault(u => u.Id == userId);

            if (tournament == null || user == null)
            {
                throw new Exception("Tournament or User not found!");
            }

            tournament.Participants.Add(user);
            return tournament;
        }

        [Authorize]
        public Tournament StartTournament(int tournamentId)
        {
            var tournament = Database.Tournaments.FirstOrDefault(t => t.Id == tournamentId);

            if (tournament == null)
                throw new Exception("Tournament not found!");

            if (tournament.Participants.Count < 2)
                throw new Exception("At least 2 players are required to start the tournament!");

            tournament.Status = "Active";

            for (int i = 0; i < tournament.Participants.Count; i += 2)
            {
                if (i + 1 >= tournament.Participants.Count) break;

                var player1 = tournament.Participants[i];
                var player2 = tournament.Participants[i + 1];

                var newMatch = new Match
                {
                    Id = Database.Matches.Count + 1,
                    TournamentId = tournament.Id,
                    Round = 1, 
                    Player1Id = player1.Id,
                    Player2Id = player2.Id
                };

                Database.Matches.Add(newMatch);
                tournament.Matches.Add(newMatch);
            }

            return tournament;
        }

        [Authorize]
        public Match PlayMatch(int matchId, int winnerId)
        {
            var match = Database.Matches.FirstOrDefault(m => m.Id == matchId);

            if (match == null)
                throw new Exception("Match not found!");

            if (match.Player1Id != winnerId && match.Player2Id != winnerId)
            {
                throw new Exception("The selected winner is not a participant in this match!");
            }

            match.WinnerId = winnerId;
            return match;
        }
    }
}