using Business_Tier.Utiltiy;
using Interface_Tier.DTOs.User_DTOs;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Service.Internal_service.Auths_Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    public class UserService(IUserRepository repository, ICryptographyService cryptography,
        IImageService imageService, IAccessTokenService tokenService, IRefreshTokenService refreshTokenService) : IUserService
    {
        public async Task<int> AddUser(AddUserServiceDTO user)
        {
            // check is it image
            CheckTypeOfImage(user.image);

            // check is exists email 
            bool res = await IsEmailVerifiedExists(user.Email);
            if (res)
                throw new Exception($"This email is already exists {user.Email} , choose another email ");

            //upload image to thired parties
            string ImageURL = await imageService.UploadImageAsync(user.image);
            if (string.IsNullOrEmpty(ImageURL))
                throw new Exception("An error occurred while post image in third parties");


            // encryption password
            string HashedPassword = cryptography.HashPassword(user.Password);


            AddUserRepositoryDTO addUserRepositoryDTO = new AddUserRepositoryDTO 
            { 
                Email = user.Email,
                Password = HashedPassword,
                ImagePath = ImageURL,
                Username = user.Username
            };
            
            return await repository.AddUser(addUserRepositoryDTO);
        }

        public async Task<string> GetAccessToken(string RefreshToken)
        {
            return await tokenService.GetAccessToken(RefreshToken);
        }

        public async Task<SimpleUserDataDTO> GetSimpleUserData(int UserID)
        {
            return await repository.GetSimpleUserData(UserID);
        }

        public async Task<UserDTO> GetUser(int UserID)
        {
            return await repository.GetUser(UserID);
        }

        public async Task<int> GetUserIDByEmail(string email)
        {
            return await repository.GetUserIDByEmail(email);
        }

        public async Task<bool> IsEmailVerifiedExists(string email)
        {
            return await repository.IsEmailVerifiedExists(email);
        }

        public async Task<bool> IsPasswordMatched(int UserID, string Password)
        {
            // Hashed password to Matching with HashedPassword in db
            string HashedPassword = cryptography.HashPassword(Password);
            return await repository.IsPasswordMatched(UserID, HashedPassword);
        }

        public async Task<(string AccessToken, string RefreshToken)> Login(string email, string password)
        {
            // Hashed password to Matching with HashedPassword in db
            string HashedPassword = cryptography.HashPassword(password);
            int ID = await repository.Login(email, HashedPassword);
            if (ID > 0)
            {
                (bool IsAdded, string Token) = await refreshTokenService.AddNewRefreshToken(ID);
                if (IsAdded)
                {
                    string AccessToken = tokenService.GenerateAccessToken(ID);
                    return (AccessToken, Token);
                }
            }
            return (null, null);
        }

        public async Task<bool> UpdatePassword(int UserID, string OldPassword, string Password)
        {
            bool res = await IsPasswordMatched(UserID, OldPassword);
            if (!res) return false;
            
            // Hashed password to Matching with HashedPassword in db
            string HashedPassword = cryptography.HashPassword(Password);

            return await repository.UpdatePassword(UserID, HashedPassword);
        }

        public async Task<bool> UpdateUser(UpdateUserServiceDTO dTO)
        {
            UpdateUserRepositoryDTO updateUser = new UpdateUserRepositoryDTO()
            {
                Email = dTO.Email,
                UserID = dTO.UserID,
                Username = dTO.Username,
            };

            if (dTO.image != null)
            {
                // check is it image
                CheckTypeOfImage(dTO.image);
                //upload image to thired parties
                string ImageURL = await imageService.UploadImageAsync(dTO.image);
                if (string.IsNullOrEmpty(ImageURL))
                    throw new Exception("An error occurred while post image in third parties");

                updateUser.ImagePath = ImageURL;
            }
            
            return await repository.UpdateUser(updateUser);
        }

        private void CheckTypeOfImage(IFormFile image)
        {
            if (!ImageType.allowedTypes.Contains(image.ContentType))
            {
                throw new Exception("*entire only image");
            }

        }
    }
}
