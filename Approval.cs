using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionCompare
{
    class Approval : Election
    {
        public Approval(List<Ballot> ballots, List<string> candidates) : base(ballots, candidates)
        {

        }

        //Calculating winner under Approval Voting
        public override string DetermineWinner()
        {
            //Initialize vote count
            int[] voteCounts = new int[candidateCount];
            for (int curVote = 0; curVote < candidateCount; curVote++)
            {
                voteCounts[curVote] = 0;
            }

            //Count votes
            foreach (Ballot curBallot in ballotList)
            {
                for (int i = 0; i < candidateCount; i++)
                {
                    if (curBallot.GetVoteForCandidate(i) != 0)
                    {
                        voteCounts[i]++;
                    }
                }
            }

            //Determine winner(s)
            int maxVotes = 0;
            for (int candidate = 0; candidate < candidateCount; candidate++)
            {
                maxVotes = Math.Max(maxVotes, voteCounts[candidate]);
            }

            List<int> winnerList = new List<int>();
            for (int potentialWinner = 0; potentialWinner < candidateCount; potentialWinner++)
            {
                if (maxVotes == voteCounts[potentialWinner])
                {
                    winnerList.Add(potentialWinner);
                }
            }

            //Return result
            string result = "Results Under Approval:\n";
            for (int candidateResult = 0; candidateResult < candidateCount; candidateResult++)
            {
                int votesReceived = voteCounts[candidateResult];
                result += candidateList[candidateResult] + ": " + ((float)votesReceived / ballotCount * 100).ToString("0.00") + "%";
                if (votesReceived == maxVotes)
                {
                    result += " Winner";
                    if (winnerList.Count == 1)
                    {
                        result += "\n";
                    }
                    else
                    {
                        result += " (Tie)\n";
                    }
                }
                else
                {
                    result += "\n";
                }
            }

            return result;
        }
    }
}
