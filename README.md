Function to detect insertions and deletions between two texts
and to markup the differences with html/other tags.

It is assumed a nearby same word is a match, and the in-between words are insertions or deletions.
This limits the algorithm to short texts with a small number of differences, where this assumption
is likely to be true.

It is intended for highlighting differences in short database entries, not for comparing long texts.
If the text contains multiple occurrences of the same word, the algorithm may not work as expected.

Original idea from: https://github.com/dj-nitehawk/DiffWords.git
This is a new implementation with a different algorithm.

The algorithm was implemented in TypeScript and converted to C# with help from Copilot. The TypeScript
version has been tested against a database with about 50000 entries where short texts were revised with
Google Gemini and works fine for this purpose.


For an example of the output, see example.html

NOTE: there are many other tools that can be used e.g.

>wdiff  -t A.txt B.txt

>git diff --word-diff A.txt B.txt     (Must not be in git repository with .gitignore which includes *.txt)

or for javascript see:
https://github.com/kpdecker/jsdiff

These are all typically based on the Longest Common Subsequence algorithm:
https://en.wikipedia.org/wiki/Longest_common_subsequence
https://en.wikipedia.org/wiki/Longest_common_substring

For implementations see e.g.
https://github.com/cubicdaiya/onp

See also: https://neil.fraser.name/writing/diff/ and https://github.com/google/diff-match-patch