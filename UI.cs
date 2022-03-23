using System.Numerics;
using System.Text;
using Raylib_cs;
using static System.Console;

struct Toggle
{
    string text;
    Rectangle rect;
    Rectangle innerRect;
    private Color innerColor = Color.GREEN;
    private Color backgroundCOlor = Color.GRAY;
    public bool Checked {get; private set; } = false;

    int fontSize = 16;
    public Toggle(int px,int py,string text="",int fontSize = 16){
        rect = new(px,py,20,20);
        innerRect = new(px+2,py+2,16,16);
        this.text = text;
        this.fontSize = fontSize;
    }

     public void Update(){
        if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)){
            Vector2 pos = Raylib.GetMousePosition();
            if( rect.x <= pos.X && pos.X <= rect.x + rect.width && rect.y <= pos.Y && pos.Y <= rect.y + rect.height ){
                Checked =  !Checked;
            }
        }
     }

     public void Draw(){
         Raylib.DrawRectangle((int)rect.x,(int)rect.y,(int)rect.width,(int)rect.height,backgroundCOlor);
         Raylib.DrawRectangle((int)innerRect.x,(int)innerRect.y,(int)innerRect.width,(int)innerRect.height,Checked?innerColor:Color.WHITE);
        Raylib.DrawText(text,(int)(rect.x + rect.width +2),(int)(rect.y + 5),10,Color.BLACK);
     }
}

struct Input
{
    Rectangle rect;
    string text;
    public string GetText {get => textInput.ToString();}
    StringBuilder textInput = new("1");
    bool selected = false;
    public Color backgroundColor = Color.GRAY;
    public Color textColor = Color.BLACK;
    public int fontSize = 16;
    
    public int GetSize {get => s;}
    int s;
    public Input(float x, float y, float widht,float height,string text){
        rect = new(x,y,widht,height);
        s = (int)widht;
        this.text =text;
    }
    public void Reset(){
        textInput = new("1");
    }
    public void Update(){
        if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)){
            Vector2 pos = Raylib.GetMousePosition();
            if( rect.x <= pos.X && pos.X <= rect.x + rect.width && rect.y <= pos.Y && pos.Y <= rect.y + rect.height ){
                selected =  true;
            }else{
                selected = false;
            }
        }
        if(selected){
            
                int key = Raylib.GetKeyPressed();
                if(key == 0) return;
                if(key == (int)KeyboardKey.KEY_BACKSPACE){
                    if(textInput.Length>0)textInput.Remove(textInput.Length-1,1);
                }else if(char.IsDigit((char)key) && textInput.Length <5) textInput.Append((char)key);
            
        }
        
    }

    public void Draw(){
        int textSize = Raylib.MeasureText(text+":",fontSize);
        int inptSize = Raylib.MeasureText(textInput.ToString(),fontSize);
        int w = (int)rect.width;
        if(w < textSize + inptSize + 30) w = textSize + inptSize + 30;
        s = w;
        Raylib.DrawRectangle((int)rect.x,(int)rect.y,w,(int)rect.height,backgroundColor);
        int px =(int) (rect.x + 5);
        int py = (int)(rect.y + rect.height/2 - fontSize/2);
        Raylib.DrawText(text+":",px,py,fontSize,textColor);
        px += 15 + textSize;
        Raylib.DrawText(textInput.ToString(),px,py,fontSize,textColor);
    }
}
struct Button{
    Rectangle rect;
    string text;
    public Color backgroundColor = Color.GRAY;
    public Color textColor = Color.BLACK;
    public int fontSize = 16;
    public Button(float x, float y, float widht,float height,string text){
        rect = new(x,y,widht,height);
        this.text =text;
    }

     public Button(float x, float y, float widht,float height,string text,Color backgroundColor,Color textColor){
        rect = new(x,y,widht,height);
        this.text =text;
        this.backgroundColor = backgroundColor;
        this.textColor = textColor;
    }

    public bool Clicked(){
        if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)){
            Vector2 pos = Raylib.GetMousePosition();
            if( rect.x <= pos.X && pos.X <= rect.x + rect.width && rect.y <= pos.Y && pos.Y <= rect.y + rect.height ){
                WriteLine("Clicked on "+text);
                return true;
            }
        }
        return false;
    }

    public void Draw(){
        Raylib.DrawRectangle((int)rect.x,(int)rect.y,(int)rect.width,(int)rect.height,backgroundColor);
        float textSize = Raylib.MeasureText(text,fontSize);
        int px =(int) (rect.x + rect.width/2 - textSize/2);
        int py = (int)(rect.y + rect.height/2 - fontSize/2);
        Raylib.DrawText(text,px,py,fontSize,textColor);
    }
}