using ExpenseTrackerAPI.DTO;  // Import the DTOs
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
   public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly JwtService _jwtService;

    public UserService(IUserRepository userRepository, PasswordHasher<User> passwordHasher, JwtService jwtService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }

public async Task<string> RegisterAsync(RegisterRequest registerRequest)
{
    if (registerRequest == null)
        throw new ArgumentNullException(nameof(registerRequest));

    if (string.IsNullOrWhiteSpace(registerRequest.Email))
        throw new ArgumentException("Email cannot be null or empty.", nameof(registerRequest.Email));

    if (string.IsNullOrWhiteSpace(registerRequest.Password))
        throw new ArgumentException("Password cannot be null or empty.", nameof(registerRequest.Password));

    // Vérifiez si un utilisateur existe déjà avec cet email
    var existingUser = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
    if (existingUser != null)
        throw new InvalidOperationException("Email is already taken.");

    // Création de l'utilisateur
    var user = new User
    {
        Email = registerRequest.Email,
        FirstName = registerRequest.FirstName ?? string.Empty,
        LastName = registerRequest.LastName ?? string.Empty,
        PasswordHash = _passwordHasher.HashPassword(new User(), registerRequest.Password) // Hash du mot de passe
    };

    // Ajoutez l'utilisateur dans la base de données
    var createdUser = await _userRepository.AddUserAsync(user);

    // Vérification finale
    if (createdUser == null)
        throw new InvalidOperationException("User registration failed.");

    // Générer le token JWT après la création de l'utilisateur
    var token = _jwtService.GenerateJwtToken(createdUser);

    // Retourner le token
    return token;
}



    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        if (loginRequest == null)
            throw new ArgumentNullException(nameof(loginRequest));

        if (string.IsNullOrWhiteSpace(loginRequest.Password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(loginRequest.Password));
             if (string.IsNullOrWhiteSpace(loginRequest.Email))
        throw new ArgumentException("Email cannot be null or empty.", nameof(loginRequest.Email));


        var existingUser = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
        if (existingUser == null)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash ?? string.Empty, loginRequest.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid username or password.");

        return _jwtService.GenerateJwtToken(existingUser);
    }
}

}
