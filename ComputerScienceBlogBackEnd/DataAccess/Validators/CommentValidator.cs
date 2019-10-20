using FluentValidation;

namespace ComputerScienceBlogBackEnd.DataAccess.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(r => r.Text).Length(0, 200).NotEmpty();
        }
    }
}
