namespace TransactionWebApp.Constants
{
    public class ErrorConstant
    {
        public const string UnknownFileFormat = "Unknown format";
        public static string FileSizeExceeds = "File size greater than: ";
        public static string EmptyFile = "Empty file uploaded!";
        public static string FileUploadFailed = "File upload failed!";
        public static string NoFileSelected = "Please select a file!";

        public static string SaveTransactionError = "Error saving transaction. Error details: ";
        public static string GetTransactionErrorByCode = "Error getting transaction by currency code. Error details: ";
        public static string GetTransactionErrorByDateRange = "Error getting transaction by date range. Error details: ";
        public static string GetTransactionErrorByStatus = "Error getting transaction by status. Error details: ";

    }
}
