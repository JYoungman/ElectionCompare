using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionCompare
{
    class Ballot
    {
        List<int> votes;

        public Ballot(List<int> voteList)
        {
            votes = voteList;

            //Clean up ballots
            RemoveOutOfScopeVotes();
            RemoveRedundantVotes();
        }

        public int GetVoteForCandidate(int candidateIdx)
        {
            return votes[candidateIdx];
        }

        //Set any vote beyond the amount of candidates as zero
        void RemoveOutOfScopeVotes()
        {
            int candidates = votes.Count;
            for(int vote = 0; vote < candidates; vote++)
            {
                if(votes[vote] > candidates)
                {
                    votes[vote] = 0;
                }
            }
        }

        //Set any redundant votes to zero
        void RemoveRedundantVotes()
        {
            List<int> usedVotes = new List<int>();
            int candidates = votes.Count;
            for(int vote = 0; vote < candidates; vote++)
            {
                if(usedVotes.Contains(votes[vote]))
                {
                    votes[vote] = 0;
                }
                else
                {
                    usedVotes.Add(votes[vote]);
                }
            }
        }
    }
}
