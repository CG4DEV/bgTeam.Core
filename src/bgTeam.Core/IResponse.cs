namespace bgTeam
{
    public interface IResponse<T>
    {
        bool Success { get; set; }

        string Message { get; set; }

        T Data { get; set; }
    }
}
