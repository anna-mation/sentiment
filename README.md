# sentiment
A sentiment analysis tool in C#, trained on Twitter data. Can assess the sentiment (positive or negative tone) of any input statement or file.

It can:
1. Take in a statement, and clean it into its root form
2. Use existing datasets to create a dictionary/weighting system of positive and negative words
3. Use the data to analyse the input statement word by word, and calculate a final rating
4. Output a positive or negative rating for the input statement

From my limited investigation after the project, I concluded my program has an 81% accuracy rate, with a higher likelihood of classifying positive tweets correctly than negative. Furthermore, it can recognise different types of words, including negations, intensifiers and “but”, and calculate the sentiment accordingly. Its processing time for medium sized documents (such as movie scripts), is mostly less than 30 seconds.

This was written in my final year of high school, in 2022.

## How to run

Run the executable `sentiment analysis application/sentiment analysis v3.exe`
