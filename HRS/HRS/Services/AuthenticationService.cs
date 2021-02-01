using HRS.Domain;
using HRS.ViewModels;
using FluentResults;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Threading.Tasks;

namespace HRS.Services
{
    public interface IAuthenticationService
    {
        Task<Result<(string authToken, UserModel user)>> Login(LoginRequest credentials);
        Task<Result<(string authToken, UserDTO user)>> LoginUsing3rdParty(string email, string _3rdPtKey);
        Task<Result> Register(SignupRequest credentials);
        Task<Result<UserDTO>> Register3rdParty(string email, string _3rdPtKey);
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
            //var user = new UserModel()
            //{
            //    Name = "Selorm",
            //    Email = "avokeselorm@live.com",
            //    IsActive = true,
            //    PhoneNumber = "0200000001",
            //    Password = "string"
            //};

            if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
            {
                return Result.Fail("Invalid Credentials");
            }

            DateTimeOffset authTokenExpiry = DateTimeOffset.Now.AddHours(12);

            string authToken = new JwtBuilder()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret("1234567890") // todo: get secret from config
              .AddClaim("exp", authTokenExpiry.ToUnixTimeSeconds())
              .AddClaim("userId", user.Id)
              .Encode();



            return Result.Ok((authToken, user)).WithSuccess("Login Successful");
            

        }

        public async Task<Result<(string authToken, UserDTO user)>> LoginUsing3rdParty(string email, string _3rdPtKey)
        {
            UserDTO user = await _userRepository.FindUserDtoByEmail(email).ConfigureAwait(false);


            if (user == null || !user.Key.Equals(_3rdPtKey))
            {
                return Result.Fail("Invalid Credentials");
            }

            DateTimeOffset authTokenExpiry = DateTimeOffset.Now.AddHours(12);

            string authToken = new JwtBuilder()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret("1234567890") // todo: get secret from config
              .AddClaim("exp", authTokenExpiry.ToUnixTimeSeconds())
              .AddClaim("userId", user.Id)
              .Encode();



            return Result.Ok((authToken, user)).WithSuccess("Login Successful");


        }

        public async Task<Result> Register(SignupRequest credentials)
        {
            UserModel user = await _userRepository.AddUser(
                Name: credentials.Name,
                Email: credentials.Email,
                Password: BCrypt.Net.BCrypt.HashPassword(credentials.Password),
                Description: "new user",
                PhoneNumber: credentials.PhoneNumber,
                Gender: "female",
                IsActive: true).ConfigureAwait(false);


            return Result.Ok().WithSuccess("Signup Successful, Please Log in");


        }


        public async Task<Result<UserDTO>> Register3rdParty(string email, string _3rdPtKey)
        {
            UserDTO user = await _userRepository.FindUserDtoByEmail(email).ConfigureAwait(false);


            if (user == null)
            {
                return Result.Fail("Invalid Credentials");
            }

            ThirdPartyAuthModel tracker = await _userRepository.AddTraker(user.Id, _3rdPtKey, email).ConfigureAwait(false);



            user.Key = tracker.Key;

            //DateTimeOffset authTokenExpiry = DateTimeOffset.Now.AddHours(12);

            //string authToken = new JwtBuilder()
            //  .WithAlgorithm(new HMACSHA256Algorithm())
            //  .WithSecret("1234567890") // todo: get secret from config
            //  .AddClaim("exp", authTokenExpiry.ToUnixTimeSeconds())
            //  .AddClaim("userId", user.Id)
            //  .Encode();



            return Result.Ok(user).WithSuccess("Account link Successful");


        }
    }
}
