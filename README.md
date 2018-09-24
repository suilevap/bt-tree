# Behaviour Tree
C# behaviour tree library with a code based API.

## Whats Behaviour Trees is
- [Behaviour tree (Wikipedia)](https://en.wikipedia.org/wiki/Behavior_tree_(artificial_intelligence,_robotics_and_control))
- [Behavior trees for AI: How they work](http://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php)

## How to use
Copy/paste files from [BTLib folder](https://github.com/suilevap/bt-tree/tree/master/trunk/BehaviourTree/BTLib)

Add implementation of [IBlackboard](https://github.com/suilevap/bt-tree/blob/master/trunk/BehaviourTree/BTLib/IBlackboard.cs) interface

For example:

```C#
        class CustomAiBlackboard : IBlackboard
        {
            //some AI related state, for example corodinates of target
            public int TargetX;
            public int TargetY;
            public bool SeeTarget;

            public void Reset()
            {
              //reset state here
            }

            public void RunningActionChanged<T>(Node<T> runningNode, Path<T> path) where T : IBlackboard
            {
              //called when current running node is changed
            }

            public void Update(TimeSpan time)
            {
              //update Ai state for example 
              //UpdateTarget();
              //SeeTarget = CanSeeTarget();
            }
        }
```

Create Behaviour tree using BTBuilder
   
```C#   
        Node<CustomAiBlackboard> _rootNode;
        private void Init()
        {
            var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
            _rootNode =
                bt.Selector("Ai",
                    bt.Sequence("Seq1",
                        bt.Condition("Condtition", blackboard => blackboard.SeeTarget),
                        bt.Action("Action1",
                            actionStart: (blackBoard, nodeContext) =>
                            {
                                bool canMakeAction1 = true;
                                //check that we can start action(executed before once)
                                return canMakeAction1;
                            },
                            checkInProgress: (blackBoard, nodeContext) =>
                            {
                                bool checkThatAction1CanBeContinued = true;
                                //check that action canbe continued(checked every update/tick)
                                return checkThatAction1CanBeContinued;
                            },
                            executionAction: (blackBoard, nodeContext) =>
                            {
                                //do smth usefull here (executed on exeach update/tick while this action is in progress)
                                //for example apply control command
                            },
                            completeAction: (blackBoard, nodeContext) =>
                            {
                                bool isCompleted = false;
                                //checked after isInProgress == false
                                //return true if action completed succeffully 
                                return isCompleted;
                            }
                            ),
                        bt.Action("Action2",
                            (blackBoard, nodeContext) =>
                            {
                                //simple action which executes only once
                                return true;
                            })
                    //end of sequence
                    ),
                    //or else do nothing
                    bt.Idle
            );
            
        }
```

To execute Behaviour tree,
for every AI agent create and store [Context](https://github.com/suilevap/bt-tree/blob/master/trunk/BehaviourTree/BTLib/Context.cs)
and call Update on each update/tick of game loop.

```C#
      Context<CustomAiBlackboard> _btContext;
      
      private void Start()
      {
        _btContext = new BT.Context<Ai.Bt.CustomAiBlackboard>(_rootNode, _blackboard);
      }
      
      private void Update()
      {
          Status statuc = _btContext.Update();
      }
```

## Node type

### Action Node
Implement abstract class [ActionNode<TBlackboard>](https://github.com/suilevap/bt-tree/blob/master/trunk/BehaviourTree/BTLib/ActionNode.cs)

```C#
        /// <summary>
        /// Run on node start 
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True if success and node moves to Status.Running state, False in case of Fail</returns>
        protected internal abstract bool Start(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext);

        /// <summary>
        /// Check if action is complete
        /// </summary>
        /// <param name="blackboard">Balckboard object</param>
        /// <returns>True if node is still in Status.Running state, False - node execution is complete</returns>
        protected internal abstract bool IsInProgress(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext);

        /// <summary>
        /// Run every tick, while node this node in Running State
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>        
        protected internal abstract void Tick(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext);

        /// <summary>
        /// Run on node complete (immediately after Run method returs False )
        /// </summary>
        /// <param name="blackboard">Blackboard object</param>
        /// <returns>True in case of Ok status, false in case of Fail</returns>
        protected internal virtual bool Complete(TBlackboard blackboard, NodeContext<TBlackboard> nodeContext)
        {
            return true;
        }
```
Example for action move:

Start - returns true if path to target exsists, otherwise false

IsInProgress - returns true, if agent position is not equal to target and user can move

Tick - save command to move to target (using blackboard)

Complete - returns true, if agent position is equal to target 

For simple actions you can use an anonymous action which delegates all these function to lambda

```C#
bt.Action("Action1",
    actionStart: (blackBoard, nodeContext) =>
    {
        bool canMakeAction1 = true;
        //check that we can start action(executed before once)
        return canMakeAction1;
    },
    checkInProgress: (blackBoard, nodeContext) =>
    {
        bool checkThatAction1CanBeContinued = true;
        //check that action canbe continued(checked every update/tick)
        return checkThatAction1CanBeContinued;
    },
    executionAction: (blackBoard, nodeContext) =>
    {
        //do smth usefull here (executed on exeach update/tick while this action is in progress)
        //for example apply control command
    },
    completeAction: (blackBoard, nodeContext) =>
    {
        bool isCompleted = false;
        //checked after isInProgress == false
        //return true if action completed succeffully 
        return isCompleted;
    }
    ),
```

### Sequence
Executes nodes in order. Fails if any of them fails, Succeeds only when all are succeed.
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;

bt.Sequence("Seq1", node1, node2, node3);
```

### Selector
Try to executes nodes in order. Fails if all of them fails, Succeeds when atleast one succeeds.
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;

bt.Selector("Selector1", altNode1, altNode2, altNode3);
```

### Condition
Check some condition
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
bt.Condition("Check condition", blackboard=>true);
```
### Optional
Decorator, which executes action node and cannot fail (returns Succeed or Running if action is in progress)
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
bt.Optional(actionNode);
```
### Parallel
Decorator, which executes all childs action node, fails or stopped if any of child action node fails or stopped
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
bt.All("Parallel",actionNode1, actionNode2);
```
#### While
Decorator, which executes child actionNode only while condition return Status.Running
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
bt.While("While check",
  initFunc: (blackboard, nodeContext)=>
  {
      //executes once
  },
  checkFunc: (blackboard, nodeContext)=>
  {
    //executes on every update
    // return Status.Running to continue execution of child node
    // return Status.Running to stop with Succeed
    // return Status.Fail to stop with Fail
    return Status.Running
  },
  actionNode);
```

#### RepeatUntilSuccessDecorator
Decorator, which tries to execute child Node until it returns Status.Ok
```C#
var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
bt.RepeatUntilSuccessDecorator(node);
```

## Advanced 

### Node context

Every node recieve blackboard and node context as arguments during updates.

Blackboard is used to get the state of world or agent.

Node context can be used to store node specific state.

For example, we can implement decorator method which will run child node for specific time only:

```C#
        private Node<CustomAiBlackboard> RepeateDuringSomeTime(float timeInSec, Node<CustomAiBlackboard> childNode)
        {
            var bt = BT.BTBuilder<CustomAiBlackboard>.Instance;
            return bt.While("Repeat during 5 sec",
               initFunc: (blackBoard, nodeContext) =>
               {
                   var startTimeInSec = blackBoard.CurrentTimeInSec;
                   nodeContext.Data = startTimeInSec;
               },
               checkFunc: (blackBoard, nodeContext) =>
               {
                   var currentTimeInSec = blackBoard.CurrentTimeInSec;
                   var startTimeInSec = nodeContext.GetData<float>();
                   if (currentTimeInSec - currentTimeInSec < timeInSec)
                   {
                       return Status.Running;
                   }
                   else
                   {
                       return Status.Ok;
                   }
               },
               node: childNode);
        }
```

### Behaviour tree update
For performance opimization reason you can restrict reevaluation tree on each update, using forceUpdate = false in method "Context.Update(TimeSpan time, bool forceUpdate)" In this case tree will try to execute last running node, and reevaluate tree only if last running node is succeed or fail.

In general for realtime game something like this can be used:

```C#
const float BT_FORCE_UPDATE_IN_SEC = 0.25f;//or any reasonably vallue
float _previousBtUpdateInSec = 0;
void UpdateBt(float gameTimeInSec)
{
    bool needForceUpdate = (gameTimeInSec - _previousBtUpdateInSec) > BT_FORCE_UPDATE_IN_SEC;
    if (nnedForceUpdate)
    {
       _previousBtUpdateInSec = gameTimeInSec;
    }
    _context.Update(gameTime, needForceUpdate);
}
```
In this case only once per 0.25 sec behaviour tree will be forced to be updated(instead of each update), 
but in same time ActionNode.Tick() will be executed during all updates.




