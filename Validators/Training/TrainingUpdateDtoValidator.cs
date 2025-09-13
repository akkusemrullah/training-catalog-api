using System.Data;
using FluentValidation;
using training_catalog_api.DTO.Training;

namespace training_catalog_api.Validators.Training
{
    public class TrainingUpdateDtoValidator : AbstractValidator<TrainingUpdateDto>
    {
        public TrainingUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık alanı zorunludur.")
            .MaximumLength(120);

            RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Kısa açıklama alanı zorunludur.")
            .MaximumLength(280);

            RuleFor(x => x.LongDescription)
            .NotEmpty().WithMessage("Uzun açıklama zorunludur.");

            RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Bitiş tarihi başlangıç tarihinden küçük olamaz.");
            
            RuleFor(x => x.ImageUrl)
            .Must(UrlValid).When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("Geçersiz resim");
        }

        private bool UrlValid(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}