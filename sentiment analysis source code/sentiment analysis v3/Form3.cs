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
    public partial class Form3 : Form
    {
        public Form3(Tuple<string[], int[]>returnedarrays)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_emojis_64; //sets icon to custom icon
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
            mainline(returnedarrays);
        }
        string[] wordcount;
        string[] wordcount1;
        string[] negations = new string[] { "zero", "doesnt", "isnt", "wasnt", "shouldnt", "wouldnt", "couldnt", "didnt", "wont", "cant", "dont", "no", "not", "noth", "neither", "nowher", "never" };
        string[] intensifiers = new string[] { "extrem", "realli", "super", "veri", "high" };


        private void mainline(Tuple<string[], int[]>returnedarrays)
        {
            string[] input = returnedarrays.Item1;
            int[] punctuation = returnedarrays.Item2; //positions of stopping punctuation within sentence
            int range = 3; //boundaries for sentiment score, used for confidence meter
            double probs = TotalProbability(input, punctuation);
            double percentage = 100;
            if (probs > 0)
            {
                sentimentlabel.Text = "Sentiment: positive (" + Math.Round(probs, 2) + ") :)";
                percentage = Math.Min(100, Math.Round(probs / range * 100, 2)); //rounds to 2 dp and also caps calue at 100
                confidencelabel.Text = "Confidence: " + percentage + "%";
            }
            if (probs < 0)
            {
                sentimentlabel.Text = "Sentiment: negative (" + Math.Round(probs, 2) + ") :(";
                percentage = Math.Min(100, Math.Round(probs / -range * 100, 2));
                confidencelabel.Text = "Confidence: " + percentage + "%";
            }
            if (probs == 0)
            {
                sentimentlabel.Text = "Sentiment: neutral (0.00) :]";
                confidencelabel.Text = "Confidence: 100%";
            }
            percentagebar(percentage);
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
            string[] wordratings = new string[sentence.Length];
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
                } else if (word == "but")
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
                wordratings[index] = word + ": " + boolean;
                index++;
                if (!negations.Contains(word) && !intensifiers.Contains(word) && word != "but") //only take probability if word is not negation/intensifier
                {
                    prob += wordprob;
                }
            }
            resultstextbox.Text = string.Join(Environment.NewLine, wordratings); //prints word list onto textbox, each on a new line
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void percentagebar(double percentage)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = Convert.ToInt32(Math.Round(percentage, 0));
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Minimum = 0; //to stop idle glow animation on bar
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) //to close down entire application if user presses 'X' in child form
                Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            mainmenu.StartPosition = FormStartPosition.Manual;
            mainmenu.Location = this.Location;
            this.Hide();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 statementinput = new Form2();
            statementinput.Show();
            statementinput.StartPosition = FormStartPosition.Manual;
            statementinput.Location = this.Location;
            this.Hide();
        }
    }
}
