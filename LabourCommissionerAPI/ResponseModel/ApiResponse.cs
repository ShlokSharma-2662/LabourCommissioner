namespace LabourCommissionerAPI.ResponseModel
{
    public class ApiResponse
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public string StackTrace { get; set; }
    }
}
