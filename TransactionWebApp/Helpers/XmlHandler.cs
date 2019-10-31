using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Helpers
{
    public class XmlHandler : IFileHandler
    {
        public TransactionModel GetTranscations(string path, string uploadedFileName)
        {
            var xmlModels = Parse(path);
            if (!xmlModels.Any())
            {
                //TODO: Add log
                return null;
            }

            var transcations = new List<Transactions>();
            var errors = GetErrors(xmlModels, uploadedFileName);

            if (!errors.Any())
            {
                foreach (var xmlModel in xmlModels)
                {
                    transcations.Add(xmlModel.ToTransaction());
                }
            }

            var transactionModel = new TransactionModel {Transcations = transcations, Errors = errors};
            return transactionModel;
        }

        private static IList<string> GetErrors(IList<XmlModel> xmlModels, string uploadedFileName)
        {
            var errors = new List<string>();
            for (var i = 0; i < xmlModels.Count; i++)
            {
                var error = xmlModels[i].IsValid(i + 1, uploadedFileName);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    errors.Add(error);
                }
            }
            return errors;
        }

        private static IList<XmlModel> Parse(string path)
        {
            List<XmlModel> xmlModels = null;
            try
            {
                var serializer = new XmlSerializer(typeof(XmlRootModel));
                if (serializer.Deserialize(new StreamReader(path)) is XmlRootModel result)
                {
                    xmlModels = result.XmlModels;
                }
            }
            catch (Exception e)
            {
                //Todo: Add log
            }
            return xmlModels;
        }
    }
}
