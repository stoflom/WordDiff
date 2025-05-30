﻿
using System.Text.RegularExpressions;
using System.Windows.Media; // Use System.Windows.Media for colors in WPF
using System.Windows.Controls; // Use System.Windows.Controls for WPF RichTextBox
using System.Windows.Documents;
using System.Windows;

public class WordDiff2
{
    private const int MatchMax = 5; // Maximum distance of words to search for a match
    private static readonly Regex WordSplitter = new Regex(@"\s+"); // Split on spaces

    private enum Operation
    {
        Same,
        Insert,
        Delete
    }

    public static void DiffFunction(string? originalText, string? modifiedText, RichTextBox richTextBox)
    {
        if (originalText != string.Empty && modifiedText != string.Empty)
        {
            if (richTextBox == null) throw new ArgumentNullException(nameof(richTextBox));

            List<string> originalList = new List<string>(WordSplitter.Split(originalText ?? string.Empty));
            List<string> modifiedList = new List<string>(WordSplitter.Split(modifiedText ?? string.Empty));

            richTextBox.Document.Blocks.Clear(); // Clear the RichTextBox before adding new text

            Operation lastOperation = Operation.Same;

            while (originalList.Count > 0 || modifiedList.Count > 0)
            {
                string? oCurrent = originalList.Count > 0 ? originalList[0] : null;
                string? mCurrent = modifiedList.Count > 0 ? modifiedList[0] : null;

                if (oCurrent == mCurrent)
                {
                    // If both words are the same, append them as normal text
                  
                    AppendText(richTextBox, oCurrent, Operation.Same, true);
                    originalList.RemoveAt(0);
                    modifiedList.RemoveAt(0);
                    lastOperation = Operation.Same;
                }
                else
                {
                    int matchIndex = FindMatchIndex(modifiedList, oCurrent);
                    if (matchIndex > -1)
                    {
                        InsertWords(richTextBox, modifiedList, matchIndex, true);
                        lastOperation = Operation.Insert;
                    }
                    else
                    {
                        matchIndex = FindMatchIndex(originalList, mCurrent);
                        if (matchIndex > -1)
                        {
                            DeleteWords(richTextBox, originalList, matchIndex, true);
                            lastOperation = Operation.Delete;
                        }
                        else
                        {
                            if (oCurrent != null)
                            {
                                AppendText(richTextBox, oCurrent, Operation.Delete, lastOperation == Operation.Delete);
                                originalList.RemoveAt(0);
                                lastOperation = Operation.Delete;
                            }
                            if (mCurrent != null)
                            {
                                AppendText(richTextBox, mCurrent, Operation.Insert, lastOperation == Operation.Insert);
                                modifiedList.RemoveAt(0);
                                lastOperation = Operation.Insert;
                            }
                        }
                    }
                    //Add a space
                    AppendText(richTextBox, " ", lastOperation, false);
                }
            }
        }
    }

    private static int FindMatchIndex(List<string> list, string? word)
    {
        for (int i = 1; i < MatchMax && i < list.Count; i++)
        {
            if (list[i] == word)
            {
                return i;
            }
        }
        return -1;
    }

    private static void InsertWords(RichTextBox richTextBox, List<string> list, int count, bool addSpace)
    {
        for (int i = 0; i < count; i++)
        {
            AppendText(richTextBox, list[0], Operation.Insert, addSpace);
            list.RemoveAt(0);
        }
    }

    private static void DeleteWords(RichTextBox richTextBox, List<string> list, int count, bool addSpace)
    {
        for (int i = 0; i < count; i++)
        {
            AppendText(richTextBox, list[0], Operation.Delete, addSpace);
            list.RemoveAt(0);
        }
    }

    private static void AppendText(RichTextBox richTextBox, string? text, Operation op, bool addSpace)
    {
        if (text != null)
        {
            var textRange = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd)
            {
                Text = text + (addSpace ? " " : "")
            };

            switch (op)
            {
                case Operation.Same:
                    textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
                    textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                    break;
                case Operation.Insert:
                    textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
                    textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                    break;
                case Operation.Delete:
                    textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                    textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                    break;
            }

        }
    }
}
