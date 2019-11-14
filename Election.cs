using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionCompare
{

    class Election
    {
        protected List<Ballot> ballotList;
        protected List<string> candidateList;

        protected int ballotCount;
        protected int candidateCount;

        public Election(List<Ballot> ballots, List<string> candidates)
        {
            ballotList = ballots;
            candidateList = candidates;

            ballotCount = ballotList.Count;
            candidateCount = candidateList.Count;
        }

        //Default election type is first past the post
        public virtual string DetermineWinner()
        {
            //Initialize vote count
            int[] voteCounts = new int[candidateCount];
            for(int curVote = 0; curVote < candidateCount; curVote++)
            {
                voteCounts[curVote] = 0;
            }

            //Count votes
            foreach(Ballot curBallot in ballotList)
            {
                for(int i = 0; i < candidateCount; i++)
                {
                    if(curBallot.GetVoteForCandidate(i) == 1)
                    {
                        voteCounts[i]++;
                        break;
                    }
                }
            }

            //Determine winner(s)
            int maxVotes = 0;
            for(int candidate = 0; candidate < candidateCount; candidate++)
            {
                maxVotes = Math.Max(maxVotes, voteCounts[candidate]);
            }

            List<int> winnerList = new List<int>();
            for(int potentialWinner = 0; potentialWinner < candidateCount; potentialWinner++)
            {
                if(maxVotes == voteCounts[potentialWinner])
                {
                    winnerList.Add(potentialWinner);
                }
            }

            //Return result
            string result = "Results Under First Past the Post:\n";
            for (int candidateResult = 0; candidateResult < candidateCount; candidateResult++)
            {
                int votesReceived = voteCounts[candidateResult];
                result += candidateList[candidateResult] + ": " + ((float)votesReceived / ballotCount * 100).ToString("0.00") + "%";
                if(votesReceived == maxVotes)
                {
                    result += " Winner";
                    if(winnerList.Count == 1)
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
