﻿
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fastdo.Core.Services;

namespace Fastdo.Core.Utilities
{

    public partial class RequireConfirmedEmail:ValidationAttribute
    {
        public RequireConfirmedEmail(bool checkForEmailIfFound=false)
        {
            _checkForEmailIfFound = checkForEmailIfFound;
        }

        private bool _checkForEmailIfFound { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_checkForEmailIfFound)
            {
                var user = RequestStaticServices.GetDbContext().Users
                    .FirstOrDefaultAsync(u => u.Email.Equals(value.ToString())).Result;
                if(user==null)
                    return new ValidationResult("هذا البريد الالكترونى غير موجود",new List<string> {validationContext.MemberName});
                if(!user.EmailConfirmed)
                    return new ValidationResult($" بريدك الالكترونى غير مفعل ,من فضلك قم بتفعيلة", new List<string> { validationContext.MemberName });
            }
            else if(!BasicUtility.UserIdentifier().IsEmailConfirmed)
                  return new ValidationResult($"بريدك الالكترونى غير مفعل ,من فضلك قم بتفعيلة", new List<string> {"G"});        
            return ValidationResult.Success;
        }
    }
}
