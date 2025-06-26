namespace WebApplication2.Models
{
    // Model widoku błędu – używany do przekazywania informacji o błędach do widoku
    public class ErrorViewModel
    {
        // Identyfikator żądania (Request ID), może być null
        public string? RequestId { get; set; }

        // Czy pokazać RequestId (true jeśli nie jest pusty)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}