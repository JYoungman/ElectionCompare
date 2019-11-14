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
        }

        public int GetVoteForCandidate(int candidateIdx)
        {
            return votes[candidateIdx];
        }
    }
}
