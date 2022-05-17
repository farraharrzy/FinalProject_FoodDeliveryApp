namespace UserService.GraphQL
{
    public record ProfileData
    (
        int? Id,
        int UserId,
        string Name,
        string Address,
        string City,
        string Phone
    );
}
