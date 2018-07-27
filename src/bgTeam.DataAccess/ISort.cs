namespace bgTeam.DataAccess
{
    public interface ISort
    {
        string PropertyName { get; set; }

        bool Ascending { get; set; }
    }
}
