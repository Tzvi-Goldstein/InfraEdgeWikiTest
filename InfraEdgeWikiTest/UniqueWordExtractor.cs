using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class UniqueWordExtractor
{
    private string input;

    public UniqueWordExtractor(string input)
    {
        this.input = input;
    }

    public Dictionary<string, int> ExtractUniqueWordsWithOccurrences()
    {
        string cleanedInput = RemoveBracketedContent(input);
        cleanedInput = RemoveDelimiters(cleanedInput);
        return CountWordOccurrences(cleanedInput);
    }

    private string RemoveBracketedContent(string text)
    {
        return Regex.Replace(text, @"\[[^\]]*\]|\([^\)]*\)|\{[^\}]*\}|\<[^\>]*\>", "");
    }

    private string RemoveDelimiters(string text)
    {
        return Regex.Replace(text, @"[.,;:!?\-\t\n\r]", " ");
    }

    private Dictionary<string, int> CountWordOccurrences(string text)
    {
        string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, int> wordOccurrences = new Dictionary<string, int>();

        foreach (var word in words.Select(word => word.ToLower()))
        {
            if (wordOccurrences.ContainsKey(word))
            {
                wordOccurrences[word]++;
            }
            else
            {
                wordOccurrences[word] = 1;
            }
        }

        return wordOccurrences;
    }
}

