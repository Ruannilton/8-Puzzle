using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.Console;

enum Direction
{
    Up,Right,Bottom,Left,None
}

public record struct Matrix3x3: IEqualityComparer<Matrix3x3>{
    int[] arr =  new[]{0,1,2,3,4,5,6,7,8};
  
    public static readonly Dictionary<int,int[]> ValidMoves = new(){
       {0,new[]{1,3}},
       {1,new[]{0,2,4}},
       {2,new[]{1,5}},
       {3,new[]{4,0,6}},
       {4,new[]{3,5,1,7}},
       {5,new[]{4,2,8}},
       {6,new[]{7,3}},
       {7,new[]{6,8,4}},
       {8,new[]{7,5}},
    };

    public Matrix3x3()
    {
    }

    public int this [int x,int y]{
        get => arr[x*3 + y];
        set => arr[x*3 + y] = value;
    }
   
   public override  string ToString()
    {
        StringBuilder sb = new();
        for (int i = 0; i < 3; i++)
        {
            string c0= this[i,0] > 0? this[i,0].ToString(): " ";
            string c1= this[i,1] > 0? this[i,1].ToString(): " ";
            string c2= this[i,2] > 0? this[i,2].ToString(): " ";

            sb.Append($"{c0} {c1} {c2}\n");
        }
        return sb.ToString();
    }

    public List<Matrix3x3> GetMoves(){
        List<Matrix3x3> moves = new();
        int emptyIndex = FindValuePos(0);

        if(emptyIndex!=-1){
            int[] m = ValidMoves[emptyIndex];
            foreach (var pos in m)
            {
                moves.Add(CreateFromMove(emptyIndex,pos));
            }
        }

        return moves;
    }

    public Matrix3x3 CreateFromMove(int from ,int to){
        Matrix3x3 n = this with { arr = (int[])this.arr.Clone()};
        
        int tmp = n.arr[to];
        n.arr[to] = n.arr[from];
        n.arr[from] = tmp;
        return n;
    }

    public int FindValuePos(int v){
        for (int i = 0; i < arr.Length; i++)
        {
            if(v == arr[i] ) return i;
        }
        return -1;
    }
   
  
    public static Matrix3x3 NewRandom(){
        Random rng = new Random();
        Matrix3x3 m = new();
        
        int n = m.arr.Length;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            int value = m.arr[k];  
            m.arr[k] = m.arr[n];  
            m.arr[n] = value;  
        }  
        return m;
    }
   

    public bool Equals(Matrix3x3 a, Matrix3x3 b)
    {
        for (int i = 0; i < 9; i++)
        {
            if(a.arr[i] != b.arr[i])  return false; 
        }
        return true;
    }

    public int GetHashCode([DisallowNull] Matrix3x3 obj)=>obj.GetHashCode();
}