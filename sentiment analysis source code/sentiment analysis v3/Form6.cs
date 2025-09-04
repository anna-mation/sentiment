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
using System.Diagnostics;
using System.Reflection;

namespace sentiment_analysis_v3
{
    public partial class Form6 : Form
    {
        public Form6(string[][] lines, int[][] stoppunct, string[] originallines, string splitby)
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
            mainline(lines, stoppunct, splitby, originallines);
        }
        string[] negations = new string[] { "zero", "doesnt", "isnt", "wasnt", "shouldnt", "wouldnt", "couldnt", "didnt", "wont", "cant", "dont", "no", "not", "noth", "neither", "nowher", "never" };
        string[] intensifiers = new string[] { "extrem", "realli", "super", "veri", "high" };
        string outputfileName = @"C:\Temp\fileoutput.txt"; //output file
        string[] wordcount;
        string[] wordcount1;

        private void mainline(string[][] lines, int[][] stoppunct, string splitby, string[] originallines) //lines split into words, lines split into indexes of stopping punctuation, splitting by sentences or lines
        {
            BackgroundWorker worker = new BackgroundWorker(); //setup background worker
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            dynamic[] workerparam = new dynamic[] { lines, stoppunct, splitby, originallines}; //carry parameters over
            worker.RunWorkerAsync(workerparam); //run background worker
        }

        private Tuple<double, string[]>TotalProbability(string[] sentence, int[] punctuation) //total probability of a (processed) sentence
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
                wordratings[index] = word + ": " + boolean;
                index++;
                if (!negations.Contains(word) && !intensifiers.Contains(word) && word != "but") //only take probability if word is not negation/intensifier
                {
                    prob += wordprob;
                }
            }
            return new Tuple<double, string[]>(prob, wordratings);
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

        private void filecreate(string[] originallines, string[][] wordratings, double[] sentenceratings)
        {
            int index = 0;
            string rating;
            if (File.Exists(outputfileName)) // Check if file already exists. If yes, delete it
            {
                File.Delete(outputfileName);
            }    
            using (StreamWriter sw = File.CreateText(outputfileName)) // Create a new file 
            {
                foreach (string line in originallines)
                {
                    sw.WriteLine(line); //original line before processing
                    double sentiment = sentenceratings[index]; //raw rating of sentence
                    if (sentiment > 0)
                    {
                        rating = "positive";
                    } else if (sentiment < 0)
                    {
                        rating = "negative";
                    }
                    else
                    {
                        rating = "neutral";
                    }
                    sw.WriteLine("Sentiment: " + rating + " (" + Math.Round(sentiment, 2) + ")");
                    sw.WriteLine("-----------------------------------------------");
                    foreach (string word in wordratings[index]) //word and repsective word sentiment + any modifiers
                    {
                        sw.WriteLine(word);
                    }
                    sw.WriteLine("-----------------------------------------------");
                    sw.WriteLine(""); //formatting
                    sw.WriteLine("");
                    index++;
                }
            }
        }

        private void percentagebar(double percentage)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = Convert.ToInt32(Math.Round(percentage, 0));
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Minimum = 0; //to stop idle glow animation on bar
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 documentinput = new Form5();
            documentinput.Show();
            documentinput.StartPosition = FormStartPosition.Manual;
            documentinput.Location = this.Location;
            this.Hide();
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) //to close down entire application if user presses 'X' in child form
                Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 mainmenu = new Form1();
            mainmenu.Show();
            mainmenu.StartPosition = FormStartPosition.Manual;
            mainmenu.Location = this.Location;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", outputfileName);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dynamic[] parameters = (dynamic[])e.Argument; //parameters from mainline
            string[][] lines = parameters[0]; //lines split into words
            int[][] stoppunct = parameters[1]; //locations of punctuation
            string splitby = parameters[2]; //sentences or lines
            string[] originallines = parameters[3];
            int lineindex = 0; //line index, to keep track of current line being processed
            double[] probs = new double[lines.Length]; //probabilities of sentences
            string[][] words = new string[lines.Length][]; //lines of words and their individual ratings
            double poscount = 0; //no of positive lines
            double negcount = 0; //no of negative lines
            double ncount = 0; //no of neutral lines
            double percentage; //confidence
            int range = lines.Length/10; //boundaries for sentiment score, used for confidence meter
            foreach (string[] line in lines)
            {
                string[] input = lines[lineindex];
                int[] punctuation = stoppunct[lineindex]; //positions of stopping punctuation within sentence
                var funcreturn = TotalProbability(input, punctuation);
                probs[lineindex] = funcreturn.Item1;
                words[lineindex] = funcreturn.Item2;
                double probability = probs[lineindex];
                if (probability > 0)
                {
                    poscount++;
                }
                if (probability < 0)
                {
                    negcount++;
                }
                if (probability == 0)
                {
                    ncount++;
                }
                int progress = Convert.ToInt32(Math.Round(Convert.ToDouble(lineindex + 1)/Convert.ToDouble(lines.Length) * 100, 0)); //convert linecount to a percentage
                ((BackgroundWorker)sender).ReportProgress(progress); //update ui
                lineindex++;
            }
            double totalprob = 0;
            foreach (double score in probs) //to add up sentiment scores of sentence to form document sentiment
            {
                totalprob += score;
            }
            if (totalprob < 0)
            {
                range = -range;
            }
            double aaa = (totalprob / (lineindex + 1)) / range;
            percentage = Math.Min(100, Math.Round((totalprob) / range * 100, 2)); //rounds to 2 dp and also caps calue at 100
            filecreate(originallines, words, probs);
            dynamic[] workeroutput = {percentage, totalprob, poscount, negcount, ncount, splitby};
            e.Result = workeroutput;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage.ToString() + "% scanned"; //update label and progress bar
            progressBar2.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) //to update gui once worker finished
        {
            string sentiment;
            dynamic[] workeroutput = (e.Result as dynamic[]); //retrieve background worker output
            double percentage = workeroutput[0];
            double totalprob = workeroutput[1];
            double poscount = workeroutput[2];
            double negcount = workeroutput[3];
            double ncount = workeroutput[4];
            string splitby = workeroutput[5];
            if (double.IsNaN(percentage))
            {
                percentage = 100;
            }
            percentagebar(percentage);
            double pospercent = Math.Round(poscount / (poscount + negcount + ncount) * 100, 2);
            if (double.IsNaN(pospercent))
            {
                pospercent = 0;
            }
            confidencelabel.Text = "Confidence: " + percentage + "%";
            if (totalprob > 0)
            {
                sentiment = "positive";
            }
            else if (totalprob == 0)
            {
                sentiment = "neutral";
            }
            else
            {
                sentiment = "negative";
            }
            resultstextbox.Text = "The overall sentiment of this document is " + sentiment + ", with a raw score of " + Math.Round(totalprob, 2) + Environment.NewLine
                + pospercent + "% of " + splitby + "s are positive." + Environment.NewLine
                + "There are " + poscount + " positive " + splitby + "s, " + ncount + " neutral " + splitby + "s, and " + negcount + " negative " + splitby + "s.";
            sentimentlabel.Text = "Sentiment: " + sentiment;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            
        }
    }
}
