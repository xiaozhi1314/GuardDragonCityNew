using System.Collections.Generic;
using UnityEngine;

public sealed partial class RVOManager 
{
    public RVOAgentBuild RedBuildAgent, BlueBuildAgent;
    public Dictionary<int, RVOAgent> leftSoliderAgent = new Dictionary<int, RVOAgent>();
    public Dictionary<int, RVOAgent> rightSoliderAgent = new Dictionary<int, RVOAgent>();
    public Common.GameState GameState = Common.GameState.Start;

    public GameConfig gameConfig;

}
