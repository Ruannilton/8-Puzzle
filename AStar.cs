public class MatrixNode : Node<Matrix3x3>
{
    public MatrixNode(Matrix3x3 value) : base(value)
    {
    }

    public override List<Node<Matrix3x3>> GetNeighbours()
    {
        return value.GetMoves().Select(x => new MatrixNode(x) as Node<Matrix3x3>).ToList();
    }
}
public class AStar : Pathfinder<Matrix3x3>
{
    public AStar(Node<Matrix3x3> start, Node<Matrix3x3> goal, CostFunction heuristic, CostFunction traversal) : base(start, goal, heuristic, traversal)
    {
    }

    protected override void FindAlgorithm(Node<Matrix3x3> node)
    {
        

        if(Find(openList,node.value) == -1 && currentNode!=null){
            float G = currentNode.GCost + NodeTraversalCost(currentNode.value.value,node.value);
            float H = HeuristicCost(node.value,goal.value);
            int id = Find(closedList,node.value);
            
            if(id==-1){
                PathNode pathNode = new PathNode(node,currentNode,G,H);
                AddOpenList(pathNode);
            }else{
                float atualGCost = openList[id].GCost;
                if(G<atualGCost){
                    openList[id].parent = currentNode;
                    openList[id].GCost = G;
                }
            }
        }
    }
}