namespace WebASPMvcCore.Application.DTOs
{
    public class ResponseDatatable<T>
    {
        public int RecordsTotal { get; set; }
        public int Page { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
