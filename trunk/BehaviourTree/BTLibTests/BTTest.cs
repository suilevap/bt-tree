using System;
using BT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BTLibTests
{
    [TestClass]
    public class BtTest
    {
        class TestExecutionContext : IBlackboard
        {
            public int[] SomeData = new int[10];

            public void Reset()
            {
            }

            public void RunningActionChanged<T>(Node<T> runningNode, Path<T> path) where T : IBlackboard
            {
            }

            public void Update(TimeSpan time)
            {
            }
        }

        [TestMethod]
        public void TestNodeContext()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            TestExecutionContext testData = new TestExecutionContext();


            int times = 3;
            var root = bt.While("While",
                (x, c) =>
                {
                    c.Data = times;
                },
                (x, c) =>
                {
                    int counter = c.GetData<int>();
                    counter--;
                    c.Data = counter;
                    var result = counter >= 0 ? Status.Running : Status.Ok;
                    return result;
                },
                bt.Action("Plus",
                    (x, c) => x.SomeData[0] == 0,
                    (x, c) => true,
                    (x, c) => x.SomeData[0]++,
                    (x, c) => true)
                 )
            ;

            var brain = bt.CreateContext(root, testData);
            Status status = Status.Running;
            int steps = 0;
            while (status == Status.Running)
            {
                status = brain.Update();
                Console.WriteLine(brain);
                steps++;
            }

            //additional step to final counter check
            Assert.AreEqual(times  + 1, steps);
            Assert.AreEqual(Status.Ok, status);
            Assert.AreEqual(times, testData.SomeData[0]);
        }


        [TestMethod]
        public void TestAction()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            TestExecutionContext testData = new TestExecutionContext();

            var root = bt.Action("Test action",
                 (x, c) => x.SomeData[0] == 0,
                 (x, c) => x.SomeData[0] < 3,
                 (x, c) => x.SomeData[0]++,
                 (x, c) => x.SomeData[0] == 3);
            var brain = bt.CreateContext(root, testData);
            Status status = Status.Running;
            int steps = 0;
            while (status == Status.Running)
            {
                status = brain.Update();
                Console.WriteLine(brain);
                steps++;
            }
            Assert.AreEqual(4, steps);
            Assert.AreEqual(Status.Ok, status);
        }

        [TestMethod]
        public void TestCondition()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            TestExecutionContext testData = new TestExecutionContext();

            var root = bt.Condition("Test condition", x => x.SomeData[0] == 1);
            var context = bt.CreateContext(root, testData);
            Status status = context.Update();
            Assert.AreEqual(Status.Fail, status);
            testData.SomeData[0] = 1;
            status = context.Update();
            Assert.AreEqual(Status.Ok, status);
        }

        [TestMethod]
        public void TestSequnce()
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
                    bt.Action("test SomeData[0]", (x, c) => true, (x, c) => testAction(x, 0), null),
                    bt.Action("test SomeData[1]", (x, c) => true, (x, c) => testAction(x, 1), null),
                    bt.Action("test SomeData[2]", (x, c) => true, (x, c) => testAction(x, 2), null)
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
            Assert.IsTrue(steps >= 5);

            Assert.AreEqual(3, testData.SomeData[0]);
            Assert.AreEqual(3, testData.SomeData[1]);
            Assert.AreEqual(3, testData.SomeData[2]);
        }

        [TestMethod]
        public void TestSelector()
        {
            var bt = BT.BTBuilder<TestExecutionContext>.Instance;
            var root =
                bt.Selector("Sel1",
                    bt.Action("test SomeData[0]", (x, c) => x.SomeData[0] > 0, (x, c) => false, null, (x, c) => x.SomeData[3] = 0),
                    bt.Action("test SomeData[1]", (x, c) => x.SomeData[1] > 0, (x, c) => false, null, (x, c) => x.SomeData[3] = 1),
                    bt.Action("test SomeData[2]", (x, c) => x.SomeData[2] > 0, (x, c) => false, null, (x, c) => x.SomeData[3] = 2)
                );
            Console.WriteLine(root);

            TestExecutionContext testData = new TestExecutionContext();
            var brain = bt.CreateContext(root, testData);
            Status status;
            Assert.AreEqual(0, testData.SomeData[3]);
            status = brain.Update();
            Assert.AreEqual(0, testData.SomeData[3]);
            Assert.AreEqual(Status.Fail, status);

            testData.SomeData[1] = 1;
            status = brain.Update();

            Assert.AreEqual(1, testData.SomeData[3]);
            Assert.AreEqual(Status.Ok, status);

            status = brain.Update();
            brain.Update();
            Assert.AreEqual(1, testData.SomeData[3]);
            Assert.AreEqual(Status.Ok, status);

            testData.SomeData[2] = 1;
            status = brain.Update();


            Assert.AreEqual(1, testData.SomeData[3]);
            Assert.AreEqual(Status.Ok, status);

            testData.SomeData[0] = 1;
            status = brain.Update();
            Assert.AreEqual(0, testData.SomeData[3]);
            Assert.AreEqual(Status.Ok, status);

        }
    }
}
