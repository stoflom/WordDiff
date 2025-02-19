Function to detect insertions and deletions between two texts
and to markup the differences with html/other tags.

It is assumed a nearby (default within 5 words) same word is a match, and the in-between words are insertions or deletions.
This limits the algorithm to short texts with a small number of differences, where this assumption
is likely to be true.

It is intended for highlighting differences in short database entries, not for comparing long texts.

Original idea from: https://github.com/dj-nitehawk/DiffWords.git
This is a new implementation with a different algorithm.

The algorithm was implemented in TypeScript and converted to C# with help from Copilot. The TypeScript
version has been tested against a database with about 50000 entries to indicate where short texts were
revised with Google Gemini and works fine for this purpose.

For an example of the output, see example.html

NOTE: there are many other tools that can be used e.g.

>wdiff  -t A.txt B.txt		(Based on diff)

>git diff --word-diff A.txt B.txt     (Must not be in git repository with .gitignore which includes *.txt)

or for javascript see:
https://github.com/kpdecker/jsdiff

These are all typically based on the Longest Common Subsequence  (LCS) algorithm:
https://en.wikipedia.org/wiki/Longest_common_subsequence
https://en.wikipedia.org/wiki/Longest_common_substring

For implementations see e.g.
https://github.com/cubicdaiya/onp

See also: https://neil.fraser.name/writing/diff/ and https://github.com/google/diff-match-patch

These tools find the least amount of edits in order to generate patches for software updates.

The LCS algorithm gives identical results to the simple algorithm implemented here when the changes
are not too extensive. When the changes are extensive the simple algorithm tends to highlight whole blocks
of text as replaced, which---subjectively---is often easier for me to comprehend.

I have also written an Excel VBA macro which can be used to display the marked-up text in color:
https://github.com/stoflom/ExcelHighliteDiffs

A screenshot is here: https://github.com/stoflom/ExcelHighliteDiffs/blob/main/Screenshot%202025-02-04%20163751.png