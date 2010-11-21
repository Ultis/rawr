using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rawr.Base
{
    public class ErrorBox
    {
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        public ErrorBox()
        {
            Title = "There was an Error";
            Message = "No Message";
            InnerMessage = "No Inner Message";
            Function = "No Function Name";
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innermessage">The Error Message of the inner message</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="stacktrace">The Stack Trace leading to this point</param>
        public ErrorBox(string title, string message, Exception innerException, string function, string info, string stacktrace)
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "No Inner Exception";
            Function = function;
            StackTrace = stacktrace;
            Info = info;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innermessage">The Error Message of the inner message</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        public ErrorBox(string title, string message, Exception innerException, string function, string info)
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "No Inner Exception";
            Function = function;
            StackTrace = "No Stack Trace";
            Info = info;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innermessage">The Error Message of the inner message</param>
        /// <param name="function">The Function throwing this Error</param>
        public ErrorBox(string title, string message, Exception innerException, string function)
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "No Inner Exception";
            Function = function;
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Show();
        }
        public string Title;
        public string Message;
        public string InnerMessage;
        public string Function;
        public string Info;
        public string StackTrace;
        private string buildFullMessage()
        {
            string retVal = "";
            retVal += Function;
            retVal += "\r\n\r\n";
            retVal += "Error Message: " + Message;
            retVal += "\r\n";
            retVal += "Inner Error Message: " + InnerMessage;
            retVal += "\r\n\r\n";
            retVal += "Info: " + Info;
            retVal += "\r\n\r\n";
            retVal += "Stack Trace:\r\n" + StackTrace;
            return retVal;
        }
        public void Show()
        {
//#if DEBUG
            try
            {
                System.Windows.MessageBox.Show(Message = Title + "\r\n\r\n" + buildFullMessage());
                if (Function == "ErrorBox.Show()") { return; }
                Console.WriteLine(Title + "\n" + buildFullMessage());
                if (Application.Current.HasElevatedPermissions)
                {
                    System.IO.StreamWriter file = System.IO.File.CreateText("DEBUGME.log");
                    file.Write("\n=====" + System.DateTime.Now.ToShortDateString() + "\n" + Title + "\n" + buildFullMessage() + "\n");
                    file.Close();
                }
            }catch(Exception ex){
                ErrorBox eb = new ErrorBox("Error creating the ErrorBox", ex.Message, ex.InnerException, "ErrorBox.Show()", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
//#endif
        }
    }
}
