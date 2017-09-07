using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;

namespace ReportHelper
{
    public static class ReportHelper
    {
        public static string TextWrapper(string inputText, double cellWidthCM = 1, int fontSize = 10,string fontFamily ="Arial", int numberOfRows = 3,bool noIndent = false, bool isDebugMode = false)
        {
            if (isDebugMode)
            {
                Debugger.Break();
            }
            if (string.IsNullOrEmpty(inputText))
                return "";

            try
            {
                var inputTypeFace = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                double cellWidth = cellWidthCM / 2.54;
                double actualWidth = noIndent ? (cellWidth-0.1)*0.75 : (cellWidth - 0.2)*0.75;       // + buffer
                string[] rowContent = new string[numberOfRows];

                for (int i = 0; i <= numberOfRows - 1; i++)
                    rowContent[i] = "";

                var words = inputText.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ").Split(' ');

                string current = "";   // Text in the current row
                int currentRow = 0;    // index of the curent row
                try
                {
                    for (int i = 0; i <= words.Length - 1; i++)
                    {
                        
                        var word = words[i];
                        FormattedText formattedText = new FormattedText(string.Format("{0} {1}", current, word), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                        double rowLenght = (string.IsNullOrEmpty(current) ? 0 : formattedText.Width/96);
                        if (rowLenght < actualWidth)    // fits in current row
                        {
                            current = string.IsNullOrEmpty(current) ? word : string.Format("{0} {1}", current, word);
                        }
                        else if (currentRow == numberOfRows - 1)   // doesn't fit and current is the last row
                        {
                            current = string.IsNullOrEmpty(current) ? word : string.Format("{0} {1}", current, word);
                            formattedText = new FormattedText(current,CultureInfo.CurrentCulture,FlowDirection.LeftToRight,inputTypeFace,fontSize,Brushes.Black);
                            rowLenght = formattedText.Width/96;
                            while (rowLenght > actualWidth)
                            {
                                var length = current.Length - 4;
                                current = current.Substring(0, length) + "...";
                                formattedText = new FormattedText(current, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                                rowLenght = formattedText.Width / 96;
                            }
                            rowContent[currentRow] = current;
                            break;
                        }
                        else      // doesn't fit, but there ara other rows
                        {
                            if (string.IsNullOrEmpty(current))
                            {
                                current = word;
                            }
                            formattedText = new FormattedText(current, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                            rowLenght = formattedText.Width/96;
                            string other = "";
                            var original = current;
                            while (rowLenght > actualWidth)
                            {
                                var length = current.Length - 1;
                                other = original.Substring(length);
                                current = current.Substring(0, length);
                                formattedText = new FormattedText(current, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                                rowLenght = formattedText.Width/96;
                            }
                            rowContent[currentRow] = current;
                            current = (string.IsNullOrEmpty(other) ? word : other + " " + word);
                            currentRow = currentRow + 1;
                        }
                        if (i == words.Length - 1)     // done with the last word, put current row in
                        {
                            rowContent[currentRow] = current;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.StackTrace;
                }
                string actualContent = "";
                foreach (string row in rowContent)
                    actualContent = string.Format("{0}{1} ", actualContent, row);

                FormattedText entireText = new FormattedText(actualContent, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                double entireWidth = entireText.Width / 96;
               // while (System.Windows.Forms.TextRenderer.MeasureText(actualContent, inputFont).Width > cellWidth*3)
                while (entireWidth > actualWidth*numberOfRows)
                {
                    actualContent = actualContent.Substring(0, actualContent.Length - 4) + "...";
                    entireText = new FormattedText(actualContent, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, inputTypeFace, fontSize, Brushes.Black);
                    entireWidth = entireText.WidthIncludingTrailingWhitespace / 96;
                }

                return actualContent;
            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.StackTrace;
            }
        }

    }
}
