using static System.Console;

MatrixNode Shuffle(MatrixNode start, int c){
    MatrixNode random = start;

    for (int i = 0; i < c; i++)
    {
        var l = random.GetNeighbours();
        int idx = Random.Shared.Next(l.Count);
        random = (MatrixNode) l[idx];
    }
    return random;
}

MatrixNode ordered = new(new());
MatrixNode random =new(Matrix3x3.NewRandom());


float ManhatamDistance (Matrix3x3 a,Matrix3x3 b){
    int distance = 0;

        for(int i =0; i < 9;i++){
            int d1 = a.FindValuePos(i);
            int d2 = b.FindValuePos(i);
        
            distance+= Math.Abs(d2%3 - d1%3) + Math.Abs(d2/3 - d1/3);
        }

    return distance;
}


AStar finder = new(
    random,
    ordered,
    ManhatamDistance,
    (Matrix3x3 a,Matrix3x3 b)=>1.0f
);

int i =0;
do
{
    finder.Step();
    i++;
} while (finder.currentStatus == Status.Running);

if(finder.currentStatus == Status.Sucess){
    WriteLine("Start:");
    WriteLine(random.value);
    List<Matrix3x3> path = finder.GetPath();
    WriteLine($"Steps {path.Count}");
    foreach (var item in path)
    {
        WriteLine(item);
    }
}else{
    WriteLine("No Path");
}