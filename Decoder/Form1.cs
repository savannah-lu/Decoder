using System.Text;
using System.Windows.Forms;

namespace Decoder
{
    public partial class Form1 : Form
    {
        Bitmap b;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //create picture, clear any text user added or text from prev. secret message
                    b = CreateBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Image = b;
                    richTextBox1.Clear();
                    richTextBox2.Clear();
                    

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //check password then show the message
        private void button2_Click(object sender, EventArgs e) 
        {
            if (b == null)
            {
                MessageBox.Show("Please select a PPM file first.");
            }
            else
            {
                try
                {
                    //check password
                    bool isPassword = DecodePassword(richTextBox2.Text, b);

                    //if password is correct, decode message
                    if (isPassword)
                    {
                        string secretMsg = Decode(b);

                        richTextBox1.Text = secretMsg;
                    }
                    //reenter password
                    else
                    {
                        MessageBox.Show("Wrong password, please try again (or don't).");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public Bitmap CreateBitmap()
        {
            //read stream, declare variables
            StreamReader reader = new StreamReader(openFileDialog1.FileName);
            string ppm = reader.ReadToEnd();
            string str = "";
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool endOfLine = true;
            int counter = 0;

    

            //determining what length my array holding the file info should be
            while (i < ppm.Length)
            {
                if (ppm[i] == '\n')
                {
                    counter++;
                }

                i++;
            }

            string[] ppmList = new string[counter];  //create array and variables
            i = 0;
            counter = 0;

            //storing ppm data into an array
            while (i < ppm.Length)
            {
                if (ppm[i] != '\n')
                {
                    str += ppm[i];
                }
                else
                {
                    ppmList[counter] = str + "\n";
                    str = "";
                    counter++;
                }

                i++;

            }

            str = ppmList[2].Trim();

            int width = 0;
            int height = 0;
            counter = 0;
            string s = "";
            Color c = new Color();
            int r = 0;
            int bl = 0;
            int g = 0;

            //getting width for bitmap
            for (i = 0; i < str.Length; i++)
            {
                if (Char.IsDigit(str[i]))
                {
                    s += str[i];
                }
                else if (str[i] == ' ')
                {
                    break;
                }

            }

            width = int.Parse(s);
            int num = s.Length;
            s = "";

            //getting height for bitmap
            for (i = num; i < str.Length; i++)
            {
                if (Char.IsDigit(str[i]))
                {
                    s += str[i];
                }
                else if (str[i] == ' ' && s != "")
                {
                    break;
                }
            }

            height = int.Parse(s);
            counter = 4;
            int wTracker = 0;
            int hTracker = 0;
            int x = 0;
            int y = 0;
            Bitmap b = new Bitmap(width, height); //create bitmap for ppm file


            
            while (endOfLine)
            {
                //if we haven't reached the end of the row or list
                if (wTracker < (width * 3) && counter < ppmList.Length)
                {
                    //get RGB values from file
                    r = int.Parse(ppmList[counter]);
                    g = int.Parse(ppmList[counter + 1]);
                    bl = int.Parse(ppmList[counter + 2]);

                    //create color and set pixel
                    c = Color.FromArgb(r, g, bl);

                    b.SetPixel(x, y, c);

                    counter += 3;
                    wTracker += 3;
                    x++;

                }
                else //we reached the end of the row
                {
                    if (hTracker < height)
                    {
                        wTracker = 0; //reset width to beginning of row
                        hTracker++;
                        x = 0;
                        y++;
                    }
                    else //we reached the end of pic
                    {
                        endOfLine = false;
                    }

                }
            }

            return b;
        }

        public string Decode(Bitmap b)
        {
            bool flag = true;
            int x = 0;
            int y = 2;
            int ch = 0;
            string message = "";

            //runs until message is completely decoded
            while(flag)
            {
                //if Green value falls within range
                if(b.GetPixel(x, y).G <= 122 && b.GetPixel(x, y).G >= 97 || b.GetPixel(x, y).G == 32 || b.GetPixel(x, y).G == 33 || b.GetPixel(x, y).G == 46 || b.GetPixel(x, y).G == 63)
                {
                    //get Green value and convert to char
                    ch = b.GetPixel(x, y).G;
                    message += GetChar(ch);
                    x++;
                    
                }
                //end of message
                else
                {
                    flag = false;
                }
            }

            return message;
        }

        //converts Green value to char
        public string GetChar(int num)
        {
            string str = "";

            /*  97-122
             *  A - Z */

            switch (num)
            {
                case 48: { str = "0"; break; }
                case 49: { str = "1"; break; }
                case 50: { str = "2"; break; }
                case 51: { str = "3"; break; }
                case 52: { str = "4"; break; }
                case 53: { str = "5"; break; }
                case 54: { str = "6"; break; }
                case 55: { str = "7"; break; }
                case 56: { str = "8"; break; }
                case 57: { str = "9"; break; }
                case 97: { str = "a"; break; }
                case 98: { str = "b"; break; }
                case 99: { str = "c"; break; }
                case 100: { str = "d"; break; }
                case 101: { str = "e"; break; }
                case 102: { str = "f"; break; }
                case 103: { str = "g"; break; }
                case 104: { str = "h"; break; }
                case 105: { str = "i"; break; }
                case 106: { str = "j"; break; }
                case 107: { str = "k"; break; }
                case 108: { str = "l"; break; }
                case 109: { str = "m"; break; }
                case 110: { str = "n"; break; }
                case 111: { str = "o"; break; }
                case 112: { str = "p"; break; }
                case 113: { str = "q"; break; }
                case 114: { str = "r"; break; }
                case 115: { str = "s"; break; }
                case 116: { str = "t"; break; }
                case 117: { str = "u"; break; }
                case 118: { str = "v"; break; }
                case 119: { str = "w"; break; }
                case 120: { str = "x"; break; }
                case 121: { str = "y"; break; }
                case 122: { str = "z"; break; }
                case 32:  { str = " "; break; }
                case 33:  { str = "!"; break; }
                case 63:  { str = "?"; break; }
                case 46:  { str = "."; break; }

                default: { str = "null"; break; }
            }



            return str;
        }

        public bool DecodePassword(string str, Bitmap b)
        {
            string password = "";
            bool isPassword = false;
            bool flag = true;
            int x = 0;
            int y = b.Height - 1;  //start decoding on next-to-last row
            int ch = 0;

            while (flag)
            {
                //checks for numbers
                if (b.GetPixel(x, y).G <= 57 && b.GetPixel(x, y).G >= 48) 
                {
                    ch = b.GetPixel(x, y).G;
                    password += GetChar(ch);
                    x++;
                }
                else //we reached the end of the password
                { 
                    flag = false;
                }
            }

            //if password entered is correct
            if(str == password)
            {
                isPassword = true;
            }

            return isPassword;
        }

       
    }
}