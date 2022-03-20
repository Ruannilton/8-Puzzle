public abstract class Node<T>{
   
    public T value { get;private set;}
    
    public Node(T value)
    {
        this.value = value;
    }

    public abstract List<Node<T>> GetNeighbours();
}

public enum Status{
    Idle,
    Running,
    Sucess,
    Fail
}

public abstract class Pathfinder<T> where T: IEqualityComparer<T>{
    protected Node<T> start;
    protected Node<T> goal;
    
    protected List<PathNode> openList = new();
    protected List<PathNode> closedList = new();

    public delegate float CostFunction(T a, T b);
    protected CostFunction HeuristicCost { get; set; }
    protected CostFunction NodeTraversalCost { get; set; }
    public Status currentStatus = Status.Running;

    protected PathNode? currentNode = null;
    public class PathNode: IComparer<PathNode>,IComparable<PathNode>{
        public PathNode? parent = null;
        public Node<T> value {get;private set;}
        public float GCost { get; set; }
        public float Hcost { get; private set; }
        public float Fcost { get => GCost+Hcost; }

        public PathNode(Node<T> value,PathNode? parent,float gCost, float hCost){
            this.value = value;
            this.GCost = gCost;
            this.Hcost = hCost;
            this.parent = parent;
        }

        public int Compare(Pathfinder<T>.PathNode? x, Pathfinder<T>.PathNode? y)
        {
            if(x.Fcost > y.Fcost) return 1;
            else if(x.Fcost < y.Fcost) return -1;
            return 0;
        }

        public int CompareTo(PathNode? obj)
        {
           if(Fcost > obj.Fcost) return 1;
            else if(Fcost < obj.Fcost) return -1;
            return 0;
        }
    }

    public Pathfinder(Node<T> start,Node<T> goal,CostFunction heuristic,CostFunction traversal){
        this.start = start;
        this.goal = goal;
        this.HeuristicCost = heuristic;
        this.NodeTraversalCost = traversal;
        
        float h = HeuristicCost(start.value,goal.value);
        PathNode node = new(start,null,0,h);

        AddOpenList(node);
        currentNode = node;
    }
    protected int Find(List<PathNode> list,T value){
        for (int i = 0; i < list.Count; i++)
        {
            if(EqualityComparer<T>.Default.Equals(list[i].value.value,value)){
                return i;
            }
        }
        return -1;
    }
    protected void AddOpenList(PathNode node){
        openList.Add(node);
        openList.Sort();
    }
    protected PathNode GetFirst(){
        PathNode v = openList[0];
        openList.RemoveAt(0);
        return v;
    }
    protected void AddCloseList(PathNode node) => closedList.Add(node);
    protected bool NodeIsGoal(PathNode node){
        T _goal = goal.value;
        T nodeV = node.value.value;

        return _goal.Equals(nodeV,_goal);
    } 
    
    public Status Step(){

        AddCloseList(currentNode);
        
        if(openList.Count == 0) {currentStatus = Status.Fail; return currentStatus;}
        
        currentNode = GetFirst();

        if(NodeIsGoal(currentNode)){currentStatus = Status.Sucess; return currentStatus;}

        List<Node<T>> neighbours = currentNode.value.GetNeighbours();

        foreach (var item in neighbours)
        {
            FindAlgorithm(item);
        }
       
        currentStatus = Status.Running;
        return currentStatus;
    }

    public List<T> GetPath(){
        if(currentStatus == Status.Fail) return new();
        List<T> path = new();
        PathNode? current = closedList.Last();
        while(current!=null){
            path.Add(current.value.value);
            current = current.parent;
        }
        path.Reverse();
        path.Add(goal.value);
        return path;
    }
    protected abstract void FindAlgorithm(Node<T> node);
}