using System;
using System.IO;

namespace ArticleReader.ErrorHandler
{
    public static class ErrorLogger
    {
        /// <summary>
        /// Conventional debugging.
        /// Enable this method for conventional debugging locally.
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(String msg)
        {
            string filePath = @"D:\home\LogFiles\Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine("Message : " + msg);
                writer.WriteLine();
            }
        }
    }
}