using FluentValidation;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.ValidationRules;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adını boş geçmeyiniz.")
            .MinimumLength(2).WithMessage("En az 2 karakter giriniz.")
            .MaximumLength(50).WithMessage("En fazla 50 karakter giriniz");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Ürün fiyatı boş geçmeyiniz.")
            .GreaterThan(0).WithMessage("Ürün fiyatı 0 olamaz");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Ürün açıklaması boş geçmeyiniz.")
            .MinimumLength(2).WithMessage("En az 2 karakter giriniz.")
            .MaximumLength(50).WithMessage("En fazla 500 karakter giriniz");
    }
}
