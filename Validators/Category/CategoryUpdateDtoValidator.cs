using FluentValidation;
using training_catalog_api.DTO.Category;

namespace training_catalog_api.Validators.Category
{
    public class CategoryUpdateDtoValidator:AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı boş olamaz.");
        }
    }
}