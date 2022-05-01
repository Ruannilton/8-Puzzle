using System.Diagnostics;
using System.Numerics;
using System.Text;
using Raylib_cs;
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

float ManhatamDistance (Matrix3x3 a,Matrix3x3 b){
    int distance = 0;

        for(int i =0; i < 9;i++){
            int d1 = a.FindValuePos(i);
            int d2 = b.FindValuePos(i);
        
            distance+= Math.Abs(d2%3 - d1%3) + Math.Abs(d2/3 - d1/3);
        }

    return distance;
}

void DrawMatrix(Matrix3x3 matrix3X3, int posX,int posY){
    
    int size = 100;
    int fontSize = 24;

    for (int x = 0; x < 3; x++)
    {
        for (int y = 0; y < 3; y++)
        {
            int px = posX + (x*size);
            int py = posY + (y*size);
            Raylib.DrawRectangle(px,py,(int)size,(int)size,Color.BLACK);
            string text = matrix3X3[y,x]>0?$"{matrix3X3[y,x]}":" ";
            int textW = Raylib.MeasureText(text,fontSize);
            Raylib.DrawText(text,px+(size/2)-textW/2,py+(size/2)-(fontSize/2),fontSize,Color.GREEN);
        }
    }
}

void DrawMiniMatrix(Matrix3x3 matrix3X3, int posX,int posY){
    
    int size = 50;
    int fontSize = 12;

    for (int x = 0; x < 3; x++)
    {
        for (int y = 0; y < 3; y++)
        {
            int px = posX + (x*size);
            int py = posY + (y*size);
            Raylib.DrawRectangle(px,py,(int)size,(int)size,Color.BLACK);
            string text = matrix3X3[y,x]>0?$"{matrix3X3[y,x]}":" ";
            int textW = Raylib.MeasureText(text,fontSize);
            Raylib.DrawText(text,px+(size/2)-textW/2,py+(size/2)-(fontSize/2),fontSize,Color.GREEN);
        }
    }
}

int btnStep = 10;
int btnSize = 100;
Input inpt = new(btnStep,10,185,50,"NUM SHUFFLE");
btnStep+= 195;
Button shuffle = new(btnStep,10,btnSize,50,"SHUFFLE");
btnStep+= btnSize+10;
Button solve = new(btnStep,10,btnSize,50,"SOLVE");
btnStep+= btnSize+10;
Button restart = new(btnStep,10,btnSize,50,"RESTART");
btnStep+= btnSize+10;
Toggle stepByStep = new(btnStep,25,"SHOW STEPS");
btnStep+= 50+10;

Button next = new(btnStep+310,10,btnSize,50,"NEXT STEP");

Queue<Matrix3x3> solvePath = new();

MatrixNode ordered = new(new());
MatrixNode random = new(new());
PuzzleState state = PuzzleState.None;

AStar? finder = null;
int steps = 0;
int shuffleCount = 0;
Matrix3x3 matrixToDraw = random.value;

Node<Matrix3x3>? searchCurrentNode = null;
List<Node<Matrix3x3>> searchNeighboursNode = new();
List<float> searchCosts = new();

Raylib.InitWindow(1024, 480, "Clube das Winx");
WriteLine("Sei que vc vai querer ser");
WriteLine("Uma de nos");
void Reset(){
    steps = 0;
    random = new(new());
    shuffleCount = 0;
    state = PuzzleState.None;
    inpt.Reset();
    matrixToDraw = random.value;
    searchCurrentNode = null;
    searchNeighboursNode = new();
    searchCosts = new();
}


while (!Raylib.WindowShouldClose()){
    

    inpt.Update();
    stepByStep.Update();

    if(restart.Clicked()){
        Reset();
    }

    switch (state)
    {
        case PuzzleState.None:
            if(shuffle.Clicked()){
                shuffleCount = int.Parse(inpt.GetText);
                matrixToDraw = random.value;
                state = PuzzleState.Shuffling;
            }
            if(solve.Clicked()){
                steps = 0;
                finder = new(random,ordered,ManhatamDistance,(Matrix3x3 a,Matrix3x3 b)=>1.0f);
                state = PuzzleState.Solving;
            }
            break;

        case PuzzleState.Shuffling:
            random = Shuffle(random,1);
            shuffleCount--;
            Thread.Sleep(15);
            matrixToDraw = random.value;
            if(shuffleCount == 0){
                state = PuzzleState.None;
            }
            break;
        case PuzzleState.Solving:
            if(finder!=null){
                if(stepByStep.Checked && next.Clicked() || !stepByStep.Checked){
                   if(!stepByStep.Checked) Thread.Sleep(100);
                    finder.Step((Node<Matrix3x3> current,List<Node<Matrix3x3>> neighbours,List<float> costs)=>{
                        searchCurrentNode = current;
                        searchNeighboursNode = neighbours;
                        searchCosts = costs;
                    });
                    if(finder.currentStatus != Status.Running){
                        solvePath = new(finder.GetPath());
                        steps = solvePath.Count;
                        state = PuzzleState.Solved;
                    }
                }
            }else state = PuzzleState.None;
            break;
        case PuzzleState.Solved:
            matrixToDraw = solvePath.Dequeue();
            Thread.Sleep(500);
            if(solvePath.Count == 0) Reset();
            break;
    }



    
    
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    shuffle.Draw();
    solve.Draw();
    restart.Draw();
    inpt.Draw();
    stepByStep.Draw();
    Raylib.DrawText($"Total Steps: {steps}",btnStep + 50,25,16,Color.BLACK);
    Raylib.DrawText($"Left Steps: {solvePath.Count}",btnStep + 200,25,16,Color.BLACK);
    if(state == PuzzleState.Solving){
         Raylib.DrawText("Solving ...",10, 438,12,Color.BLACK);     
    }
    Raylib.DrawText($"Memory Allocated: {Process.GetCurrentProcess().PrivateMemorySize64/(1024*1024)} Mb",10, 450,12,Color.BLACK);
    DrawMatrix(matrixToDraw,10,80);

    if(state == PuzzleState.Solving){
        if(stepByStep.Checked)next.Draw();
        if(searchCurrentNode!=null){
            DrawMiniMatrix(searchCurrentNode.value,320,80);
            {
                string txt = "Current State";
                int h = Raylib.MeasureText(txt,12);
                Raylib.DrawText(txt,320+150/2 - h/2,240,12,Color.BLACK);
            }
            
            int c = 0;
            foreach (var n in searchNeighboursNode)
            {
                int x = 320 + c*160;
                DrawMiniMatrix(n.value,x,300);
                string txt = $"{searchCosts[c]}";
                int h = Raylib.MeasureText(txt,12);
                Raylib.DrawText(txt,x+150/2 - h/2,460,12,Color.BLACK);
                c++;
            }
        }
    }
    Raylib.EndDrawing();
}
Raylib.CloseWindow();

enum PuzzleState{
    None,
    Shuffling,
    Solving,
    Solved,
}

