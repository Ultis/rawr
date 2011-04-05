using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rawr.Base
{
    public class ErrorBox
    {
        #region Constructors
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
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        public ErrorBox(string title, string message, string sugfix = "")
        {
            Title = title;
            Message = message;
            InnerMessage = "";
            Function = "";
            Info = "";
            StackTrace = "";
            SuggestedFix = sugfix;
        }
#if FALSE
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="function">The Function throwing this Error</param>
        public ErrorBox(string title, string message, Exception innerException, string function, string sugfix = "")
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "";
            Function = function;
            Info = "";
            StackTrace = "";
            SuggestedFix = sugfix;
        }
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        public ErrorBox(string title, string message, Exception innerException, string function, string info, string sugfix = "")
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "";
            Function = function;
            Info = info;
            StackTrace = "";
            SuggestedFix = sugfix;
        }
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="stacktrace">The Stack Trace leading to this point</param>
        public ErrorBox(string title, string message, Exception innerException, string function, string info, string stacktrace, string sugfix = "")
        {
            Title = title;
            Message = message;
            InnerMessage = innerException != null ? innerException.Message : "";
            Function = function;
            Info = info;
            StackTrace = stacktrace;
        }
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="exception">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        public ErrorBox(string title, Exception exception, string function, string sugfix = "")
        {
            Title = title;
            Message = exception.Message;
            InnerMessage = exception.InnerException != null ? exception.InnerException.Message : "";
            Function = function;
            Info = "";
            SuggestedFix = sugfix;
            StackTrace = exception.StackTrace;
        }
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="exception">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        public ErrorBox(string title, Exception exception, string function, string info, string sugfix = "")
        {
            Title = title;
            Message = exception.Message;
            InnerMessage = exception.InnerException != null ? exception.InnerException.Message : "";
            Function = function;
            Info = info;
            SuggestedFix = sugfix;
            StackTrace = exception.StackTrace;
        }
#endif
        #endregion
        #region Variables
        public string Title = "";
        public string Message = "";
        public string InnerMessage = "";
        public string Function = "";
        public string Info = "";
        public string StackTrace = "";
        public string SuggestedFix = "";
        /// <summary>
        /// Setting TheException automatically sets Message, InnerMessage and StackTrace
        /// <para>You still need to set Title, Info, Function and SuggestedFix</para>
        /// </summary>
        public Exception TheException {
            set {
                if (value != null) {
                    Message = value.Message;
                    InnerMessage = value.InnerException != null ? value.InnerException.Message : "";
                    StackTrace = value.StackTrace;
                }
            }
        }
        #endregion
        #region Functions
        public string buildFullMessage()
        {
            string retVal = "";
            retVal += Function     != "" ? string.Format("Function: {0}\r\n\r\n",                Function    ) : "";
            retVal += Message      != "" ? string.Format("Error Message: {0}\r\n",               Message     ) : "";
            retVal += InnerMessage != "" ? string.Format("\r\nInner Error Message: {0}\r\n\r\n", InnerMessage) : "";
            retVal += Info         != "" ? string.Format("Info: {0}\r\n\r\n",                    Info        ) : "";
            retVal += SuggestedFix != "" ? string.Format("Suggested Fix:\r\n{0}",                SuggestedFix) : "";
            retVal += StackTrace   != "" ? string.Format("Stack Trace:\r\n{0}",                  StackTrace  ) : "";
            return retVal;
        }
        public void Show() {
            try {
#if RAWRSERVER
                System.Windows.MessageBox.Show(buildFullMessage(), Title, MessageBoxButton.OK);
#else
                ErrorWindow ew = new ErrorWindow() {
                    ErrorMessage = this.Message + (this.InnerMessage != "" ? "\n" + this.InnerMessage : ""),
                    StackTrace = this.StackTrace,
                    SuggestedFix = this.SuggestedFix,
                    Info = this.Info,
                };
#if SILVERLIGHT
                ew.Show();
#else
                ew.ShowDialog();
#endif

#endif
                System.Diagnostics.Debug.WriteLine(Title + "\n" + buildFullMessage());
            }catch(Exception){ }
        }
        #endregion
    }
}
