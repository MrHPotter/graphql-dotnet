using GraphQL.Federation.CodeFirst.Sample3.Models;
using GraphQL.Types;

namespace GraphQL.Federation.CodeFirst.Sample3.Schema;

public class UserGraphType : ObjectGraphType<User>
{
    public UserGraphType()
    {
        this.Key("id");
        this.ResolveReference((_, source) => source); // User.Id is provided through the source object and no other properties exist
        Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>));
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<ReviewGraphType>>>, IEnumerable<Review>>("reviews")
            .ResolveAsync(ctx =>
            {
                var data = ctx.RequestServices!.GetRequiredService<Data>();
                return data.GetReviewsByUserIdAsync(ctx.Source.Id)!;
            });
    }
}