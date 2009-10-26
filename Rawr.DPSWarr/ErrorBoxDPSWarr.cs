using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class ErrorBoxDPSWarr
    {
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        public ErrorBoxDPSWarr()
        {
            Title = "There was an Error";
            Message = "No Message";
            Function = "No Function Name";
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Line = 0;
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="stacktrace">The Stack Trace leading to this point</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function, string info, string stacktrace, int line)
        {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = stacktrace;
            Info = info;
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function, string info, int line)
        {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = info;
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function, int line)
        {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function)
        {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Line = 0;
            Show();
        }
        public string Title;
        public string Message;
        public string Function;
        public string Info;
        public string StackTrace;
        public int Line;
        private string buildFullMessage()
        {
            string retVal = "";
            retVal += Function + " Line: " + Line.ToString();
            retVal += "\r\n\r\n";
            retVal += "Error Message: " + Message;
            retVal += "\r\n\r\n";
            retVal += "Info: " + Info;
            retVal += "\r\n\r\n";
            retVal += "Stack Trace:\r\n" + StackTrace;
            return retVal;
        }
        public void Show()
        {
#if RAWR3
            //new ErrorWindow() { Message = Title + "\r\n\r\n" + buildFullMessage() }.Show();
#else
            System.Windows.Forms.MessageBox.Show(buildFullMessage(), Title);
#endif
            Console.WriteLine(Title + "\n" + buildFullMessage());
        }
    }
}
