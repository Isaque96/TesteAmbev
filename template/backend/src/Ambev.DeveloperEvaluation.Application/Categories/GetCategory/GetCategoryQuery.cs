using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.GetCategory;

public class GetCategoryQuery : IRequest<GetCategoryResult>
{
    public Guid Id { get; set; }
}