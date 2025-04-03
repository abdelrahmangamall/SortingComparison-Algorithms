using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "SortingComparison2"; } }

        public override void TryMyCode()
        {
            /* WRITE 4~6 DIFFERENT CASES FOR TRACE, EACH WITH
             * 1) SMALL INPUT SIZE
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT FROM THE FUNCTION
             * 4) PRINT THE CASE 
             */
            
            //Already Sorted
            {
                int N = 10;
                int threshold = 2;
                float[] input = { -4, -3, -1, 1, 3, 4, 6, 8, 10, 12 };
                float[] output1 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, threshold);
                float[] output2 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, 1);
                PrintCase(N, threshold, input, output1, output2);
            }
            //Reversely Sorted
            {
                int N = 10;
                int threshold = 3;
                float[] input = { 44, 33, 11, 1, 0, -14, -26, -88, -101, -122 };
                float[] output1 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, threshold);
                float[] output2 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, 1);
                PrintCase(N, threshold, input, output1, output2);
            }
            //Random
            {
                int N = 8;
                int threshold = 4;
                float[] input = { 1.2f, 4.3f, 7.6f, -3.2f, 0.40f, 0.33f, 0.77f, -0.99f};
                float[] output1 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, threshold);
                float[] output2 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, 1);
                PrintCase(N, threshold, input, output1, output2);
            }
            //Duplicates
            {
                int N = 11;
                int threshold = 3;
                float[] input = { 1, 7, 6, 3, 3, 6, 1, 8, 8, 7, 7};
                float[] output1 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, threshold);
                float[] output2 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, 1);
                PrintCase(N, threshold, input, output1, output2);
            }
            //Identical
            {
                int N = 9;
                int threshold = 2;
                float[] input = { 1.321f, 1.321f, 1.321f, 1.321f, 1.321f, 1.321f, 1.321f, 1.321f, 1.321f };
                float[] output1 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, threshold);
                float[] output2 = PROBLEM_CLASS.RequiredFunction((float[])input.Clone(), N, 1);
                PrintCase(N, threshold, input, output1, output2);
            }
        }

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0;
            int threshold = 0;
            string caseType = string.Empty; 
            float[] arr = null;
            float[] actualResult1 = null;
            float[] actualResult2 = null;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }

            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                threshold = br.ReadInt32();
                caseType = br.ReadString();
                arr = new float[N];
                for (int j = 0; j < N; j++)
                {
                    arr[j] = br.ReadSingle();
                }

                actualResult1 = new float[N];
                actualResult2 = new float[N];
                arr.CopyTo(actualResult1, 0);
                arr.CopyTo(actualResult2, 0);

                Stopwatch sw1 = null;
                Stopwatch sw2 = null;

                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            sw1 = Stopwatch.StartNew();
                            actualResult1 = PROBLEM_CLASS.RequiredFunction(actualResult1, N, threshold);
                            sw1.Stop();
                            sw2 = Stopwatch.StartNew();
                            actualResult2 = PROBLEM_CLASS.RequiredFunction(actualResult2, N, 1);
                            sw2.Stop();
                            double speedUp = Math.Round((double)sw2.ElapsedMilliseconds / sw1.ElapsedMilliseconds, 2);
                            if (level == HardniessLevel.Easy)
                                Console.WriteLine($"Test Case {i} ({caseType}): N = {N}, threshold = {threshold}");
                            else
                                Console.WriteLine($"Test Case {i} ({caseType}): N = {N}, threshold = {threshold}, QuickInsert time (ms) = {sw1.ElapsedMilliseconds}, QuickSort time (ms) = {sw2.ElapsedMilliseconds}. \nSpeedup = {speedUp}x");
                        }
                        catch
                        {
                            caseException = true;
                            actualResult1 = null;
                            actualResult2 = null;

                        }
                        caseTimedOut = false;
                    });

                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = br.ReadInt32();
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 1000; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (actualResult1.Length == N && actualResult2.Length == N)   //Passed
                {
                    float[] expectedResult = (float[])arr.Clone();
                    Array.Sort(expectedResult);
                    if (CheckSortingCorrectness(expectedResult, actualResult1) && CheckSortingCorrectness(expectedResult, actualResult2) 
                        && (level == HardniessLevel.Easy || sw1.ElapsedMilliseconds < sw2.ElapsedMilliseconds))
                    {
                        Console.WriteLine("Test Case {0} Passed!", i);
                        correctCases++;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Answer in Case {0}.", i);
                        wrongCases++;
                    }
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    wrongCases++;
                }

                i++;
            }
            s.Close();
            br.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));

        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int N, int threshold, float[] input, float[] output1, float[] output2)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT
             * 4) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine($"N: {N}");

            Console.WriteLine("Input:");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine($"Output of QuickInsertion (threshold = {threshold}):");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(output1[i] + " ");
            }
            Console.WriteLine();
           
            Array.Sort(input);

            Console.WriteLine(CheckSortingCorrectness(input, output1) ? "SORTED" : "WRONG: NOT SORTED");

            Console.WriteLine("Output of QuickSort Only:");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(output2[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine(CheckSortingCorrectness(input, output2) ? "SORTED" : "WRONG: NOT SORTED");
            Console.WriteLine("========================================\n");

        }

        static bool CheckSortingCorrectness(float[] original, float[] result)
        {
            int N = original.Length;
            if (result.Length != N) { return  false; }

            for (int i = 0; i < N; i++)
            {
                if (original[i] != result[i])
                    return false;
            }
            return true;
        }
        static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(-20, 20));
            return (float)(mantissa * exponent);
        }

        #endregion

    }
}
