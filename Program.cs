using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ElectionCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Count<string>() != 1)
            {
                Console.Write("To simulate an election, drag and drop a correctly formatted csv file onto this executable.");
                return;
            }
            else
            {
                //Parse the election data
                List<string[]> dataStrings = new List<string[]>();
                foreach (string line in File.ReadLines(args[0]))
                {
                    dataStrings.Add(line.Split(','));
                }

                //Store the names of the candidates
                List<string> candidates = new List<string>();
                foreach(string[] dataLine in dataStrings)
                {
                    candidates.Add(dataLine[0]);
                }

                //Generate the ballots
                int ballotCount = dataStrings[0].Length - 1;
                int candidateCount = candidates.Count;
                List<Ballot> ballots = new List<Ballot>();

                for(int curBallot = 1; curBallot < ballotCount; curBallot++)
                {
                    List<int> tempBallot = new List<int>();
                    for(int curVoteValue = 0; curVoteValue < candidateCount; curVoteValue++)
                    {
                        tempBallot.Add(Int32.Parse(dataStrings[curVoteValue][curBallot]));
                    }

                    ballots.Add(new Ballot(tempBallot));
                }

                //Run the elections
                List<Election> electionTypes = new List<Election>();
                List<string> electionResults = new List<string>();
                electionTypes.Add(new Election(ballots, candidates));   //First past the post
                electionTypes.Add(new Approval(ballots, candidates));   //Approval voting
                electionTypes.Add(new RCV(ballots, candidates));        //Ranked choice voting
                foreach(Election voteCalc in electionTypes)
                {
                    electionResults.Add(voteCalc.DetermineWinner());
                    Console.WriteLine(voteCalc.DetermineWinner());
                }

                //Output results to a text file
                string curTime = DateTime.Now.TimeOfDay.ToString();
                curTime = curTime.Replace(":", "");
                curTime = curTime.Replace(".", "");
                File.WriteAllLines("output"+curTime+".txt", electionResults);
            }
        }
    }
}
