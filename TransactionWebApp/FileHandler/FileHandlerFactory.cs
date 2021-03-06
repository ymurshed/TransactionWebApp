﻿using System.IO;
using TransactionWebApp.Utility.Constants;

namespace TransactionWebApp.FileHandler
{
    public class FileHandlerFactory
    {
        public static IFileHandler GetFileHandler(string path)
        {
            IFileHandler fileHandler = null;
            var fileExtension = Path.GetExtension(path);

            switch (fileExtension)
            {
                case CommonConstant.Csv:
                    fileHandler = new CsvHandler();
                    break;

                case CommonConstant.Xml:
                    fileHandler = new XmlHandler();
                    break;
            }
            return fileHandler;
        }
    }
}
