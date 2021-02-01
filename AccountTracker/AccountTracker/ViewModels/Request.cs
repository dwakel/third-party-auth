using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AccountTracker.ViewModels
{
    public class LoginRequest
    {
        public LoginRequest()
        {
        }
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; set; }
        //[JsonIgnore]
        public string Password { get; set; }
    }

    internal class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        LoginRequestValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }
    }

    public class SignupRequest
    {
        public SignupRequest(string email, string password, string name, string description, string phoneNumber)
        {
            Email = email;
            Password = password;
            Name = name;
            Description = description;
            PhoneNumber = phoneNumber;
        }
        public SignupRequest()
        {
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
    }

    internal class SignupRequestValidator : AbstractValidator<SignupRequest>
    {
        SignupRequestValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
            //RuleFor(r => r.PhoneNumber) //Phone number validator require 
        }
    }
}
