using System;
using Microsoft.AspNetCore.Mvc;
using StudyFiles.Auth.Suppliers;
using StudyFiles.DTO.Auth;
using StudyFiles.DTO.Requests.Auth;
using StudyFiles.Logging;

namespace StudyFiles.Auth.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<AuthController>();

        private readonly IAuthSupplier _supplier;

        public AuthController(IAuthSupplier supplier)
        {
            _supplier = supplier;
        }

        [HttpPost]
        [Route("register")]
        public bool Register(LoginRequest request)
        {
            try
            {
                return _supplier.Register(request.Email, request.Login, request.Password);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With login: {request.Login}");
                throw;
            }
        }

        [HttpPost]
        [Route("login")]
        public LoginDTO Authenticate(LoginRequest request)
        {
            try
            {
                return _supplier.Authenticate(request.Login, request.Password);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With login: {request.Login}");
                throw;
            }
        }

        [HttpGet]
        [Route("restoreSession")]
        public LoginDTO RestoreSession(int id)
        {
            try
            {
                return _supplier.RestoreSession(id);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With id: {id}");
                throw;
            }
        }

        [HttpPut]
        [Route("logout")]
        public void Logout(int id)
        {
            try
            {
                _supplier.Logout(id);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With id: {id}");
                throw;
            }
        }

        [HttpPost]
        [Route("validate")]
        public bool ValidateToken(SessionRequest request)
        {
            try
            {
                return _supplier.ValidateToken(request.ID, request.Token);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With id: {request.ID}");
                throw;
            }
        }
    }
}
