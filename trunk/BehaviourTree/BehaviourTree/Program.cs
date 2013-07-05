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
            TestCondition();
            TestAction();
            TestSequnce();
            TestSelector();
        }

        class TestExecutionContext : IBlackboard
        {
            public int[] SomeData = new int[10];

            public void Update(TimeSpan time)
            {
            }
        }

        static private void TestAction()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            TestExecutionContext testData = new TestExecutionContext();

            var root = bt.Action("Test action",
                x => x.SomeData[0] == 0,
                x=> x.SomeData[0]<3,
                x => x.SomeData[0]++,
                x => x.SomeData[0] == 3);
            var brain = bt.CreateContext(root, testData);
            Status status = Status.Running;
            int steps = 0;
            while (status == Status.Running)
            {
                status = brain.Update();
                Console.WriteLine(brain);
                steps++;
            }
            Debug.Assert(steps == 4);
            Debug.Assert(status == Status.Ok);
        }

        static private void TestCondition()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            TestExecutionContext testData = new TestExecutionContext();

            var root = bt.Condition("Test condition", x => x.SomeData[0] == 1);
            var context = bt.CreateContext(root, testData);
            Status status = context.Update();
            Debug.Assert(status == Status.Fail);
            testData.SomeData[0] = 1;
            status = context.Update();
            Debug.Assert(status == Status.Ok);
        }

        static private void TestSequnce()
        {
            Func<TestExecutionContext, int, bool> testAction = (TestExecutionContext x, int i) =>
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
                };
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            var root =
                bt.Sequence("Seq1",
                    bt.Action("test SomeData[0]", x => true, x => testAction(x, 0), null),
                    bt.Action("test SomeData[1]", x => true, x => testAction(x, 1), null),
                    bt.Action("test SomeData[2]", x => true, x => testAction(x, 2), null)
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
            Debug.Assert(steps >= 5);

            Debug.Assert(testData.SomeData[0] == 3);
            Debug.Assert(testData.SomeData[1] == 3);
            Debug.Assert(testData.SomeData[2] == 3);
        }


        static private void TestSelector()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            var root =
                bt.Selector("Sel1",
                    bt.Action("test SomeData[0]", x => x.SomeData[0] > 0, x=> false, null,  x => x.SomeData[3] = 0),
                    bt.Action("test SomeData[1]", x => x.SomeData[1] > 0, x => false, null, x => x.SomeData[3] = 1),
                    bt.Action("test SomeData[2]", x => x.SomeData[2] > 0, x => false, null, x => x.SomeData[3] = 2)
                );
            Console.WriteLine(root);

            TestExecutionContext testData = new TestExecutionContext();
            var brain = bt.CreateContext(root, testData);
            Status status;
            Debug.Assert(testData.SomeData[3] == 0);
            status = brain.Update();
            Debug.Assert(testData.SomeData[3] == 0);
            Debug.Assert(status == Status.Fail);

            testData.SomeData[1] = 1;
            status = brain.Update();
            
            Debug.Assert(testData.SomeData[3] == 1);
            Debug.Assert(status == Status.Ok);

            status = brain.Update();
            brain.Update();
            Debug.Assert(testData.SomeData[3] == 1);
            Debug.Assert(status == Status.Ok);

            testData.SomeData[2] = 1;
            status = brain.Update();


            Debug.Assert(testData.SomeData[3] == 1);
            Debug.Assert(status == Status.Ok);

            testData.SomeData[0] = 1;
            status = brain.Update();
            Debug.Assert(testData.SomeData[3] == 0);
            Debug.Assert(status == Status.Ok);

        }


    }
}
