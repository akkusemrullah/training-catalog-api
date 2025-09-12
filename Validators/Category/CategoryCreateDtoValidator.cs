using FluentValidation;
using training_catalog_api.DTO.Category;

namespace training_catalog_api.Validators.Category
{
    public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı boş olamaz.");
        }
    }
}