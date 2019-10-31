using TransactionWebApp.Models;

namespace TransactionWebApp.FileHandler
{
    public interface IFileHandler
    {
        TransactionModel GetTranscations(string path, string uploadedFileName);
    }
}
