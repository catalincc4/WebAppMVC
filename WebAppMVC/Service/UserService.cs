using System.Text;
using WebAppMVC.Models;
using WebAppMVC.Repositories;

using System.Security.Cryptography;
using System.Text;

namespace WebAppMVC.Service
{
    public class UserService
    {
        public readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async void Create(Users user)
        {
            await _unitOfWork.Users.Add(user);
        }
        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _unitOfWork.Users.GetAll();
        }
        public async Task<Users> Get(int id)
        {
            return await _unitOfWork.Users.Get(id);
        }

        public async void Update(Users user)
        {
            _unitOfWork.Users.Update(user);
        }

        public async void Delete(Users user)
        {
            _unitOfWork.Users.Delete(user);
        }

        public bool isDuplicate(int id)
        {

            return (_unitOfWork.Users.Get(id) == null) ? false : true;

        }



public static string HashPassword(string password)
    {
        // Convert the password string to a byte array
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Create a new instance of the SHA256 algorithm
        SHA256 sha256 = SHA256.Create();

        // Compute the hash value of the password
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);

        // Convert the hash byte array to a string and return it
        return Convert.ToBase64String(hashBytes);
    }
}
}
