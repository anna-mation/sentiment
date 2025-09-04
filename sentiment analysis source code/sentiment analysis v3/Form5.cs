using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Porter2Stemmer;

namespace sentiment_analysis_v3
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            label2.Hide();
            this.Icon = Properties.Resources.icons8_emojis_64; //sets icon to custom icon
        }
        string[] chars = new string[] { ",", ".", "?", "!", "$", "%", "^", "&", "*", ";", "-", "_", "(", ")", ":", "|", "[", "]", "/", "+", "=", "{", "}", "[", "]", "`", "~", "<", ">", "@", "#" };
        string[] stopwords = new string[] { "i", "im",  "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "your", "yours", "yourself", "yourselves", "he", "him",
                                                "his", "himself", "she", "her", "hers", "herself", "it", "its", "itself", "they", "them", "their", "theirs", "themselves",
                                                "what", "which", "who", "whom", "this", "that", "these", "those", "am", "is", "are", "was", "were", "be", "been", "being",
                                                "have", "has", "had", "having", "do", "does", "did", "doing", "a", "an", "the", "and", "if", "or", "because", "as",
                                                "until", "while", "of", "at", "by", "for", "with", "about", "between", "into", "through", "during", "before",
                                                "after", "above", "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", "under", "again", "further", "then",
                                                "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each", "few", "more", "most", "other", "some",
                                                "such", "only", "own", "same", "so", "than", "too", "can", "will", "just", "should", "now"};
        string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", " ", "\n", "\r\n", "\r" };

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            mainmenu.StartPosition = FormStartPosition.Manual;
            mainmenu.Location = this.Location;
            this.Hide();
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) //to close down entire application if user presses 'X' in child form
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainline();
        }

        private void mainline()
        {
            string address = (TextBox1.Text).Replace("\"", ""); //address of file
            string splitby = "sentence";
            if (File.Exists(@address) == true && address.Contains(".txt")) //if address is valid
            {
                label2.Hide();
                FileStream fs = File.OpenRead(@address);
                var sr = new StreamReader(fs);
                string line = "";
                List<string> totallines = new List<string>();
                while (line != null)
                {
                    if (comboBox1.SelectedIndex == 0) //split by sentence
                    {
                        List<string> currentline = line.Split(new char[] { '!', '?', '.' }).ToList();
                        totallines.AddRange(currentline); //to combine both lists
                    }
                    else //split by line
                    {
                        totallines.Add(line);
                        splitby = "line";
                    }
                    line = sr.ReadLine();
                }
                totallines = totallines.Where(x => !string.IsNullOrWhiteSpace(x)).ToList(); //removes elements made up of spaces or empty
                string[] arraylines = totallines.ToArray();
                string[][] processedtotallines = new string[arraylines.Length][]; //each sentence is an array, within those arrays are the words in sentence
                int[][] totalstoppunctuation = new int[arraylines.Length][];
                for (int i = 0; i < arraylines.Length; i++)
                {
                    var temp = inputProcess(arraylines[i]);
                    processedtotallines[i] = temp.Item1;
                    totalstoppunctuation[i] = temp.Item2;
                }
                Form6 documentoutput = new Form6(processedtotallines, totalstoppunctuation, arraylines, splitby);
                this.Hide();
                documentoutput.Show();
                documentoutput.Refresh();
                documentoutput.StartPosition = FormStartPosition.Manual;
                documentoutput.Location = this.Location;
            }
            else
            {
                label2.Show();
            }
        }

        private Tuple<string[], int[]> inputProcess(string input) //to make words easier to work with
        {
            int count = 0;
            foreach (char letter in input.ToLower())
            {
                if (!(alphabet.Contains(letter.ToString()) || chars.Contains(letter.ToString()))) //deletes all non-letters or non-punctuation. effectively gets rid of emojis
                {
                    input = input.ToLower().Replace(letter, '0');
                }
            }
            string[] output = input.Replace("0", "")
                .Replace("\n", " ")
                .Replace("\r\n", " ")
                .Replace("\r", " ")
                .ToLower().Split(new char[] { ',', '$', '%', '^', '&', '*', ';', '-', '_', '(', ')', ':', '|', '[', ']', '/', '+', '=', '{', '}', '[', ']', '`', '~', '<', '>', ' ' });
            //splits input by spaces and punctuation
            var stemmer = new EnglishPorter2Stemmer();
            List<string> newOutput = new List<string>();
            List<int> punctuationarray = new List<int>();
            foreach (string word in output) //only putting words without @# and not stopwords into new array
            {
                if (word != "")
                {
                    string word1 = word; //to be able to modify word
                    if (word[word.Length - 1] == '.' || word[word.Length - 1] == '!' || word[word.Length - 1] == '?') //checking for stopping punctuation
                    {
                        punctuationarray.Add(newOutput.Count); //noting down the index of every word that ends with stopping punctuation
                        word1 = word.Replace(".", "").Replace("!", "").Replace("?", "");
                    }
                    if ((word1.Contains("#") || word1.Contains("@") || word1.Contains("http") || word1.Contains("com") || word1 == "" || checkStopwords(word1)) == false)
                    {
                        newOutput.Add(word1);
                    }
                }
            }
            string[] newoutputarray = newOutput.ToArray();
            foreach (string word in newoutputarray)
            {
                //stemming words to their root form
                var result = stemmer.Stem(word).Value;
                newoutputarray[count] = RepeatingChars(result); //removes any repeating characters
                count++;
            }
            return new Tuple<string[], int[]>(newoutputarray, punctuationarray.ToArray());
        }

        private bool checkStopwords(string word) //to check if a word is a stopword before lemmanising
        {
            foreach (string stopword in stopwords)
            {
                if (word == stopword)
                {
                    return true;
                }
            }
            return false;
        }

        private string RepeatingChars(string str) //deletes instances where there are three or more repeating consecutive characters in a word
        {
            int i = 0; //index 
            char[] chars = (str + "00").ToCharArray(); //converts string to character array, adds 2 empty characters at end to account for out of bound errors :)
            while (i < (chars.Length - 2))
            {
                if (chars[i] == chars[i + 1] && chars[i] == chars[i + 2]) //while 3 consecutive characters are the same
                {
                    int x = 0; //to move through indexes
                    char temp = chars[i + 2]; //temporary variable to hold character to compare next characters to
                    while (temp == chars[i + 2 + x] && (i + 2 + x) < (chars.Length - 2)) //compares all next characters with third repeated character, if the same then delete
                    {
                        chars[i + 2 + x] = '0'; //replaces duplicate characters  with empty space from the third repeated character onwards
                        x++;
                    }
                }
                i++;
            }
            str = String.Join("", chars).Replace("0", ""); //replaces empty character with empty string
            return str;
        }
    }
}
