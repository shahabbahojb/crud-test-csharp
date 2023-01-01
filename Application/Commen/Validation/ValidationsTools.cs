using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.Common.Validation
{
    public static class ValidationsTools
    {
        public static bool Failed(this ValidationResult validationResult)
        {
            return !validationResult.IsValid;
        }

        public static IEnumerable<ValidationFailure> Messages(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(x => new ValidationFailure
            {
                PropertyName = x.PropertyName,
                ErrorMessage = x.ErrorMessage,
            }).ToList();
        }

        public static ValidationResult FromFluentValidationResult(ValidationResult result)
        {
            return new ValidationResult(result.Errors);
        }

        public static IRuleBuilderOptions<T, TProperty> WhenNotNull<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.Configure(config =>
                config.ApplyCondition(x => config.GetPropertyValue(x.InstanceToValidate) != null)
            );
        }

        public static IRuleBuilderOptions<T, TProperty> WithDefaultMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule
        )
        {
            return rule.WithMessage(Validator<T, DbContext>.Default);
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches("^(09)[0-9]{9}$");
        }

        public static IRuleBuilderOptions<T, string> Link<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(
                @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})");
        }

        public static IRuleBuilderOptions<T, int> Hour<T>(this IRuleBuilder<T, int> rule)
        {
            return rule.Must(x => x >= 0 && x <= 24);
        }

        public static IRuleBuilderOptions<T, int> Minute<T>(this IRuleBuilder<T, int> rule)
        {
            return rule.Must(x => x >= 0 && x <= 60);
        }

        public static IRuleBuilderOptions<T, string> Gender<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(x => x == "مرد" || x == "زن");
        }

        public static IRuleBuilderOptions<T, string> NationalCode<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches("^[0-9]{10}$");
        }

        public static IRuleBuilderOptions<T, string> Number<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches("^[0-9]+$");
        }

        public static IRuleBuilderOptions<T, string> Number<T>(this IRuleBuilder<T, string> rule, int digitCount)
        {
            return rule.Matches($"^[0-9]{{{digitCount}}}$");
        }

        public static IRuleBuilderOptions<T, IFormFile> MaxSize<T>(this IRuleBuilder<T, IFormFile> rule, long size)
        {
            return rule.Must(x => x.Length <= size);
        }

        public static IRuleBuilderOptions<T, IFormFile> SupportedTypes<T>(this IRuleBuilder<T, IFormFile> rule,
            IEnumerable<string> types)
        {
            return rule.Must(x => types.Contains(x.ContentType));
        }

        public static IRuleBuilderOptions<T, TProperty> Include<T, TProperty>(this IRuleBuilder<T, TProperty> rule,
            IEnumerable<TProperty> values)
        {
            return rule.Must(values.Contains);
        }

        public static IRuleBuilderOptions<T, TProperty> Include<T, TProperty>(this IRuleBuilder<T, TProperty> rule,
            TProperty[] values)
        {
            return rule.Must(values.Contains);
        }

        public static IRuleBuilderOptions<T, TProperty> IncludeAsync<T, TProperty>(this IRuleBuilder<T, TProperty> rule,
            Task<IEnumerable<TProperty>> values)
        {
            return rule.MustAsync(async (x, _) => (await values).Contains(x));
        }

        public static IRuleBuilderOptions<T, TProperty> IncludeAsync<T, TProperty>(this IRuleBuilder<T, TProperty> rule,
            Task<TProperty[]> values)
        {
            return rule.MustAsync(async (x, _) => (await values).Contains(x));
        }

        public static IRuleBuilderOptions<T, IFormFile> Image<T>(this IRuleBuilder<T, IFormFile> rule)
        {
            return rule.Must(x => new List<string>
                {
                    "image/jpeg",
                    "image/jpg",
                    "image/png",
                    "image/webp",
                    "image/svg+xml",
                    "image/HEIC"
                }.Contains(x.ContentType)
            );
        }

        public static IRuleBuilderOptions<T, IFormFile> ImageOrPdf<T>(this IRuleBuilder<T, IFormFile> rule)
        {
            return rule.Must(x => new List<string>
                {
                    "image/jpeg",
                    "image/jpg",
                    "image/png",
                    "image/webp",
                    "image/svg+xml",
                    "application/pdf"
                }.Contains(x.ContentType)
            );
        }
        
        public static IRuleBuilderOptions<T, IFormFile> Pdf<T>(this IRuleBuilder<T, IFormFile> rule)
        {
            return rule.Must(x => new List<string>
                {
                    "application/pdf"
                }.Contains(x.ContentType)
            );
        }

        public static IRuleBuilderOptions<T, IFormFile> Voice<T>(this IRuleBuilder<T, IFormFile> rule)
        {
            return rule.Must(x => new List<string>
                {
                    "audio/mpeg",
                    "audio/aac",
                    "audio/mp3"
                }.Contains(x.ContentType)
            );
        }

        public static IRuleBuilderOptions<T, IFormFile> Video<T>(this IRuleBuilder<T, IFormFile> rule)
        {
            return rule.Must(x => new List<string>
                {
                    "video/mp4",
                    "video/mkv",
                    "video/avi",
                    "video/3gp",
                    "video/mwv",
                }.Contains(x.ContentType)
            );
        }


  



        public static IRuleBuilderOptions<T, short> Percent<T>(this IRuleBuilder<T, short> rule)
        {
            return rule.Must(x => x is >= 0 and <= 100);

            // return rule.MustAsync(async (x,y,_)=> y.CompareTo(number>=100));
        }

   

        public static IRuleBuilderOptions<T, TProperty> Count<T, TProperty>(this IRuleBuilder<T, TProperty> rule,
            int count)
            where TProperty : ICollection
        {
            return rule.Must(x => x.Count == count);
        }

        public static IRuleBuilderOptions<T, TProperty> CountAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> rule, Task<int> predicate) where TProperty : ICollection
        {
            return rule.MustAsync(async (x, _) => x.Count == await predicate);
        }

        public static IRuleBuilderOptions<T, TProperty> LessThanOrEqualToAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> rule, Task<TProperty> predicate)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return rule.MustAsync(async (x, _) => x.CompareTo(await predicate) <= 0);
        }

        public static IRuleBuilderOptions<T, TProperty> LessThanAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> rule, Task<TProperty> predicate)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return rule.MustAsync(async (x, _) => x.CompareTo(await predicate) < 0);
        }

        public static IRuleBuilderOptions<T, TProperty> GreaterThanOrEqualToAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> rule, Task<TProperty> predicate)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return rule.MustAsync(async (x, _) => x.CompareTo(await predicate) >= 0);
        }

        public static IRuleBuilderOptions<T, TProperty> GreaterThanAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> rule, Task<TProperty> predicate)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return rule.MustAsync(async (x, _) => x.CompareTo(await predicate) > 0);
        }

    

        public static string GenerateMessage(string messageKey, string name,string otherName=null, string language = "fa")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ValidationMessages.json");
            List<ValidationMessageModel> items = null;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<ValidationMessageModel>>(json);
            }

            var item = items.FirstOrDefault(x => x.Key == messageKey);

            if (item == null)
            {
                return $"{name} نامعتبر می باشد";
            }

            var message = item.Value.Replace("#KEY", name);

            if (otherName!=null)
            { 
                message = message.Replace("#VALUE", otherName);
            }

            return message;
        }
    }
}