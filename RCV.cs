using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionCompare
{
    class RCV : Election
    {
        int round = 0;
        int[] voteCounts;

        public RCV(List<Ballot> ballots, List<string> candidates) : base(ballots, candidates)
        {
            voteCounts = new int[candidateCount];
            for (int curVote = 0; curVote < candidateCount; curVote++)
            {
                voteCounts[curVote] = 0;
            }
        }

        //Calculating winner under Approval Voting
        public override string DetermineWinner()
        {
            string result = "Results Under Ranked Choice Voting\n";
            bool winnerFound = false;

            //First round of voting
            foreach (Ballot curBallot in ballotList)
            {
                for (int i = 0; i < candidateCount; i++)
                {
                    if (curBallot.GetVoteForCandidate(i) == 1)
                    {
                        voteCounts[i]++;
                        break;
                    }
                }
            }

            //Display current results
            result += "Round " + (round + 1).ToString()+ "\n";
            for (int candidateResult = 0; candidateResult < candidateCount; candidateResult++)
            {
                int votesReceived = voteCounts[candidateResult];
                float votePercentage = ((float)votesReceived / ballotCount * 100);
                result += candidateList[candidateResult] + ": " + votePercentage.ToString("0.00") + "%";

                //Check for a winner
                if (votePercentage > 50.0f)
                {
                    winnerFound = true;
                    result += "Winner\n";
                }
                else
                {
                    result += "\n";
                }
            }

            //Determine winner
            while(!winnerFound)
            {
                //Remove and redistribute votes from lowest performer
                int removeIdx = FindRoundLoser();
                result += "No winner found. Removing " + candidateList[removeIdx] + ".\n";
                
                //Determine which votes needs to be redistributed
                List<Ballot> modifiedBallots = new List<Ballot>();
                foreach(Ballot curBallot in ballotList)
                {
                    if(FindNextPreference(curBallot) == removeIdx)
                    {
                        modifiedBallots.Add(curBallot);
                    }

                }
                voteCounts[removeIdx] = 0;

                //Move votes from round loser to next viable preference
                foreach(Ballot modBallot in modifiedBallots)
                {
                    int nextPref = FindNextPreference(modBallot);
                    if(nextPref != candidateCount + 1)
                    {
                        voteCounts[nextPref]++;
                    }
                }

                //Advance round counter
                round++;

                //Determine new highest vote tally
                int maxVotes = 0;
                for(int candidate = 0; candidate < candidateCount; candidate++)
                {
                    maxVotes = Math.Max(maxVotes, voteCounts[candidate]);
                }

                //Display current results
                result += "\nRound " + (round + 1).ToString() + "\n";
                for (int candidateResult = 0; candidateResult < candidateCount; candidateResult++)
                {
                    int votesReceived = voteCounts[candidateResult];
                    float votePercentage = ((float)votesReceived / ballotCount * 100);
                    result += candidateList[candidateResult] + ": " + votePercentage.ToString("0.00") + "%";
                    //Check for a winner
                    if (votePercentage > 50.0f)
                    {
                        winnerFound = true;
                        result += " Winner\n";
                    }
                    else
                    {
                        result += "\n";
                    }
                }
            }

            return result;
        }

        //Determine round loser
        int FindRoundLoser()
        {
            int minVotes = ballotCount + 1;
            int loserIdx = 0;
            for(int candidate = 0; candidate < candidateCount; candidate++)
            {
                if(voteCounts[candidate] != 0)
                {
                    if(voteCounts[candidate] < minVotes)
                    {
                        minVotes = voteCounts[candidate];
                        loserIdx = candidate;
                    }
                }
            }

            return loserIdx;
        }

        //Determine highest preference from remaining candidates
        int FindNextPreference(Ballot ballot)
        {
            int threshold = candidateCount + 1;
            int preference = threshold;

            for(int i = 0; i < candidateCount; i++)
            {
                if(voteCounts[i] != 0)
                {
                    int votePosition = ballot.GetVoteForCandidate(i);
                    if(votePosition < threshold && votePosition != 0)
                    {
                        threshold = votePosition;
                        preference = i;
                    }
                }
            }

            return preference;
        }
    }
}
