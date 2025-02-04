//Function to detect insertions and deletions between two texts
//and to markup the differences with html/other tags.

//Original idea from: https://github.com/dj-nitehawk/DiffWords.git
//This is a new implementation with a different algorithm.

//It is assumed a nearby same word is a match, and the rest are insertions or deletions.
//This limits the algorithm to short texts with a small number of differences,
//where this assumption is likely to be true.
//It is intended for highlighting short diffences in database entries, not for comparing long texts.
//If the text contains multiple occurrences of the same word, the algorithm may not work as expected.

//C Lombard 4 Feb 2025

function wordDiff(originalText: string, modifiedText: string): string {

    // regex to split the text into words
    const regx = /\s+/;       //For wordDiff split on spaces
    const spce = ' ';         //For wordDiff spaces are inserted between words

    // html tags for deletions and insertions,
    // if you use another markup standard, you can change these tags
    const delOpen = "<del>";
    const insOpen = "<ins>";
    const delClose = "</del>";
    const insClose = "</ins>";

    const originalBuffer = originalText.split(regx);
    const modifiedBuffer = modifiedText.split(regx);

    let inserting = false;
    let deleting = false;
    let outputBuffer = '';
    let oCurrent: string = '';
    let mCurrent: string = '';
    let insBuffer: string = insOpen;
    let delBuffer: string = delOpen;
    let matchIndex: number = -1;
    let matchMax: number = 5;  //Maximum distance of words to search for a match

    do {//until both arrays are empty
        oCurrent = originalBuffer[0];
        mCurrent = modifiedBuffer[0];

        if (oCurrent === mCurrent) { //if the word agrees finalize any inserting/deleting
            if (deleting) {
                outputBuffer += delBuffer + spce + delClose;
                deleting = false;
                delBuffer = delOpen;
            }
            if (inserting) {
                outputBuffer += insBuffer + spce + insClose;
                inserting = false;
                insBuffer = insOpen;
            }
            outputBuffer += oCurrent + spce; //insert matching word in output
            originalBuffer.shift();
            modifiedBuffer.shift();
        } else {    //If it disagrees, 
            // check if any of the next matchMax words in B agrees, 
            matchIndex = -1;
            for (let i = 0; i < matchMax; i++) {
                if (modifiedBuffer[i] === oCurrent) {
                    matchIndex = i;
                    break;
                }
            }
            if (matchIndex > -1) {
                //Found---so insert words from B up to match
                inserting = true;
                for (let i = 0; i < matchIndex; i++) {
                    insBuffer += modifiedBuffer.shift() + spce;
                }
            }
            else { //Check if any of the next matchMax words in A agrees
                matchIndex = -1;
                for (let i = 0; i < matchMax; i++) {
                    if (originalBuffer[i] === mCurrent) {
                        matchIndex = i;
                        break;
                    }
                }
                if (matchIndex > -1 ) {
                    //Found---so delete words from A up to match
                    deleting = true;
                    for (let i = 0; i < matchIndex; i++) {
                        delBuffer += originalBuffer.shift() + spce;
                    }
                }
                else { //No matches anywhere delete and add if defined
                    if (oCurrent) {
                        deleting = true;
                        delBuffer += originalBuffer.shift() + spce;
                    }
                    if (mCurrent) {
                        inserting = true;
                        insBuffer += modifiedBuffer.shift() + spce;
                    }
                }
            }
        }
    } while (originalBuffer[0] || modifiedBuffer[0]);
    //Do final closure
    if (deleting) {
        outputBuffer += delBuffer + spce + delClose;
    }
    if (inserting) {
        outputBuffer += insBuffer + spce + insClose;
    }
    return outputBuffer;
}

// Example usage

// const text1 = "min1 zero one too tree for fyve sevn eight nine ten fourteen fifteen";
// const text2 = "zero one two three four five six seven eight nine ten eleven twelve thirteen fourteen fifteen sixteen";

const text1 = `Plants acaulescent, with leaves in a basal rosette about 8 cm diam., proliferous from the base and forming small clusters. Leaves about 50, the young erect, incurved at the tips, the old ascending, somewhat spreading, with tips incurved, about 27 mm long, 9 mm broad, up to 5 mm thick, elongate or ovate-elongate, blue-green; upper surface flat to rounded, semi-pellucid, with 5 to 7 indistinct reticulate lines, smooth; lower surface convex, smooth, not pellucid; keel 1, often 2, centrally positioned in the upper half, the second one when present usually towards the margin, dentate; margins acute with a whitish edge visible from about the middle to the base, smooth, or with 0.5 mm long pellucid teeth; end-awn about 7 mm long, white, smooth. Inflorescence about 15 cm tall; peduncle simple, terete, about 2 mm diam., 8 cm long, bracteate; sterile bracts membranous, ovate, about 4 mm long, erect, keeled; raceme about 7 cm long, lax, with about 20 spirally arranged flowers and buds, 3 to 4 open simultaneously; floral bracts membranous, 5 mm long, deltoid, acute, keeled with reddish-brown veins, clasping the pedicels, (longer than the pedicels); pedicels 2 mm long, 1 mm diam., green, erect; perianth with brownish-green keels to the segments, 14 mm long, curved, compressed at base, funnel-shaped, segments free to the base, limb 2 lipped; posterior segments little recurved, pinkish-white, spreading, with brown veins; exterior part strongly recurved, spreading, pinkish-white with light brown veins; stamens 6 of two lengths, 8 and 9 mm long, inserted within the perianth tube; ovary 4 mm long, 2 mm diam., green; style 1 mm long, straight, white, capitate.`

const text2 = `Plants acaulescent, with leaves in a basal rosette about 8 cm diam., proliferous from the base, forming small clusters.  Leaves about 50, young erect, incurved at tips, old ascending, somewhat spreading, with incurved tips, 27 mm long, 9 mm broad, up to 5 mm thick, elongate or ovate-elongate, blue-green; upper surface flat to rounded, semi-pellucid, with 5–7 indistinct reticulate lines, smooth; lower surface convex, smooth, not pellucid; keel 1, often 2, centrally positioned in upper half, second one when present usually towards margin, dentate; margins acute with a whitish edge visible from about the middle to base, smooth, or with 0.5-mm-long pellucid teeth; end-awn about 7 mm long, white, smooth. Inflorescence about 15 cm tall; peduncle simple, terete, about 2 mm diam., 8 cm long, bracteate; sterile bracts membranous, ovate, about 4 mm long, erect, keeled; raceme about 7 cm long, lax, with about 20 spirally arranged flowers and buds, 3–4 open simultaneously; floral bracts membranous, 5 mm long, deltoid, acute, keeled with reddish-brown veins, clasping pedicels (longer than pedicels); pedicels 2 mm long, 1 mm diam., green, erect; perianth with brownish-green keels to segments, 14 mm long, curved, compressed at base, funnel-shaped, segments free to base, limb 2-lipped; posterior segments little recurved, pinkish-white, spreading, with brown veins; exterior part strongly recurved, spreading, pinkish-white with light brown veins; stamens 6, of two lengths, 8 and 9 mm long, inserted within perianth tube; ovary 4 mm long, 2 mm diam., green; style 1 mm long, straight, white, capitate.`

const result = wordDiff(text1, text2);

const html =
    `<!DOCTYPE html>
 <head>
 <style>
ins {color: blue;}
del {color: red;}
</style>
 </head>
  <body>
    <b>Text1:</b> &nbsp ${text1}<br><br>
    <b>Text2:</b> &nbsp ${text2}<br><br>
    <b>RESULT:</b> &nbsp ${result}<br>
  </body>
</html>` ;


console.log(html); //See example.html for output
