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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_emojis_64; //sets icon to custom icon
        }

        private void Form2_Load(object sender, EventArgs e)
        {

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

        private void mainline()
        {
            string sentence = TextBox1.Text;
            var returnedarrays = inputProcess(sentence);
            Form3 statementoutput = new Form3(returnedarrays);
            statementoutput.StartPosition = FormStartPosition.Manual;
            statementoutput.Location = this.Location;
            this.Hide();
            statementoutput.ShowDialog();
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
                .Replace("\n", "\n ")
                .Replace("\r\n", "\n ")
                .Replace("\r", "\n ")
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
                    if (word[word.Length - 1] == '.' || word[word.Length - 1] == '!' || word[word.Length - 1] == '?' || word[word.Length-1] == '\n') //checking for stopping punctuation
                    {
                        punctuationarray.Add(newOutput.Count); //noting down the index of every word that ends with stopping punctuation
                        word1 = word.Replace(".", "").Replace("!", "").Replace("?", "").Replace("\n", "");
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

        private void openDatabase() //to open tweet database, and create a file with a count for every unique word. only needs to be run once
        {
            string wordcount = @"C:\Users\Anna Dong\Downloads\archive\word_count1.txt"; //txt file to write wordcount in
            var fileName = @"C:\Users\Anna Dong\Downloads\train.csv\train.csv"; //tweet database 2
            //var fileName = @"C:\Users\Anna Dong\Downloads\archive\training.1600000.processed.noemoticon.csv"; //tweet database 1
            float linecount = 1; //to keep track of how many lines program has processed
            int linecountstop = 100001;//1600000 for file 1, 100001 for file 2

            Dictionary<string, int[]> unique_words = new Dictionary<string, int[]>(); //dictionary to store unique word, count in negative tweets, count in positive tweets
            FileStream fs = File.OpenRead(fileName);
            var sr = new StreamReader(fs);
            string[] line = null;
            while (linecount <= linecountstop)
            {
                //line = sr.ReadLine().Split(new string[] { "\",\"" }, StringSplitOptions.None);
                line = sr.ReadLine().Replace("0,", "0>").Replace("1,", "1>").Replace(", ", " ").Split(new char[] { ',', '>' }); //formatting
                string[] processed_line = inputProcess(line[2]).Item1; //stems and processes tweet, [5] for 1, [2] for 2
                foreach (string word in processed_line)
                {
                    string word1 = word;
                    word1 = word.Replace("\"", ""); //removing ""
                    if (unique_words.ContainsKey(word1) == false) //if word is already in dictionary
                    {
                        if (line[1].Contains("0")) //negative tweet, line[0] for file 1, [1] for 2
                        {
                            int[] temp_neg = { 1, 0 };
                            unique_words.Add(word1, temp_neg);
                        }
                        else //positive tweet
                        {
                            int[] temp_pos = { 0, 1 };
                            unique_words.Add(word1, temp_pos);
                        }
                    }
                    else
                    {
                        if (line[1].Contains("0")) //neg
                        {
                            unique_words[word1][0]++; //add 1 to wordcount
                        }
                        else //pos
                        {
                            unique_words[word1][1]++;
                        }
                    }
                }
                linecount++;
                Console.WriteLine((linecount / linecountstop) * 100); //percentage of database processed
            }
            Console.ReadLine();
            fs = new FileStream(wordcount, FileMode.Open, FileAccess.Write); //to write into word count file
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (string key in unique_words.Keys)
                {
                    sw.WriteLine(key + "," + string.Join(",", unique_words[key])); //writes word, neg count, pos count onto one line
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainline();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) //to close down entire application if user presses 'X' in child form
                Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e) //back button
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            mainmenu.StartPosition = FormStartPosition.Manual;
            mainmenu.Location = this.Location;
            this.Hide();
        }
    }
}
