using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WordDiff
{
    public static string WordDiffFunction(string originalText, string modifiedText)
    {
        // regex to split the text into words
        Regex regx = new Regex(@"\s+"); // For wordDiff split on spaces
        const string spce = " "; // For wordDiff spaces are inserted between words

        // html tags for deletions and insertions,
        // if you use another markup standard, you can change these tags
        const string delOpen = "<del>";
        const string insOpen = "<ins>";
        const string delClose = "</del>";
        const string insClose = "</ins>";
        List<string> originalList = new List<string>();
        List<string> modifiedList = new List<string>();
        bool inserting = false;
        bool deleting = false;
        string outputBuffer = "";
        string oCurrent = "";
        string mCurrent = "";
        string insBuffer = insOpen;
        string delBuffer = delOpen;
        int matchIndex = -1;
        const int matchMax = 5; // Maximum distance of words to search for a match


        foreach (string element in regx.Split(originalText))
        {
            originalList.Add(element);
        }

        foreach (string element in regx.Split(modifiedText))
        {
            modifiedList.Add(element);
        }

        do
        {
            oCurrent = originalList.Count > 0 ? originalList[0] : null;
            mCurrent = modifiedList.Count > 0 ? modifiedList[0] : null;

            if (oCurrent == mCurrent)
            {
                // if the word agrees finalize any inserting/deleting
                if (deleting)
                {
                    outputBuffer += delBuffer + spce + delClose;
                    deleting = false;
                    delBuffer = delOpen;
                }
                if (inserting)
                {
                    outputBuffer += insBuffer + spce + insClose;
                    inserting = false;
                    insBuffer = insOpen;
                }
                outputBuffer += oCurrent + spce; // insert matching word in output
                originalList.RemoveAt(0);
                modifiedList.RemoveAt(0);
            }
            else
            {
                // If it disagrees, check if any of the next matchMax words in B agrees
                matchIndex = -1;
                for (int i = 0; i < matchMax && i < modifiedList.Count; i++)
                {
                    if (modifiedList[i] == oCurrent)
                    {
                        matchIndex = i;
                        break;
                    }
                }
                if (matchIndex > -1)
                {
                    // Found---so insert words from B up to match
                    inserting = true;
                    for (int i = 0; i < matchIndex; i++)
                    {
                        insBuffer += modifiedList[0] + spce;
                        modifiedList.RemoveAt(0);
                    }
                }
                else
                {
                    // Check if any of the next matchMax words in A agrees
                    matchIndex = -1;
                    for (int i = 0; i < matchMax && i < originalList.Count; i++)
                    {
                        if (originalList[i] == mCurrent)
                        {
                            matchIndex = i;
                            break;
                        }
                    }
                    if (matchIndex > -1)
                    {
                        // Found---so delete words from A up to match
                        deleting = true;
                        for (int i = 0; i < matchIndex; i++)
                        {
                            delBuffer += originalList[0] + spce;
                            originalList.RemoveAt(0);
                        }
                    }
                    else
                    {
                        // No matches anywhere delete and add if defined
                        if (oCurrent != null)
                        {
                            deleting = true;
                            delBuffer += originalList[0] + spce;
                            originalList.RemoveAt(0);
                        }
                        if (mCurrent != null)
                        {
                            inserting = true;
                            insBuffer += modifiedList[0] + spce;
                            modifiedList.RemoveAt(0);
                        }
                    }
                }
            }
        } while (originalList.Count > 0 || modifiedList.Count > 0);

        // Do final closure
        if (deleting)
        {
            outputBuffer += delBuffer + spce + delClose;
        }
        if (inserting)
        {
            outputBuffer += insBuffer + spce + insClose;
        }
        return outputBuffer;
    }
}

