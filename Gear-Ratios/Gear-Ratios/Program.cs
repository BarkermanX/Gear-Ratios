﻿// See https://aka.ms/new-console-template for more information
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("Hello, World!");


//467..114..
//...*......
//..35..633.
//......#...
//617 * ....
//.....+.58.
//..592.....
//......755.
//...$.*....
//.664.598..

// Specify the path to the text file

int iGearSum = 0;

string filePath = "TestData.txt";
List<string> lstAllLines = new List<string>(); ;
try
{
    using (StreamReader reader = new StreamReader(filePath))
    {
        string line;

        // Read lines one at a time until the end of the file
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line); // Process each line as needed
            lstAllLines.Add(line);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

string line1 = lstAllLines[0];
string line2 = lstAllLines[1] ;
List<char> lstDistinctSymbols = new List<char>();

Dictionary<int, LineInfo> dctLineData = new Dictionary<int, LineInfo>();

int iLineNumber = 0;
// Work along each line and find the numbers
foreach (string strRow in lstAllLines)
{
    List<(string substring, int position)> lstResult = Helper.GetSpecificDigitInfo(strRow);

    List<char> lstChar = Helper.FindNonDotOrNumberChars(strRow);

    foreach(char c in lstChar)
    {
        if (!lstDistinctSymbols.Contains(c))
        {
            lstDistinctSymbols.Add(c);
        }
    }

    LineInfo objiLineNumber = new(strRow, lstResult);

    dctLineData.Add(iLineNumber, objiLineNumber);
    iLineNumber++;
}

for(iLineNumber = 0; iLineNumber < dctLineData.Count; iLineNumber++)
{
    string strLineData = ((LineInfo)dctLineData[iLineNumber]).LineData;

    string strLineAbove = string.Empty;
    string strLineBelow = string.Empty;

    if (iLineNumber != 0)
    {
        // populate for lines except for the first
        strLineAbove = ((LineInfo)dctLineData[iLineNumber - 1]).LineData;
    }


    if (iLineNumber != dctLineData.Count - 1)
    {
        // populate for lines except for the last
        strLineBelow = ((LineInfo)dctLineData[iLineNumber + 1]).LineData;
    }


    List<(string substring, int position)> lstItems = ((LineInfo)dctLineData[iLineNumber]).NumberPositionList;

    for (int iItem = 0; iItem < lstItems.Count; iItem++)
    {
        string strNumberString = lstItems[iItem].substring;
        int iStringPosition = lstItems[iItem].position;

        bool bResult = false;

        // Check left and right
        bResult = Helper.checkLeft(strLineData, iStringPosition, lstDistinctSymbols);

        if(!bResult)
            bResult = Helper.checkRight(strLineData, strNumberString, iStringPosition, lstDistinctSymbols);

        // Above and below first letter
        if (!bResult)
            bResult = Helper.CheckAboveFirstLetter(strLineAbove, iStringPosition, lstDistinctSymbols);

        if (!bResult)
            bResult = Helper.CheckBelowFirstLetter(strLineBelow, iStringPosition, lstDistinctSymbols);

        // Above and below last letter
        if (!bResult)
            bResult = Helper.CheckAboveLastNumber(strLineAbove, strNumberString, iStringPosition, lstDistinctSymbols);

        if (!bResult)
            bResult = Helper.CheckBelowLastNumber(strLineBelow, strNumberString, iStringPosition, lstDistinctSymbols);

        // Diagnol from first letter
        if (!bResult)
            bResult = Helper.checkDiagnolAboveFirst(strLineAbove, iStringPosition, lstDistinctSymbols);

        if (!bResult)
            bResult = Helper.checkDiagnolBelowFirst(strLineBelow, iStringPosition, lstDistinctSymbols);


        // Diagnol from last letter
        if (!bResult)
            bResult = Helper.CheckDiagnolAboveLast(strLineAbove, strNumberString, iStringPosition, lstDistinctSymbols);

        if (!bResult)
            bResult = Helper.CheckDiagnolBelowLast(strLineBelow, strNumberString, iStringPosition, lstDistinctSymbols);

        if (bResult)
        {
            // Valid Number
            int iValidNumber = int.Parse(strNumberString);

            iGearSum = iGearSum + iValidNumber;

        }

    }

}

Console.WriteLine(iGearSum);

string strEND = "END";



public static class Helper
{
    public static bool symbolBelow()
    {
        
        return false;
    }

    public static bool checkLeft(string strLine, int iPosition, List<char> lstDistinctSymbols)
    {
        // check character before the string starts
        int iPositionLeft = iPosition - 1;

        if(iPositionLeft > strLine.Length - 1 || iPositionLeft < 0)
        {
            return false;
        }

        char cCharacterToCheck = strLine[iPositionLeft];

        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool checkRight(string strLine, string strNumberString, int iPosition, List<char> lstDistinctSymbols)
    {
        // check character before the string starts
        int iPositionRight = iPosition + strNumberString.Length;

        if (iPositionRight > strLine.Length - 1)
        {
            return false;
        }

        char cCharacterToCheck = strLine[iPositionRight];


        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool CheckAboveFirstLetter(string strLineAbove, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineAbove))
        {
            return false;
        }

        if (iPosition > strLineAbove.Length - 1)
        {
            return false;
        }

        char cCharacterToCheck = strLineAbove[iPosition];

        bool bValid = lstDistinctSymbols.Contains(cCharacterToCheck);

        if (!bValid)
        {
            cCharacterToCheck = strLineAbove[iPosition + 1];

            bValid = lstDistinctSymbols.Contains(cCharacterToCheck);
        }

        return bValid;

    }

    public static bool CheckBelowFirstLetter(string strLineBelow, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineBelow))
        {
            return false;
        }

        if (iPosition > strLineBelow.Length - 1)
        {
            return false;
        }

        char cCharacterToCheck = strLineBelow[iPosition];

        bool bValid = lstDistinctSymbols.Contains(cCharacterToCheck);

        if (!bValid)
        {
            cCharacterToCheck = strLineBelow[iPosition + 1];

            bValid = lstDistinctSymbols.Contains(cCharacterToCheck);
        }

        return bValid;
    }

    public static bool CheckAboveLastNumber(string strLineAbove, string strNumberString, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineAbove))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition + strNumberString.Length - 1;

        if (iPositionToTest > strLineAbove.Length - 1)
        {
            return false;
        }
        char cCharacterToCheck = strLineAbove[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool CheckBelowLastNumber(string strLineBelow, string strNumberString, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineBelow))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition + strNumberString.Length - 1;

        if (iPositionToTest > strLineBelow.Length - 1)
        {
            return false;
        }
        char cCharacterToCheck = strLineBelow[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool checkDiagnolAboveFirst(string strLineAbove, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineAbove))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition - 1;

        if (iPositionToTest > strLineAbove.Length - 1 || iPositionToTest < 0)
        {
            return false;
        }

        char cCharacterToCheck = strLineAbove[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool checkDiagnolBelowFirst(string strLineBelow, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineBelow))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition - 1;

        if (iPositionToTest > strLineBelow.Length - 1 || iPositionToTest < 0)
        {
            return false;
        }
        char cCharacterToCheck = strLineBelow[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);
    }

    public static bool CheckDiagnolAboveLast(string strLineAbove, string strNumberString, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineAbove))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition + strNumberString.Length;

        if (iPositionToTest > strLineAbove.Length - 1 || iPositionToTest < 0)
        {
            return false;
        }
        char cCharacterToCheck = strLineAbove[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);

    }

    public static bool CheckDiagnolBelowLast(string strLineBelow, string strNumberString, int iPosition, List<char> lstDistinctSymbols)
    {
        if (string.IsNullOrEmpty(strLineBelow))
        {
            return false;
        }

        // check character before the string starts
        int iPositionToTest = iPosition + strNumberString.Length;

        if (iPositionToTest > strLineBelow.Length - 1 || iPositionToTest < 0)
        {
            return false;
        }
        char cCharacterToCheck = strLineBelow[iPositionToTest];

        return lstDistinctSymbols.Contains(cCharacterToCheck);

    }








    public static List<(string substring, int position)> GetSpecificDigitInfo(string input)
    {
        List<(string substring, int position)> result = new List<(string substring, int position)>();

        int startPos = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                startPos = i;
                int endPos = i;

                // Find the end position of the current sequence of digits
                while (endPos < input.Length && char.IsDigit(input[endPos]))
                {
                    endPos++;
                }

                string substring = input.Substring(startPos, endPos - startPos);
                result.Add((substring, startPos));

                i = endPos; // Skip the characters we've already processed
            }
        }

        return result;
    }

    public static List<char> FindNonDotOrNumberChars(string input)
    {
        // Define a regular expression pattern to match characters that are not dots or numbers
        string pattern = @"[^.\d]";

        // Use Regex.Matches to find all matches in the input string
        MatchCollection matches = Regex.Matches(input, pattern);

        // Convert the matches to a list of characters
        List<char> result = new List<char>();
        foreach (Match match in matches)
        {
            result.Add(match.Value[0]);
        }

        return result;
    }
}

public class LineInfo
{
    public string? LineData;
    public List<(string substring, int position)> NumberPositionList;

    public LineInfo(string strLine, List<(string substring, int position)> lstNumberPositionList)
    {
        LineData = strLine;
        NumberPositionList = lstNumberPositionList;
    }
}
