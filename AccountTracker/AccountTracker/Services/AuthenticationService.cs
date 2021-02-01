using AccountTracker.Domain;
using AccountTracker.ViewModels;
using FluentResults;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Threading.Tasks;

namespace AccountTracker.Services
{
    public interface IAuthenticationService
    {
        Task<Result<(string authToken, UserModel user)>> Login(LoginRequest credentials);

        Task<Result> Register(SignupRequest credentials);
    }
    class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<(string authToken, UserModel user)>> Login(LoginRequest credentials)
        {
            UserModel user = await _userRepository.FindUserByEmail(credentials.Email).ConfigureAwait(false);

            if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
            {
                return Result.Fail("Invalid Credentials");
            }

            //if (user == null || !user.Password.Equals(credentials.Password))
            //{

            //}

            DateTimeOffset authTokenExpiry = DateTimeOffset.Now.AddHours(12);

            string authToken = new JwtBuilder()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret("1234567890") // todo: get secret from env.
              .AddClaim("exp", authTokenExpiry.ToUnixTimeSeconds())
              .AddClaim("userId", user.Id)
              .Encode();

            return Result.Ok((authToken, user)).WithSuccess("Login Successful");
            

        }

        public async Task<Result> Register(SignupRequest credentials)
        {
            UserModel exist = await _userRepository.FindUserByEmail(credentials.Email).ConfigureAwait(false);

            if (exist is not null)
            {
                return Result.Fail(new Error($"User with email {credentials.Email} already exist"));
            }

            UserModel user = await _userRepository.AddUser(
                Name: credentials.Name,
                Email: credentials.Email,
                Password: BCrypt.Net.BCrypt.HashPassword(credentials.Password),
                Description: "new user",
                PhoneNumber: credentials.PhoneNumber,
                Gender: "female", //for brevity default to female, because im not validating string. This will usually be an enum or it can be a string
                IsActive: true).ConfigureAwait(false);

            if (user is null)
            {
                return Result.Fail(new Error("Failed to register a new account"));
            }


            return Result.Ok().WithSuccess("Signup Successful, Please Log in");


        }

    }
}
