using FluentValidation;

namespace ComputerScienceBlogBackEnd.DataAccess.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(r => r.Text).Length(10, 2000);
            RuleFor(r => r.Title).Length(0, 60).NotEmpty();
            RuleFor(r => r.CreatedUser).NotEmpty();
        }
    }
}
