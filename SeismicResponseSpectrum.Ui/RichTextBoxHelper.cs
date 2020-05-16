using System.IO;
using System.Text;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace SeismicResponseSpectrum.Ui
{
    public class RichTextBoxHelper : DependencyObject
    {
        #region Highlighted Line Number

        public static readonly DependencyProperty HighlightedLineNumberProperty =
            DependencyProperty.RegisterAttached(
                "HighlightedLineNumber",
                typeof (int),
                typeof (RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = (obj, e) =>
                    {
                        var richTextBox = (RichTextBox) obj;

                        var lineNumber = (int) richTextBox.GetValue(HighlightedLineNumberProperty);

                        SelectLine(lineNumber, richTextBox);
                    }
                });

        public static void SetHighlightedLineNumber(UIElement element, int value)
        {
            element.SetValue(HighlightedLineNumberProperty, value);
        }

        public static int GetHighlightedLineNumber(UIElement element)
        {
            return (int) element.GetValue(HighlightedLineNumberProperty);
        }

        #endregion

        #region HighlightedBackground

        public static readonly DependencyProperty HighlightedBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "HighlightedBackground",
                typeof(Brush),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = Brushes.Chartreuse,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = (obj, e) =>
                    {
                        var richTextBox = (RichTextBox)obj;

                        var lineNumber = (int)richTextBox.GetValue(HighlightedLineNumberProperty);

                        SelectLine(lineNumber, richTextBox);
                    }
                });

        public static void SetHighlightedBackground(UIElement element, Brush value)
        {
            element.SetValue(HighlightedBackgroundProperty, value);
        }

        public static Brush GetHighlightedBackground(UIElement element)
        {
            return (Brush)element.GetValue(HighlightedBackgroundProperty);
        }

        #endregion


        public static void SelectLine(int line,RichTextBox editor)
        {
            int c = 0;
            TextRange r;

            var highlightedBackground = editor.GetValue(HighlightedBackgroundProperty);

            if (highlightedBackground == null)
                return;


            if (line < 0)
            {
                foreach (var item in editor.Document.Blocks)
                {
                    //if (line == c)
                    {
                        r = new TextRange(item.ContentStart, item.ContentEnd);
                        if (r.Text.Trim().Equals(""))
                        {
                            continue;
                        }

                        if (Equals(r.GetPropertyValue(TextElement.BackgroundProperty), highlightedBackground))
                        {
                            r.ApplyPropertyValue(TextElement.BackgroundProperty, editor.Background);
                            r.ApplyPropertyValue(TextElement.ForegroundProperty, editor.Foreground);
                            return;
                        }
                        //

                    }
                    c++;
                }
            }

            foreach (var item in editor.Document.Blocks)
            {
                if (line == c)
                {
                    r = new TextRange(item.ContentStart, item.ContentEnd);
                    if (r.Text.Trim().Equals(""))
                    {
                        continue;
                    }
                    r.ApplyPropertyValue(TextElement.BackgroundProperty, highlightedBackground);
                    r.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
                    return;
                }
                c++;
            }
        }
    }
}