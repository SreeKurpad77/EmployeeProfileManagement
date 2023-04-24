namespace EmployeeProfileManagement.Core.Model
{
    public class ResultObject<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public string ErrorMessage { get; set; }

    }
}
