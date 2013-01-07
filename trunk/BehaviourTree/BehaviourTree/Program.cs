using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BT;

namespace BehaviourTree
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSequnce();
        }

        class TestExecutionContext
        {
            public int[] SomeData = new int[10];
        }
        static private void TestSequnce()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            var root =
                bt.Sequence("Seq1",
                    bt.Action("test SomeData[0]", x => true, x => TestAction(x, 0)),
                    bt.Action("test SomeData[1]", x => true, x => TestAction(x, 1)),
                    bt.Action("test SomeData[2]", x => true, x => TestAction(x, 2))
                );
            Console.WriteLine(root);

            TestExecutionContext testData = new TestExecutionContext();
            var brain = bt.CreateContext(root, testData);
            Status status = Status.Running;
            int steps = 0;
            while (status == Status.Running)
            {
                status = brain.Update();
                Console.WriteLine(brain);
                steps++;
            }
            Debug.Assert(steps == 10);

            Debug.Assert(testData.SomeData[0] == 3);
            Debug.Assert(testData.SomeData[1] == 3);
            Debug.Assert(testData.SomeData[2] == 3);
        }

        private static bool TestAction(TestExecutionContext x, int i)
        {
            if (x.SomeData[i] == 3)
            {
                return false;
            }
            else
            {
                x.SomeData[i]++;
                return true;
            }
        }
    }
}
