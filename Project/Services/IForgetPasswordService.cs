using Project.DTOs;

namespace Project.Services
{
    public interface IForgetPasswordService
    {
        public Guid ChangePassword(ForgetPasswordDto forgetPasswordDto);
        public string ChnagePasswordRequest(ForgetPasswordRequestDto forgetPasswordRequestDto);
    }
}
