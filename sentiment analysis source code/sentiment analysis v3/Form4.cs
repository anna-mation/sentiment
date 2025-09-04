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
using System.Reflection;

namespace sentiment_analysis_v3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
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
        string[] negations = new string[] { "zero", "doesnt", "isnt", "wasnt", "shouldnt", "wouldnt", "couldnt", "didnt", "wont", "cant", "dont", "no", "not", "noth", "neither", "nowher", "never" };
        string[] intensifiers = new string[] { "extrem", "realli", "super", "veri", "high" };
        string[] wordcount;
        string[] wordcount1;
        private void Form4_Load(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //file 1
            var resourceName = "sentiment_analysis_v3.Resources.word_count.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                wordcount = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
            //file 2
            resourceName = "sentiment_analysis_v3.Resources.word_count1.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                wordcount1 = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
        }
        private void test() //to test accuracy of program on another database
        {
            BackgroundWorker worker = new BackgroundWorker(); //background worker setup
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync(); //run background worker
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
                    if (word[word.Length - 1] == '.' || word[word.Length - 1] == '!' || word[word.Length - 1] == '?' || word[word.Length - 1] == '\n') //checking for stopping punctuation
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
        private double TotalProbability(string[] sentence, int[] punctuation) //total probability of a (processed) sentence
        {
            double prob = 0;
            double[] singleprob = new double[2];
            bool negationstatus = false;
            int intensitystatus = 1;
            bool toneshift = false; //if word is after a tone shift (but)
            int index = 0;
            int index1 = 0; //to keep track of punctuation array
            foreach (string word in sentence)
            {
                string boolean = "negative";
                if (negations.Contains(word)) //if word is a negation/intensifier, disregard probability
                {
                    boolean = "negation";
                    if (negationstatus) //switch sentiments
                    {
                        negationstatus = false;
                    }
                    else
                    {
                        negationstatus = true;
                    }
                }
                else if (intensifiers.Contains(word))
                {
                    boolean = "intensifier";
                    intensitystatus++;
                }
                else if (word == "but")
                {
                    toneshift = true;
                    boolean = "tone shift";
                }
                if (punctuation.Length > index1) //to skip over if array is empty
                {
                    if (index == punctuation[index1] + 1) //to see if a stopping punctuation is in the last word - if so, turn off tone shift filter
                    {
                        if (word != "but")
                        {
                            toneshift = false;
                        }
                    }
                }
                if (boolean == "negative") //classifying sentiment
                {
                    singleprob = TotalProbabilityWord(word); //probability of single word
                    if (singleprob[1] > singleprob[0])
                    {
                        boolean = "positive";
                    }
                    if (singleprob[1] == singleprob[0])
                    {
                        boolean = "neutral";
                    }
                }
                double wordprob = Math.Log10(singleprob[1] / singleprob[0]); //probability of single word
                if (boolean.Contains("negative") || boolean.Contains("positive"))
                {
                    if (intensitystatus != 1) //if word before was a negation, double main sentiment of word
                    {
                        wordprob *= intensitystatus;
                    }
                    if (toneshift) //to intensify any phrases between "but" and any stopping punctuation
                    {
                        wordprob *= 4;
                        boolean = boolean + ", after tone shift";
                    }
                    if (negationstatus) //if word before was a negation, switch sentiment
                    {
                        wordprob = -wordprob;
                        if (wordprob < 0)
                        {
                            boolean += " to negative";
                        }
                        else
                        {
                            boolean += " to positive";
                        }
                    }
                    intensitystatus = 1;
                    negationstatus = false;
                }
                index++;
                if (!negations.Contains(word) && !intensifiers.Contains(word) && word != "but") //only take probability if word is not negation/intensifier
                {
                    prob += wordprob;
                }
            }
            return prob;
        }

        private double[] TotalProbabilityWord(string word) //calculates probability for single word
        {
            double negcount;
            double poscount;
            double totalnegcount = 6130476 + 321940;
            double totalposcount = 5829862 + 384689;
            int linecount = 1;
            string[] line = wordcount[0].Split(',');
            while (linecount < 216643 && line[0] != word) //to get the wordcount for specific word
            {
                linecount++;
                line = wordcount[linecount - 1].Split(',');
            }
            negcount = Convert.ToInt32(line[1]);
            poscount = Convert.ToInt32(line[2]);

            //for file 2
            linecount = 1;
            while (linecount < 40784 && line[0] != word) //to get the wordcount for specific word
            {
                linecount++;
                line = wordcount[linecount - 1].Split(',');
            }
            negcount += Convert.ToInt32(line[1]);
            poscount += Convert.ToInt32(line[2]);

            if (poscount == 0 || negcount == 0) //if word not in database, to avoid 0 or infinity errors
            {
                return new double[] { 0.5, 0.5 };
            }
            //calculate neg probability
            double negprob = negcount / totalnegcount; //P(word|negative)

            //calculate pos probability
            double posprob = poscount / totalposcount; //P(word|positive)
            return new double[] { negprob, posprob };
        }

        private void wordCount() //to count total number of words in positive and negative tweets
        {
            int negcount = 0;
            int poscount = 0;
            //file 1
            foreach (string word in wordcount)
            {
                string[] splitline = word.Split(',');
                negcount += Convert.ToInt32(splitline[1]);
                poscount += Convert.ToInt32(splitline[2]);
 
            }

            //for file 2
            foreach (string word1 in wordcount1)
            {
                string[] splitline = word1.Split(',');
                negcount += Convert.ToInt32(splitline[1]);
                poscount += Convert.ToInt32(splitline[2]);
            }
            richTextBox1.Text = "neg: " + negcount + Environment.NewLine + //6130476 for file 1, 321940 for file 2
                                "pos: " + poscount; //5829862, 384689
        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
            button1.Enabled = false; //to avoid stacking background worker processes
            button3.Enabled = false;
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) //to close down entire application if user presses 'X' in child form
            {
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wordCount();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "sentiment_analysis_v3.Resources.tweets_data.txt";
            string[] testdata;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                testdata = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
            resourceName = "sentiment_analysis_v3.Resources.tweets_label.txt";
            string[] sentimentdata;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                sentimentdata = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
            int false_neg = 0;
            int false_pos = 0;
            int true_neg = 0;
            int true_pos = 0;
            double linecount = 1;
            int actualsentiment;
            int sentiment = 69420;
            for (int i = 0; i < 500; i++)
            {
                if (sentimentdata[i] != "O") //ignore neutral tweets
                {
                    var returnedarrays = inputProcess(testdata[i]);
                    double probs = TotalProbability(returnedarrays.Item1, returnedarrays.Item2);
                    if (probs > 0)
                    {
                        sentiment = 1; //positive
                    }
                    if (probs < 0)
                    {
                        sentiment = 0; //negative
                    }
                    string filesentiment = sentimentdata[i];
                    if (filesentiment == "P")
                    {
                        actualsentiment = 1;
                    }
                    else
                    {
                        actualsentiment = 0;
                    }
                    if (actualsentiment == sentiment)
                    {
                        if (sentiment == 1)
                        {
                            true_pos++;
                        }
                        else
                        {
                            true_neg++;
                        }
                    }
                    else
                    {
                        if (sentiment == 1)
                        {
                            false_pos++;
                        }
                        else
                        {
                            false_neg++;
                        }
                    }
                }
                int progress = Convert.ToInt32(Math.Round(Convert.ToDouble(linecount + 1) / 500 * 100, 0));
                int[] updateinfo = new int[] { false_neg, false_pos, true_neg, true_pos };
                ((BackgroundWorker)sender).ReportProgress(progress, updateinfo);
                linecount++;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage + "% scanned"; //to update labels + progress bar
            progressBar1.Value = e.ProgressPercentage;
            int[] updateinfo = (int[])e.UserState;
            int false_neg = updateinfo[0];
            int false_pos = updateinfo[1];
            int true_neg = updateinfo[2];
            int true_pos = updateinfo[3];
            richTextBox1.Text = "True pos: " + true_pos + Environment.NewLine +
                             "True neg: " + true_neg + Environment.NewLine +
                             "Actual neg but calc pos: " + false_pos + Environment.NewLine +
                             "Actual pos but calc neg: " + false_neg;
            double accuracy = Math.Round(100 * (Convert.ToDouble(true_pos + true_neg) / Convert.ToDouble(true_pos + true_neg + false_pos + false_neg)), 2);
            label2.Text = "Accuracy: " + accuracy + "%";
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            mainmenu.StartPosition = FormStartPosition.Manual;
            mainmenu.Location = this.Location;
            this.Hide();
        }
    }
}
